using BattleshipEngine.Players;

namespace BattleshipEngine
{
    public struct PlayerTurnResult
    {
        public PlayerAction PlayerAction { get; }
        public bool Hit { get; }
        public ShipHitInfo ShipHitInfo { get; }

        public PlayerTurnResult(PlayerAction playerAction, bool hit, ShipHitInfo shipHitInfo)
        {
            PlayerAction = playerAction;
            Hit = hit;
            ShipHitInfo = shipHitInfo;
        }
    }
}