﻿// See https://aka.ms/new-console-template for more information

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

// End
Console.WriteLine("Finished");

void AddShips(IShipsMap shipsMap)
{
    Console.WriteLine("Adding ships...");

    foreach (var keyValuePair in rules.shipsInMap)
    {
        for (int i = 0; i < keyValuePair.Value; i++)
        {
            var ship = keyValuePair.Key;

            bool isInsideMap = false;
            bool canBePositioned = false;
            ShipLocation shipLocation = new ShipLocation(ship, new MapCoordinates(), true);
            while (!isInsideMap || !canBePositioned)
            {
                GetRandomPosition(ship, ref shipLocation);
                var shipCoordinates = shipLocation.GetCoordinatesFromShipLocation();
                isInsideMap = shipsMap.IsCoordinatesInsideMap(shipCoordinates);
                canBePositioned = shipsMap.IsCoordinatesFree(shipCoordinates);
            }

            shipsMap.PositionShip(ship.Name, i, shipLocation.StartingTile, shipLocation.IsHorizontal);
        }
    }

    Console.WriteLine("Added ships");
    Console.WriteLine();
}

void GetRandomPosition(Ship ship, ref ShipLocation shipLocation)
{
    Random r = new Random();
    var mapCoordinates = new MapCoordinates(r.Next(rules.ColumnsSize), r.Next(rules.RowsSize));
    shipLocation.ChangeLocation(mapCoordinates, r.Next(2) > 0);
}

void PrintMap(char[,] map)
{
    Console.WriteLine("Map");
    for (int i = 0; i < map.GetLength(0); i++)
    {
        var sb = new StringBuilder();

        for (int j = 0; j < map.GetLength(1); j++)
        {
            sb.Append(map[i, j]);
        }

        Console.WriteLine(sb);
    }

    Console.WriteLine();
}