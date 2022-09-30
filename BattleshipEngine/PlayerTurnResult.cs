using BattleshipEngine.Players;

namespace BattleshipEngine
{
    /// <summary>
    /// Defines a player turn. Contains the player action.
    /// If it was a successful hit and the ship that was hit if it exists.
    /// </summary>
    public readonly struct PlayerTurnResult
    {
        public PlayerAction PlayerAction { get; }
        public bool Hit { get; }

        public PlayerTurnResult(PlayerAction playerAction, bool hit)
        {
            PlayerAction = playerAction;
            Hit = hit;
        }
    }
}