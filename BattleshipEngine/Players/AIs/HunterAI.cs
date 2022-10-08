using BattleshipEngine.Maps;

namespace BattleshipEngine.Players.AIs
{
    /// <summary>
    /// Based on https://www.datagenetics.com/blog/december32011/
    /// </summary>
    public class HunterAI : BaseAI
    {
        private ITargetMap _enemyMap;
        /// <summary>
        /// True when searching neighboring tiles.
        /// </summary>
        private bool huntMode;
        private MapCoordinates initialHitCoordinates;
        /// <summary>
        /// When false the priorities are all over the initial hit coordinates.
        /// When true the alignment of the target is found as horizontal or vertical.
        /// </summary>
        private bool isTargetAligned;

        private bool isTargetHorizontal;
        private List<MapCoordinates> priorities = new List<MapCoordinates>();
        
        public HunterAI(BattleshipSession battleshipSession) : base(battleshipSession)
        {
        }
        
        public override void PositionShips(ICreateMap ownMap)
        {
            foreach (var shipInMap in ownMap.ShipsInMap)
            {
                ownMap.PositionShipInRandomCoordinatesUnsafe(shipInMap);
            }
        }

        public override MapCoordinates GetFireCoordinates(ITargetMap enemyMap)
        {
            //todo: only works for 2 players
            _enemyMap = enemyMap;

            if (huntMode)
            {
                int coordinatesIndex = _random.Next(priorities.Count);
                return priorities[coordinatesIndex];
            }
            else
            {
                return GetRandomFireAtCoordinates(enemyMap);
            }
        }

        protected override void PlayerTurnResultExecuted(PlayerTurnResult playerTurnResult)
        {
            base.PlayerTurnResultExecuted(playerTurnResult);

            if (!playerTurnResult.PlayerAction.Player.Equals(_self)) return;

            var fireCoordinates = playerTurnResult.PlayerAction.FireCoordinates;

            if (playerTurnResult.Hit)
            {
                if(!huntMode)
                {
                    huntMode = true;
                    isTargetAligned = false;
                    initialHitCoordinates = fireCoordinates;
                    
                    AssignInitialPriorities();
                }
                else
                {
                    priorities.Remove(fireCoordinates);
                    
                    if (!isTargetAligned)
                    {
                        isTargetHorizontal = initialHitCoordinates.Column != fireCoordinates.Column;
                        isTargetAligned = true;
                        
                        foreach (var priority in priorities.ToArray())
                        {
                            if (isTargetHorizontal)
                            {
                                if (priority.Row != initialHitCoordinates.Row)
                                {
                                    priorities.Remove(priority);
                                }
                            }
                            else
                            {
                                if (priority.Column != initialHitCoordinates.Column)
                                {
                                    priorities.Remove(priority);
                                }
                            }
                        }
                    }
                     
                    // Update priorities
                    // todo: check current length and maximum available ships length
                    if (isTargetHorizontal)
                    {
                        var rightCoordinates = new MapCoordinates(fireCoordinates.Column + 1, fireCoordinates.Row);
                        if (CanBePriority(rightCoordinates))
                            priorities.Add(rightCoordinates);
                        var leftCoordinates = new MapCoordinates(fireCoordinates.Column - 1, fireCoordinates.Row);
                        if (CanBePriority(leftCoordinates))
                            priorities.Add(leftCoordinates);
                    }
                    else
                    {
                        var topCoordinates =
                            new MapCoordinates(fireCoordinates.Column, fireCoordinates.Row - 1);
                        if (CanBePriority(topCoordinates))
                            priorities.Add(topCoordinates);

                        var bottomCoordinates =
                            new MapCoordinates(fireCoordinates.Column, fireCoordinates.Row + 1);
                        if(CanBePriority(bottomCoordinates))
                            priorities.Add(bottomCoordinates);
                    }
                }
            }
            else
            {
                if (huntMode)
                {
                    priorities.Remove(fireCoordinates);
                }
            }

            if (huntMode)
            {
                if (priorities.Count == 0)
                {
                    huntMode = false;
                }
            }
        }

        private void AssignInitialPriorities()
        {
            var rightCoordinates =
                new MapCoordinates(initialHitCoordinates.Column + 1, initialHitCoordinates.Row);
            if (CanBePriority(rightCoordinates))
                priorities.Add(rightCoordinates);

            var leftCoordinates =
                new MapCoordinates(initialHitCoordinates.Column - 1, initialHitCoordinates.Row);
            if(CanBePriority(leftCoordinates))
                priorities.Add(leftCoordinates);

            var topCoordinates =
                new MapCoordinates(initialHitCoordinates.Column, initialHitCoordinates.Row - 1);
            if (CanBePriority(topCoordinates))
                priorities.Add(topCoordinates);

            var bottomCoordinates =
                new MapCoordinates(initialHitCoordinates.Column, initialHitCoordinates.Row + 1);
            if(CanBePriority(bottomCoordinates))
                priorities.Add(bottomCoordinates);
        }

        private bool CanBePriority(MapCoordinates mapCoordinates)
        {
            return _enemyMap.AreCoordinatesValid(mapCoordinates) &&
                   !_enemyMap.AreCoordinatesFiredAt(mapCoordinates);
        }
    }
}