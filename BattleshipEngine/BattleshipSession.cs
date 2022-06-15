using BattleshipEngine.BattleshipEngineExceptions;
using BattleshipEngine.Interfaces;
using BattleshipEngine.Maps;
using BattleshipEngine.Players;

namespace BattleshipEngine;

public class BattleshipSession
{
    private Dictionary<string, AIPlayer> _aiPlayers;
    private Dictionary<string, HumanPlayer> _humanPlayers;

    public IEnumerable<string> Players =>
        _aiPlayers.Select(kvp => kvp.Key).Concat(_humanPlayers.Select(kvp => kvp.Key));

    private Dictionary<string, Map> _maps;
    private Rules _rules;
    private string[] _playerOrder;

    private int _currentTurn;
    public int CurrentTurn => _currentTurn;
    private int _currentPlayerIndex;

    public Player CurrentPlayer { private set; get; }
    private Player _nextPlayer;

    public SessionState SessionState { private set; get; }

    public delegate void PlayerTurnResultDelegate(PlayerTurnResult playerTurnResult);

    public event PlayerTurnResultDelegate OnPlayerTurnResultExecuted;

    public delegate void TurnDelegate(int turn);

    public event TurnDelegate OnTurnChanged;

    public delegate void PlayerDelegate(Player player);

    public event PlayerDelegate OnSessionCompleted;

    private Queue<PlayerAction> _playerActions;

    public BattleshipSession(Rules rules)
    {
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

    public void SetMaps(Dictionary<string, Map> humanPlayersMaps)
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

            Map map = humanPlayersMaps[humanPlayer.Name];

            foreach (var ship in _rules.ShipsInMap)
            {
                if (!map.IsShipPositioned(ship, out _))
                {
                    throw new Exception($"Ship {ship} not positioned. Position all your ships on the map first.");
                }
            }

            _maps.Add(humanPlayer.Name, humanPlayersMaps[humanPlayer.Name].Clone());
        }

        // set up ai maps
        foreach (var aiPlayer in _aiPlayers.Values)
        {
            Map map = new Map(_rules);
            aiPlayer.Strategy.PositionShips(map);

            _maps.Add(aiPlayer.Name, map);
        }

        SessionState = SessionState.Ready;
    }

    public ITargetMap GetTargetMap(string playerName)
    {
        if (_maps.TryGetValue(playerName, out Map map))
        {
            return map;
        }

        throw new Exception($"Player {playerName} isn't included in maps");
    }

    public void Start()
    {
        if (_maps == null)
        {
            throw new Exception("Set up was not run");
        }

        _playerActions = new Queue<PlayerAction>();

        _currentTurn = 0;

        SessionState = SessionState.WaitingForPlayerTurn;
        OnTurnChanged.Invoke(_currentTurn);
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

        if(updateTurn)
        {
            _currentTurn++;
            OnTurnChanged.Invoke(_currentTurn);
        }
    }

    public bool PlayAITurn()
    {
        if (SessionState != SessionState.WaitingForPlayerTurn)
        {
            return false;
        }
        
        if (!CurrentPlayer.IsAI)
        {
            return false;
        }

        Map enemyMap = _maps[_nextPlayer.Name];

        var mapCoordinates = _aiPlayers[CurrentPlayer.Name].Strategy.GetFireCoordinates(enemyMap);

        PlayerAction playerAction = new PlayerAction(CurrentPlayer, _nextPlayer, mapCoordinates);
        bool hit = enemyMap.FireToCoordinates(mapCoordinates, out ShipHitInfo shipHitInfo);
        _playerActions.Enqueue(playerAction);

        var playerTurnResult = new PlayerTurnResult(playerAction, hit, shipHitInfo);
        OnPlayerTurnResultExecuted?.Invoke(playerTurnResult);

        // check if enemy has lost
        if (hit && shipHitInfo.IsShipWrecked)
        {
            if (enemyMap.ShipsRemaining == 0)
            {
                // player lost
                CheckGameEnd();
            }
        }
        
        if(SessionState == SessionState.WaitingForPlayerTurn)
            AdvanceCurrentPlayer();

        return true;
    }

    public bool PlayHumanTurn(Player enemyPlayer, MapCoordinates fireCoords)
    {
        if (SessionState != SessionState.WaitingForPlayerTurn)
            return false;
        
        if (CurrentPlayer.IsAI)
            return false;

        if (fireCoords.Column >= _rules.ColumnsSize || fireCoords.Row >= _rules.RowsSize)
            return false;

        Map enemyMap = _maps[enemyPlayer.Name];

        if (enemyMap.AreCoordinatesFiredAt(fireCoords))
            return false;

        PlayerAction playerAction = new PlayerAction(CurrentPlayer, enemyPlayer, fireCoords);
        bool hit = enemyMap.FireToCoordinates(fireCoords, out ShipHitInfo shipHitInfo);
        _playerActions.Enqueue(playerAction);

        var playerTurnResult = new PlayerTurnResult(playerAction, hit, shipHitInfo);
        OnPlayerTurnResultExecuted?.Invoke(playerTurnResult);

        if (hit && shipHitInfo.IsShipWrecked)
        {
            if (enemyMap.ShipsRemaining == 0)
            {
                // player lost
                CheckGameEnd();
            }
        }

        if(SessionState == SessionState.WaitingForPlayerTurn)
            AdvanceCurrentPlayer();

        return true;
    }

    private Player GetPlayer(string name)
    {
        if (_humanPlayers.TryGetValue(name, out HumanPlayer humanPlayer))
            return humanPlayer;

        if (_aiPlayers.TryGetValue(name, out AIPlayer aiPlayer))
            return aiPlayer;

        return null;
    }

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