namespace BattleshipEngine.Interfaces;

public interface ITargetMap
{
    bool IsCoordinatesFiredAt(MapCoordinates fireCoordinates);
    
    bool FireToCoordinates(MapCoordinates fireCoordinates, out ShipHitInfo shipHitInfo);
}