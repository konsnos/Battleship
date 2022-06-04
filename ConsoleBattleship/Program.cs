using System.Text;
using BattleshipEngine;
using BattleshipEngine.Interfaces;

Console.WriteLine("Generating ships map");

var rules = new Rules();

var map = new Map(rules);

Console.WriteLine("Generated ships map");

AddShips(map);

var mapToPrint = map.GetMapForPrint();
PrintMap(mapToPrint);

TestFire(map);

// End
Console.WriteLine("Finished");

void AddShips(IShipsMap shipsMap)
{
    Console.WriteLine("Adding ships...");

    foreach (var shipInMap in shipsMap.ShipsInMap)
    {
        shipsMap.PositionShipInRandomCoordinatesUnsafe(shipInMap);
    }

    Console.WriteLine("Added ships");
    Console.WriteLine();
}

void TestFire(ITargetMap fireMap)
{
    StringBuilder sb = new StringBuilder();
    sb.AppendLine("Fire solution");
    Random r = new Random();
    int fireRounds = 50;
    for (int i = 0; i < fireRounds; i++)
    {
        MapCoordinates mapCoordinates = new MapCoordinates();
        bool isFiredAt = true;
        while (isFiredAt)
        {
            mapCoordinates = new MapCoordinates(r.Next(rules.ColumnsSize), r.Next(rules.RowsSize));
            isFiredAt = map.IsCoordinatesFiredAt(mapCoordinates);
        }

        bool hit = map.FireToCoordinates(mapCoordinates, out ShipHitInfo shipHitInfo);

        sb.Append($"Fired at {mapCoordinates} and hit {hit}.");
        if (hit)
        {
            sb.Append($" Hit info: {shipHitInfo.Ship.Name}:{shipHitInfo.Ship.Id}.");
            sb.Append($" Wrecked {shipHitInfo.IsShipWrecked}.");
        }
        sb.AppendLine();
    }
    
    Console.WriteLine(sb);
}

void PrintMap(char[,] mapChars)
{
    Console.WriteLine("Map");
    for (int r = 0; r < mapChars.GetLength(0); r++)
    {
        var sb = new StringBuilder();

        for (int c = 0; c < mapChars.GetLength(1); c++)
        {
            sb.Append(mapChars[r, c]);
        }

        Console.WriteLine(sb);
    }

    Console.WriteLine();
}