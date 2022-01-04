using System;
using BattleshipEngine;
using BattleshipEngine.Interfaces;
using NUnit.Framework;

namespace BattleshipTest;

public class FireTests
{
    private Rules rules;
    private ITargetMap targetMap;

    [SetUp]
    public void Setup()
    {
        rules = new Rules();

        Map map = new Map(rules);

        AddShips(map);

        targetMap = map;
    }
    
    void AddShips(IShipsMap shipsMap)
    {
        foreach (var shipInMap in shipsMap.ShipsInMap)
        {
            shipsMap.PositionShipInRandomCoordinatesUnsafe(new ShipLocation(shipInMap));
        }
        
        
    }

    [Test]
    public void TestFireSuccess()
    {
        var mapCoordinate = new MapCoordinates();
        var isHit = targetMap.FireToCoordinates(mapCoordinate, out var shipHitInfo);

        var isFiredAt = targetMap.IsCoordinatesFiredAt(mapCoordinate);

        Assert.IsTrue(isFiredAt);
    }
    
    //todo: test successful shot
    //todo: test ship wreck
}