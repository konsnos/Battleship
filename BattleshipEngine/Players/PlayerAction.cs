using BattleshipEngine.Maps;

namespace BattleshipEngine.Players;

public struct PlayerAction
{
    public Player Player { private set; get; }
    
    public Player EnemyPlayer { private set; get; }
    public MapCoordinates FireCoordinates { private set; get; }

    public PlayerAction(Player player, Player enemyPlayer, MapCoordinates fireCoordinates)
    {
        Player = player;
        EnemyPlayer = enemyPlayer;
        FireCoordinates = fireCoordinates;
    }
}