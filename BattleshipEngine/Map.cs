using BattleshipEngine.BattleshipEngineExceptions;
using BattleshipEngine.Interfaces;

namespace BattleshipEngine;

public class Map : IShipsMap
{
    private Rules rules;
    private List<ShipLocation> shipLocations;
    private List<MapCoordinates> occupiedTiles;

    public Map(Rules newRules)
    {
        rules = newRules;
        shipLocations = new List<ShipLocation>(rules.shipsInMap.Count);
        occupiedTiles = new List<MapCoordinates>();
    }

    public void PositionShip(string shipName, int index, MapCoordinates newCoordinates, bool isHorizontal)
    {
        bool found = GetShip(shipName, index, out var ship);

        if (!found)
        {
            throw new ShipNotFoundException(shipName, index);
        }

        var coordinates = MapCoordinates.GetAllCoordinates(newCoordinates, isHorizontal, ship.Size);
        if (!IsCoordinatesInsideMap(coordinates))
        {
            throw new OutOfMapException(newCoordinates, isHorizontal, ship.Size);
        }

        bool isShipPositioned = IsShipPositioned(shipName, index, out ShipLocation newShipLocation);
        if (isShipPositioned)
            RemoveShip(newShipLocation);
        
        if (!IsCoordinatesFree(coordinates))
        {
            AddShip(newShipLocation);
            throw new OccupiedTileException(newCoordinates, isHorizontal, ship.Size);
        }

        if (isShipPositioned)
        {
            newShipLocation.ChangeLocation(newCoordinates, isHorizontal);
        }
        else
        {
            newShipLocation = new ShipLocation(ship, newCoordinates, isHorizontal);
        }

        AddShip(newShipLocation);
    }

    private void RemoveShip(ShipLocation shipLocation)
    {
        foreach (var coordinates in shipLocation.GetCoordinatesFromShipLocation())
        {
            occupiedTiles.Remove(coordinates);
        }

        shipLocations.Remove(shipLocation);
    }

    private void AddShip(ShipLocation shipLocation)
    {
        occupiedTiles.AddRange(shipLocation.GetCoordinatesFromShipLocation());
        shipLocations.Add(shipLocation);
    }

    public bool IsCoordinatesInsideMap(MapCoordinates[] coordinatesArray)
    {
        foreach (var coordinates in coordinatesArray)
        {
            if (coordinates.column >= rules.ColumnsSize || coordinates.row >= rules.RowsSize)
            {
                return false;
            }
        }

        return true;
    }

    public bool IsCoordinatesFree(MapCoordinates[] coordinatesArray)
    {
        foreach (var coordinates in coordinatesArray)
        {
            if (occupiedTiles.Contains(coordinates))
            {
                return false;
            }
        }

        return true;
    }

    public bool IsShipPositioned(string shipName, int index, out ShipLocation positionedShip)
    {
        int foundIndex = 0;
        foreach (var shipLocation in shipLocations)
        {
            if (!shipName.Equals(shipLocation.Ship.Name)) continue;

            if (foundIndex == index)
            {
                positionedShip = shipLocation;
                return true;
            }
            
            foundIndex++;
        }

        positionedShip = new ShipLocation();
        return false;
    }

    private bool GetShip(string shipName, int index, out Ship ship)
    {
        int shipIndex = 0;
        foreach (var keyValuePair in rules.shipsInMap)
        {
            ship = keyValuePair.Key;
            if (!shipName.Equals(ship.Name)) continue;
            
            for (int i = 0; i < keyValuePair.Value; i++)
            {
                if (shipIndex == index)
                    return true;

                shipIndex++;
            }
        }

        ship = new Ship();
        return false;
    }

    public char[,] GetMapForPrint()
    {
        char[,] map = new char[rules.RowsSize, rules.ColumnsSize];
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                map[i, j] = 'O';
            }
        }

        foreach (var shipLocation in shipLocations)
        {
            var shipCoordinates = shipLocation.GetCoordinatesFromShipLocation();
            char c = shipLocation.Ship.Name[0];
            foreach (var shipCoordinate in shipCoordinates)
            {
                map[shipCoordinate.row, shipCoordinate.column] = c;
            }
        }

        return map;
    }
}