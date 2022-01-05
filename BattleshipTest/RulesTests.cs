using BattleshipEngine;
using NUnit.Framework;

namespace BattleshipTest;

public class RulesTests
{
    private Rules rules;

    [SetUp]
    public void SetUp()
    {
        rules = new Rules();
    }

    [Test]
    public void TestGetCoordinates()
    {
        MapCoordinates mapCoordinates;
        mapCoordinates = rules.GetRandomPositionForShip(true, 9);

        Assert.AreEqual(0, mapCoordinates.column);

        mapCoordinates = rules.GetRandomPositionForShip(false, 9);

        Assert.AreEqual(0, mapCoordinates.row);
    }
}