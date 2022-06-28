using BattleshipEngine;
using BattleshipEngine.Maps;
using NUnit.Framework;

namespace BattleshipTest;

public class MapTests
{
    private Rules _rules;
    private Map _map;

    [SetUp]
    public void SetUp()
    {
        _rules = Rules.Default();
        _map = new Map(_rules);
    }

    [Test]
    public void TestGetCoordinates()
    {
        MapCoordinates mapCoordinates;
        mapCoordinates = _map.GetRandomPositionForShip(true, 9);

        Assert.AreEqual(0, mapCoordinates.Column);

        mapCoordinates = _map.GetRandomPositionForShip(false, 9);

        Assert.AreEqual(0, mapCoordinates.Row);
    }
}