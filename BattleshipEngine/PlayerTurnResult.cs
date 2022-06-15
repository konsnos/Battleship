using BattleshipEngine.Players;

namespace BattleshipEngine;

public struct PlayerTurnResult
{
    public PlayerAction PlayerAction { private set; get; }
    public bool Hit { private set; get; }
    public ShipHitInfo ShipHitInfo { private set; get; }

    public PlayerTurnResult(PlayerAction playerAction, bool hit, ShipHitInfo shipHitInfo)
    {
        PlayerAction = playerAction;
        Hit = hit;
        ShipHitInfo = shipHitInfo;
    }

    public PlayerTurnResult()
    {
        PlayerAction = new PlayerAction();
        Hit = false;
        ShipHitInfo = new ShipHitInfo();
    }
}