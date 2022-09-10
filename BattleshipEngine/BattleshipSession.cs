using BattleshipEngine.BattleshipEngineExceptions;
using BattleshipEngine.Maps;
using BattleshipEngine.Players;

namespace BattleshipEngine
{
    public class BattleshipSession
    {
        private readonly Dictionary<string, AIPlayer> _aiPlayers;
        private readonly Dictionary<string, HumanPlayer> _humanPlayers;

        public IEnumerable<string> Players =>
            _aiPlayers.Select(kvp => kvp.Key).Concat(_humanPlayers.Select(kvp => kvp.Key));

        private readonly Dictionary<string, Map> _maps;
        private readonly Rules _rules;
        private string[] _playerOrder;

        public int CurrentTurn { get; private set; }

        private int _currentPlayerIndex;

        public Player CurrentPlayer { private set; get; }
        private Player _nextPlayer;

        /// <summary>What is the next action required.</summary>
        public SessionState SessionState { private set; get; }

        #region Event variables

        public delegate void PlayerTurnResultDelegate(PlayerTurnResult playerTurnResult);

        /// <summary>
        /// Event invoked after an executed player turn.
        /// </summary>
        public event PlayerTurnResultDelegate OnPlayerTurnResultExecuted;

        public delegate void TurnDelegate(int turn);

        /// <summary>
        /// Event invoked every time turn changes.
        /// </summary>
        public event TurnDelegate OnTurnChanged;

        public delegate void PlayerDelegate(Player player);

        /// <summary>
        /// Event invoked when the session has been completed with a winner.
        /// </summary>
        public event PlayerDelegate OnSessionCompleted;

        #endregion

        private Queue<PlayerAction> _playersActions;

        /// <summary>
        /// Initialises a session with rules.
        /// </summary>
        /// <param name="rules">Rules for map creation.</param>
        /// <exception cref="Exception">Thrown if ships are missing from rules.</exception>
        public BattleshipSession(Rules rules)
        {
            if (rules.ShipsInMap.Count == 0)
                throw new Exception("No ships added in rules");

            _rules = rules;
            SessionState = SessionState.WaitingForPlayers;
            _aiPlayers = new Dictionary<string, AIPlayer>();
            _humanPlayers = new Dictionary<string, HumanPlayer>();
            _maps = new Dictionary<string, Map>();
            _playerOrder = Array.Empty<string>();
        }

        private void UpdateCurrentPlayer()
        {
            CurrentPlayer = _aiPlayers.ContainsKey(_playerOrder[_currentPlayerIndex])
                ? _aiPlayers[_playerOrder[_currentPlayerIndex]]
                : _humanPlayers[_playerOrder[_currentPlayerIndex]];
        }

        /// <summary>
        /// Will set the players for the session. Order matters in player turn.
        /// </summary>
        public void SetPlayers(AIPlayer[] aiPlayers, HumanPlayer[] humanPlayers,
            string[] playerOrder)
        {
            int amountOfPlayers = aiPlayers.Length + humanPlayers.Length;

            if (amountOfPlayers < 2)
                throw new Exception("Not enough players");

            _humanPlayers.Clear();
            _aiPlayers.Clear();

            foreach (var aiPlayer in aiPlayers)
            {
                _aiPlayers.Add(aiPlayer.Name, new AIPlayer(aiPlayer));
            }

            foreach (var humanPlayer in humanPlayers)
            {
                _humanPlayers.Add(humanPlayer.Name, new HumanPlayer(humanPlayer));
            }

            _playerOrder = playerOrder;

            UpdateCurrentPlayer();
            UpdateNextPlayer();

            SessionState = SessionState.WaitingForMaps;
        }

        /// <summary>
        /// Creates player and ai maps.
        /// </summary>
        /// <param name="humanPlayersMaps">Dictionary with the name of the player and its map.</param>
        /// <exception cref="MissingPlayerInformationException">Thrown if any player map is missing.</exception>
        /// <exception cref="Exception">Thrown if ships are missing from maps.</exception>
        public void SetMaps(Dictionary<string, ICreateMap> humanPlayersMaps)
        {
            _maps.Clear();

            // set up human maps
            foreach (var humanPlayer in _humanPlayers.Values)
            {
                if (!humanPlayersMaps.ContainsKey(humanPlayer.Name))
                {
                    throw new MissingPlayerInformationException(humanPlayer.Name,
                        $"Missing player's {humanPlayer.Name} map");
                }

                var map = (Map)humanPlayersMaps[humanPlayer.Name];

                foreach (var ship in _rules.ShipsInMap)
                {
                    if (!map.IsShipPositioned(ship, out _))
                        throw new Exception($"Ship {ship} not positioned. Position all your ships on the map first.");
                }

                _maps.Add(humanPlayer.Name, map.Clone());
            }

            // set up ai maps
            foreach (var aiPlayer in _aiPlayers.Values)
            {
                var map = new Map(_rules);
                aiPlayer.Strategy.PositionShips(map);

                _maps.Add(aiPlayer.Name, map);
            }

            SessionState = SessionState.Ready;
        }

        public ITargetMap GetTargetMap(string playerName)
        {
            if (_maps.TryGetValue(playerName, out var map))
                return map;

            throw new Exception($"Player {playerName} isn't included in maps");
        }

        /// <summary>
        /// Initialises the state of the session.
        /// After that make sure you listen to events for player turn result and session completed.
        /// </summary>
        /// <exception cref="Exception">Throws an exception if maps aren't set.</exception>
        public void Start()
        {
            if (_maps == null)
                throw new Exception("Maps weren't set");

            _playersActions = new Queue<PlayerAction>();

            CurrentTurn = 0;

            SessionState = SessionState.WaitingForPlayerTurn;
            OnTurnChanged.Invoke(CurrentTurn);
        }

        private void UpdateNextPlayer()
        {
            int nextPlayerIndex = _currentPlayerIndex + 1;
            if (nextPlayerIndex == _playerOrder.Length)
                nextPlayerIndex = 0;

            _nextPlayer = _aiPlayers.ContainsKey(_playerOrder[nextPlayerIndex])
                ? _aiPlayers[_playerOrder[nextPlayerIndex]]
                : _humanPlayers[_playerOrder[nextPlayerIndex]];
        }

        private void AdvanceCurrentPlayer()
        {
            _currentPlayerIndex++;
            bool updateTurn = false;
            if (_currentPlayerIndex == _playerOrder.Length)
            {
                _currentPlayerIndex = 0;
                updateTurn = true;
            }

            UpdateCurrentPlayer();
            UpdateNextPlayer();

            if (updateTurn)
            {
                CurrentTurn++;
                OnTurnChanged.Invoke(CurrentTurn);
            }
        }

        /// <summary>
        /// Plays an AI player action based on its strategy.
        /// </summary>
        /// <returns>If the action is valid.</returns>
        public bool PlayAITurn()
        {
            if (SessionState != SessionState.WaitingForPlayerTurn)
                return false;

            if (!CurrentPlayer.IsAI)
                return false;

            var enemyMap = _maps[_nextPlayer.Name];

            var mapCoordinates = _aiPlayers[CurrentPlayer.Name].Strategy.GetFireCoordinates(enemyMap);

            var playerAction = new PlayerAction(CurrentPlayer, _nextPlayer, mapCoordinates);
            ExecutePlayerAction(playerAction, enemyMap);

            return true;
        }

        /// <summary>
        /// Players a human player turn according to the parameters of enemy and fire coordinates.
        /// If the action is not valid the turn won't be executed and will wait until a valid action.
        /// </summary>
        /// <param name="enemyPlayer">Which is the enemy player</param>
        /// <param name="fireCoords">Where to fire on enemy's map</param>
        /// <returns>If the action is valid.</returns>
        public bool PlayHumanTurn(Player enemyPlayer, MapCoordinates fireCoords)
        {
            if (SessionState != SessionState.WaitingForPlayerTurn)
                return false;

            if (CurrentPlayer.IsAI)
                return false;

            var enemyMap = _maps[enemyPlayer.Name];

            if (!enemyMap.AreCoordinatesValid(fireCoords))
                return false;

            if (enemyMap.AreCoordinatesFiredAt(fireCoords))
                return false;

            var playerAction = new PlayerAction(CurrentPlayer, enemyPlayer, fireCoords);
            ExecutePlayerAction(playerAction, enemyMap);

            return true;
        }

        private void ExecutePlayerAction(PlayerAction playerAction, Map enemyMap)
        {
            bool hit = enemyMap.FireToCoordinates(playerAction.FireCoordinates, out var shipHitInfo);
            _playersActions.Enqueue(playerAction);

            var playerTurnResult = new PlayerTurnResult(playerAction, hit, shipHitInfo);
            OnPlayerTurnResultExecuted?.Invoke(playerTurnResult);

            if (hit && shipHitInfo.IsShipWrecked && enemyMap.ShipsRemaining == 0)
                CheckGameEnd();

            if (SessionState == SessionState.WaitingForPlayerTurn)
                AdvanceCurrentPlayer();
        }

        private Player GetPlayer(string name)
        {
            if (_humanPlayers.TryGetValue(name, out HumanPlayer humanPlayer))
                return humanPlayer;

            if (_aiPlayers.TryGetValue(name, out AIPlayer aiPlayer))
                return aiPlayer;

            return null;
        }

        /// <summary>
        /// Checks how many players have at least 1 remaining ship.
        /// </summary>
        private void CheckGameEnd()
        {
            int remainingPlayers = 0;
            string lastPlayer = string.Empty;
            foreach (var kvpMap in _maps)
            {
                if (kvpMap.Value.ShipsRemaining > 0)
                {
                    remainingPlayers++;
                    lastPlayer = kvpMap.Key;
                }
            }

            if (remainingPlayers < 2)
            {
                SessionState = SessionState.Ended;

                Player winner = GetPlayer(lastPlayer);
                OnSessionCompleted?.Invoke(winner);
            }
        }
    }

    public enum SessionState
    {
        None,
        WaitingForPlayers,
        WaitingForMaps,
        Ready,
        WaitingForPlayerTurn,
        Ended
    }
}