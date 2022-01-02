using BattleshipEngine;
using BattleshipEngine.BattleshipEngineExceptions;
using BattleshipEngine.Interfaces;
using NUnit.Framework;

namespace BattleshipTest;

public class ShipsTests
{
    private Rules rules;
    private IShipsMap shipsMap;

    [SetUp]
    public void Setup()
    {
        rules = new Rules();

        shipsMap = new Map(rules);
    }

    [Test]
    public void TestAddSuccess()
    {
        var ship = rules.shipsInMap[0];
        shipsMap.PositionShip(ship, new MapCoordinates(0, 0), true);

        Assert.Pass();
    }

    [Test]
    public void TestNotExistingShip()
    {
        var newShip = new Ship("Frigate", 3, 0);
        Assert.Throws<ShipNotFoundException>(() =>
        {
            shipsMap.PositionShip(newShip, new MapCoordinates(), true);
        });
    }

    [Test]
    public void TestAddOutOfMap()
    {
        var ship = rules.shipsInMap[0];
        Assert.Throws<OutOfMapException>(() => { shipsMap.PositionShip(ship, new MapCoordinates(6, 0), true); });
        
        Assert.Throws<OutOfMapException>(() => { shipsMap.PositionShip(ship, new MapCoordinates(0, 6), false); });
        
        Assert.Throws<OutOfMapException>(() => { shipsMap.PositionShip(ship, new MapCoordinates(9, 7), true); });
    }

    [Test]
    public void TestShipOverlapping()
    {
        var carrier = rules.shipsInMap[0];
        shipsMap.PositionShip(carrier, new MapCoordinates(), true);
        // Check reposition
        shipsMap.PositionShip(carrier, new MapCoordinates(1, 0), true);
        Assert.Pass();

        var battleship = rules.shipsInMap[1];
        Assert.Throws<OccupiedTileException>(() =>
        {
            shipsMap.PositionShip(battleship, new MapCoordinates(2, 0), true);
        });
    }
}