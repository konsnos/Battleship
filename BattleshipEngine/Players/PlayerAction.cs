using BattleshipEngine.Maps;

namespace BattleshipEngine.Players
{
    /// <summary>
    /// PlayerAction defines an action by which Player invoked it, which player was the target and what were the fire coordinates.
    /// </summary>
    public readonly struct PlayerAction
    {
        public Player Player { get; }

        public Player EnemyPlayer { get; }
        public MapCoordinates FireCoordinates { get; }

        public PlayerAction(Player player, Player enemyPlayer, MapCoordinates fireCoordinates)
        {
            Player = player;
            EnemyPlayer = enemyPlayer;
            FireCoordinates = fireCoordinates;
        }
    }
}