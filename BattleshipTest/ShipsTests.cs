using BattleshipEngine;
using BattleshipEngine.BattleshipEngineExceptions;
using BattleshipEngine.Maps;
using NUnit.Framework;

namespace BattleshipTest;

public class ShipsTests
{
    private Rules _rules;
    private Map _shipsMap;

    [SetUp]
    public void Setup()
    {
        _rules = new Rules();

        _shipsMap = new Map(_rules);
    }

    [Test]
    public void TestAddSuccess()
    {
        var ship = _rules.ShipsInMap[0];
        var shipLocation = new ShipLocation(ship, new MapCoordinates(), true);
        _shipsMap.PositionShip(shipLocation);

        Assert.Pass();

        var shipExists = _shipsMap.IsShipPositioned(ship, out var existingShipLocation);

        Assert.IsTrue(shipExists);
    }

    [Test]
    public void TestNotExistingShip()
    {
        var ship = new Ship("Frigate", 3, 0);
        var shipLocation = new ShipLocation(ship, new MapCoordinates(), true);
        Assert.Throws<ShipNotFoundException>(() => { _shipsMap.PositionShip(shipLocation); });
    }

    [Test]
    public void TestAddOutOfMap()
    {
        var ship = _rules.ShipsInMap[0];
        Assert.Throws<OutOfMapException>(() =>
        {
            _shipsMap.PositionShip(new ShipLocation(ship, new MapCoordinates(6, 0), true));
        });

        Assert.Throws<OutOfMapException>(() =>
        {
            _shipsMap.PositionShip(new ShipLocation(ship, new MapCoordinates(0, 6), false));
        });

        Assert.Throws<OutOfMapException>(() =>
        {
            _shipsMap.PositionShip(new ShipLocation(ship, new MapCoordinates(9, 7), true));
        });
    }

    [Test]
    public void TestShipOverlapping()
    {
        var carrier = _rules.ShipsInMap[0];
        var carrierLocation = new ShipLocation(carrier, new MapCoordinates(), true);
        _shipsMap.PositionShip(carrierLocation);
        // Check reposition
        carrierLocation.ChangeLocation(new MapCoordinates(1, 0), true);
        _shipsMap.PositionShip(carrierLocation);
        Assert.Pass();

        var battleship = _rules.ShipsInMap[1];
        var battleshipLocation = new ShipLocation(battleship, new MapCoordinates(2, 0), true);
        Assert.Throws<OccupiedTileException>(() =>
        {
            _shipsMap.PositionShip(battleshipLocation);
        });
    }

    [Test]
    public void TestShipReferenceIntegrity()
    {
        var firstShip = _rules.ShipsInMap[0];
        var shipLocation = new ShipLocation(firstShip, new MapCoordinates(), true);
        _shipsMap.PositionShip(shipLocation);
        shipLocation.ChangeLocation(new MapCoordinates(1, 1), false);

        bool shipExists = _shipsMap.IsShipPositioned(firstShip, out var existingShipLocation);

        Assert.IsTrue(shipExists);

        Assert.AreEqual(new MapCoordinates(), existingShipLocation.StartingTile);

        existingShipLocation.ChangeLocation(new MapCoordinates(1, 1), false);
        _shipsMap.IsShipPositioned(firstShip, out var existingShipLocation2);
        
        Assert.AreEqual(new MapCoordinates(), existingShipLocation2.StartingTile);
    }

    [Test]
    public void TestOccupiedCoordinates()
    {
        var firstShip = _rules.ShipsInMap[0];
        var shipLocation = new ShipLocation(firstShip, new MapCoordinates(), true);
        _shipsMap.PositionShip(shipLocation);

        foreach (var occupiedCoordinate in shipLocation.OccupiedCoordinates)
        {
            bool areFree = _shipsMap.AreCoordinatesFree(shipLocation.OccupiedCoordinates);
            Assert.AreEqual(false, areFree);
        }
    }
}