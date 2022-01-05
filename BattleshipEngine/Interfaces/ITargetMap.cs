namespace BattleshipEngine.Interfaces;

public interface ITargetMap
{
    public IEnumerable<Ship> ShipsWrecked { get; }
    public int ShipsRemaining { get; }

    bool IsCoordinatesFiredAt(MapCoordinates fireCoordinates);
    
    bool FireToCoordinates(MapCoordinates fireCoordinates, out ShipHitInfo shipHitInfo);
}