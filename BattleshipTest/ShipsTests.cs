using BattleshipEngine;
using BattleshipEngine.BattleshipEngineExceptions;
using BattleshipEngine.Interfaces;
using NUnit.Framework;

namespace BattleshipTest;

public class ShipsTests
{
    private IShipsMap shipsMap;

    [SetUp]
    public void Setup()
    {
        var rules = new Rules();

        shipsMap = new Map(rules);
    }

    [Test]
    public void TestAddSuccess()
    {
        shipsMap.PositionShip("Carrier", 0, new MapCoordinates(0, 0), true);

        Assert.Pass();
    }

    [Test]
    public void TestNotExistingShip()
    {
        Assert.Throws<ShipNotFoundException>(() =>
        {
            shipsMap.PositionShip("Frigate", 0, new MapCoordinates(), true);
        });
    }

    [Test]
    public void TestAddOutOfMap()
    {
        Assert.Throws<OutOfMapException>(() => { shipsMap.PositionShip("Carrier", 0, new MapCoordinates(6, 0), true); });
        
        Assert.Throws<OutOfMapException>(() => { shipsMap.PositionShip("Carrier", 0, new MapCoordinates(0, 6), false); });
        
        Assert.Throws<OutOfMapException>(() => { shipsMap.PositionShip("Carrier", 0, new MapCoordinates(9, 7), true); });
    }

    [Test]
    public void TestShipOverlapping()
    {
        shipsMap.PositionShip("Carrier", 0, new MapCoordinates(), true);
        // Check reposition
        shipsMap.PositionShip("Carrier", 0, new MapCoordinates(1, 0), true);
        Assert.Pass();
        
        Assert.Throws<OccupiedTileException>(() =>
        {
            shipsMap.PositionShip("Battleship", 0, new MapCoordinates(2, 0), true);
        });
    }
}