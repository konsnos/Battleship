using System.Collections.Generic;
using BattleshipEngine;
using BattleshipEngine.Interfaces;
using NUnit.Framework;

namespace BattleshipTest;

public class FireTests
{
    private Rules rules;
    private ITargetMap targetMap;
    private List<ShipLocation> shipLocations;

    [SetUp]
    public void Setup()
    {
        rules = new Rules();
        shipLocations = new List<ShipLocation>(rules.shipsInMap.Count);

        Map map = new Map(rules);

        AddShips(map);

        targetMap = map;
    }

    private void AddShips(IShipsMap shipsMap)
    {
        foreach (var shipInMap in shipsMap.ShipsInMap)
        {
            shipLocations.Add(shipsMap.PositionShipInRandomCoordinatesUnsafe(shipInMap));
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

    [Test]
    public void TestFireToEmpty()
    {
        HashSet<MapCoordinates> occupiedCoordinates = new HashSet<MapCoordinates>();

        foreach (var shipLocation in shipLocations)
        {
            var shipMapCoordinates = shipLocation.GetCoordinatesFromShipLocation();
            foreach (var shipMapCoordinate in shipMapCoordinates)
            {
                occupiedCoordinates.Add(shipMapCoordinate);
            }
        }

        MapCoordinates freeMapCoordinate = new MapCoordinates();
        for (int r = 0; r < rules.RowsSize; r++)
        {
            for (int c = 0; c < rules.ColumnsSize; c++)
            {
                freeMapCoordinate = new MapCoordinates(c, r);
                if (occupiedCoordinates.Contains(freeMapCoordinate)) continue;
                
                var isHit = targetMap.FireToCoordinates(freeMapCoordinate, out var shipHitInfo);
                Assert.IsFalse(isHit);
            }
        }
    }
    
    //todo: test ship wreck
}