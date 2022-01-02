namespace BattleshipEngine.BattleshipEngineExceptions;

public class ShipNotFoundException : Exception
{
    public Ship Ship { private set; get; }
    
    public ShipNotFoundException(Ship newShip) :base($"Ship {newShip.Name} not found in rules")
    {
        Ship = newShip;
    }
}