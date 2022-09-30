using BattleshipEngine.Maps;

namespace BattleshipEngine.Players.AIs
{
    public abstract class BaseAI
    {
        protected static readonly Random _random = new Random();
        private BattleshipSession _battleshipSession;
        protected Player _self { private set; get; }

        protected BaseAI(BattleshipSession battleshipSession)
        {
            _battleshipSession = battleshipSession;

            SubscribeToEvents();
        }

        ~BaseAI()
        {
            UnsubscribeFromEvents();
        }

        public void AssignPlayer(Player player)
        {
            _self = player;
        }

        private void SubscribeToEvents()
        {
            _battleshipSession.PlayerTurnResultExecuted += PlayerTurnResultExecuted;
        }

        private void UnsubscribeFromEvents()
        {
            _battleshipSession.PlayerTurnResultExecuted -= PlayerTurnResultExecuted;
        }

        protected virtual void PlayerTurnResultExecuted(PlayerTurnResult playerTurnResult)
        {
            // do nothing
        }

        public abstract void PositionShips(ICreateMap ownMap);

        public abstract MapCoordinates GetFireCoordinates(ITargetMap enemyMap);
        
        protected static MapCoordinates GetRandomFireAtCoordinates(ITargetMap enemyMap)
        {
            var mapCoordinates = new MapCoordinates();
            do
            {
                mapCoordinates.ChangeCoordinates(_random.Next(enemyMap.Columns), _random.Next(enemyMap.Rows));
            } while (enemyMap.AreCoordinatesFiredAt(mapCoordinates));

            return mapCoordinates;
        }
    }
}