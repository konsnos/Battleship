using System.Collections.Generic;
using System.Linq;
using BattleshipEngine;
using BattleshipEngine.Interfaces;
using BattleshipEngine.Maps;
using NUnit.Framework;

namespace BattleshipTest;

public class FireTests
{
    private Rules _rules;
    private ITargetMap _targetMap;
    private List<ShipLocation> _shipLocations;

    [SetUp]
    public void Setup()
    {
        _rules = new Rules();
        _shipLocations = new List<ShipLocation>(_rules.ShipsInMap.Count);

        Map map = new Map(_rules);

        AddShips(map, _rules);

        _targetMap = map;
    }

    private void AddShips(ICreateMap shipsMap, Rules rules)
    {
        foreach (var shipInMap in rules.ShipsInMap)
        {
            _shipLocations.Add(shipsMap.PositionShipInRandomCoordinatesUnsafe(shipInMap));
        }
    }

    [Test]
    public void TestFireSuccess()
    {
        var mapCoordinate = new MapCoordinates();
        var isHit = _targetMap.FireToCoordinates(mapCoordinate, out var shipHitInfo);

        var isFiredAt = _targetMap.AreCoordinatesFiredAt(mapCoordinate);

        Assert.IsTrue(isFiredAt);
    }

    [Test]
    public void TestFireToEmpty()
    {
        HashSet<MapCoordinates> occupiedCoordinates = new HashSet<MapCoordinates>();

        foreach (var shipLocation in _shipLocations)
        {
            var shipMapCoordinates = shipLocation.OccupiedCoordinates;
            foreach (var shipMapCoordinate in shipMapCoordinates)
            {
                occupiedCoordinates.Add(shipMapCoordinate);
            }
        }

        MapCoordinates freeMapCoordinate = new MapCoordinates();
        for (int r = 0; r < _rules.RowsSize; r++)
        {
            for (int c = 0; c < _rules.ColumnsSize; c++)
            {
                freeMapCoordinate = new MapCoordinates(c, r);
                if (occupiedCoordinates.Contains(freeMapCoordinate)) continue;
                
                var isHit = _targetMap.FireToCoordinates(freeMapCoordinate, out var shipHitInfo);
                Assert.IsFalse(isHit);
            }
        }
    }

    [Test]
    public void TestFireToShips()
    {
        int shipsWrecked = 0;
        foreach (var shipLocation in _shipLocations)
        {
            var shipCoordinates = shipLocation.OccupiedCoordinates;

            ShipHitInfo shipHitInfo = new ShipHitInfo();
            foreach (var shipCoordinate in shipCoordinates)
            {
                var isHit = _targetMap.FireToCoordinates(shipCoordinate, out shipHitInfo);

                Assert.IsTrue(isHit);
            }

            Assert.IsTrue(shipHitInfo.IsShipWrecked);
            shipsWrecked++;

            Assert.AreEqual(shipsWrecked, _targetMap.ShipsWrecked.Count());
            Assert.AreEqual(_shipLocations.Count - shipsWrecked, _targetMap.ShipsRemaining);
        }

        Assert.AreEqual(_shipLocations.Count, shipsWrecked);
        Assert.AreEqual(0, _targetMap.ShipsRemaining);
    }
}