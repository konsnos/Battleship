using BattleshipEngine.Players;

namespace BattleshipEngine
{
    /// <summary>
    /// Defines a player turn. Contains the player action.
    /// If it was a successful hit and the ship that was hit if it exists.
    /// </summary>
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