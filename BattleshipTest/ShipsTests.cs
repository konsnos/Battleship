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
        var shipLocation = new ShipLocation(ship, new MapCoordinates(), true);
        shipsMap.PositionShip(shipLocation);

        Assert.Pass();

        var shipExists = shipsMap.IsShipPositioned(ship, out var existingShipLocation);

        Assert.IsTrue(shipExists);
    }

    [Test]
    public void TestNotExistingShip()
    {
        var newShip = new Ship("Frigate", 3, 0);
        var shipLocation = new ShipLocation(newShip, new MapCoordinates(), true);
        Assert.Throws<ShipNotFoundException>(() => { shipsMap.PositionShip(shipLocation); });
    }

    [Test]
    public void TestAddOutOfMap()
    {
        var ship = rules.shipsInMap[0];
        Assert.Throws<OutOfMapException>(() =>
        {
            shipsMap.PositionShip(new ShipLocation(ship, new MapCoordinates(6, 0), true));
        });

        Assert.Throws<OutOfMapException>(() =>
        {
            shipsMap.PositionShip(new ShipLocation(ship, new MapCoordinates(0, 6), false));
        });

        Assert.Throws<OutOfMapException>(() =>
        {
            shipsMap.PositionShip(new ShipLocation(ship, new MapCoordinates(9, 7), true));
        });
    }

    [Test]
    public void TestShipOverlapping()
    {
        var carrier = rules.shipsInMap[0];
        var carrierLocation = new ShipLocation(carrier, new MapCoordinates(), true);
        shipsMap.PositionShip(carrierLocation);
        // Check reposition
        carrierLocation.ChangeLocation(new MapCoordinates(1, 0), true);
        shipsMap.PositionShip(carrierLocation);
        Assert.Pass();

        var battleship = rules.shipsInMap[1];
        var battleshipLocation = new ShipLocation(battleship, new MapCoordinates(2, 0), true);
        Assert.Throws<OccupiedTileException>(() =>
        {
            shipsMap.PositionShip(battleshipLocation);
        });
    }

    [Test]
    public void TestShipReferenceIntegrity()
    {
        var firstShip = rules.shipsInMap[0];
        var shipLocation = new ShipLocation(firstShip, new MapCoordinates(), true);
        shipsMap.PositionShip(shipLocation);
        shipLocation.ChangeLocation(new MapCoordinates(1, 1), false);

        bool shipExists = shipsMap.IsShipPositioned(firstShip, out var existingShipLocation);

        Assert.IsTrue(shipExists);

        Assert.AreEqual(new MapCoordinates(), existingShipLocation.StartingTile);

        existingShipLocation.ChangeLocation(new MapCoordinates(1, 1), false);
        shipsMap.IsShipPositioned(firstShip, out var existingShipLocation2);
        
        Assert.AreEqual(new MapCoordinates(), existingShipLocation2.StartingTile);
    }
}