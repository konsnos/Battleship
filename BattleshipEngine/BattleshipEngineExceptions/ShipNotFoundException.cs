namespace BattleshipEngine.BattleshipEngineExceptions;

public class ShipNotFoundException : Exception
{
    public string ShipName { get; private set; }
    public int Index { get; private set; }
    
    public ShipNotFoundException(string newShipName, int newIndex) :base($"Ship {newShipName} number {newIndex} not found in rules")
    {
        ShipName = newShipName;
        Index = newIndex;
    }
}