using BattleshipEngine.BattleshipEngineExceptions;
using BattleshipEngine.Interfaces;

namespace BattleshipEngine;

public class Map : IShipsMap
{
    private Rules rules;
    private List<ShipLocation> shipLocations;
    private HashSet<MapCoordinates> occupiedTiles;
    
    private HashSet<MapCoordinates> firedTiles;
    private HashSet<Ship> shipsWrecked;

    public Map(Rules newRules)
    {
        rules = newRules;
        shipLocations = new List<ShipLocation>(rules.shipsInMap.Count);
        occupiedTiles = new HashSet<MapCoordinates>();
        
        firedTiles = new HashSet<MapCoordinates>();
        shipsWrecked = new HashSet<Ship>();
    }

    #region IShipsMap
    public void PositionShip(Ship newShip, MapCoordinates newCoordinates, bool isHorizontal)
    {
        if (!rules.shipsInMap.Contains(newShip))
            throw new ShipNotFoundException(newShip);
        
        var coordinates = MapCoordinates.GetAllCoordinates(newCoordinates, isHorizontal, newShip.Size);
        if (!IsCoordinatesInsideMap(coordinates))
        {
            throw new OutOfMapException(newCoordinates, isHorizontal, newShip.Size);
        }

        bool isShipPositioned = IsShipPositioned(newShip, out ShipLocation newShipLocation);
        if (isShipPositioned)
            RemoveShip(newShipLocation);
        
        if (!IsCoordinatesFree(coordinates))
        {
            AddShip(newShipLocation);
            throw new OccupiedTileException(newCoordinates, isHorizontal, newShip.Size);
        }

        newShipLocation.ChangeLocation(newCoordinates, isHorizontal);

        AddShip(newShipLocation);
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
    #endregion

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
        var coordinatesToAdd = shipLocation.GetCoordinatesFromShipLocation();
        foreach (var coordinateToAdd in coordinatesToAdd)
        {
            occupiedTiles.Add(coordinateToAdd);
        }
        shipLocations.Add(shipLocation);
    }

    public bool IsShipPositioned(Ship ship, out ShipLocation positionedShip)
    {
        foreach (var shipLocation in shipLocations)
        {
            if (!ship.Equals(shipLocation.Ship)) continue;

            positionedShip = shipLocation;
            return true;
        }

        positionedShip = new ShipLocation(ship, new MapCoordinates(), true);
        return false;
    }

    public bool IsCoordinatesFiredAt(MapCoordinates fireCoordinates)
    {
        return firedTiles.Contains(fireCoordinates);
    }

    public bool FireToCoordinates(MapCoordinates fireCoordinates, out ShipHitInfo shipHitInfo)
    {
        firedTiles.Add(fireCoordinates);
        foreach (var shipLocation in shipLocations)
        {
            if (!shipLocation.CheckIfTileIsOccupied(fireCoordinates)) continue;

            var tilesOccupied = shipLocation.GetCoordinatesFromShipLocation();
            bool isShipWrecked = true;
            foreach (var tile in tilesOccupied)
            {
                if (!firedTiles.Contains(tile))
                {
                    isShipWrecked = false;
                    break;
                }
            }

            shipHitInfo = new ShipHitInfo(shipLocation.Ship, fireCoordinates, isShipWrecked);
            return true;
        }

        shipHitInfo = new ShipHitInfo();
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