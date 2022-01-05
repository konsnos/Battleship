using BattleshipEngine.BattleshipEngineExceptions;
using BattleshipEngine.Interfaces;

namespace BattleshipEngine;

public class Map : IShipsMap, ITargetMap
{
    private Rules rules;
    public IEnumerable<Ship> ShipsInMap => rules.shipsInMap;

    private HashSet<ShipLocation> shipLocations;
    private HashSet<MapCoordinates> occupiedTiles;
    
    private HashSet<MapCoordinates> firedTiles;
    private HashSet<Ship> shipsWrecked;
    public IEnumerable<Ship> ShipsWrecked => shipsWrecked;
    public int ShipsRemaining => ShipsInMap.Count() - shipsWrecked.Count;

    private static readonly Random random = new Random();
    private static bool randomBool => random.Next(2) > 0;

    public Map(Rules newRules)
    {
        rules = newRules;
        shipLocations = new HashSet<ShipLocation>(rules.shipsInMap.Count);
        occupiedTiles = new HashSet<MapCoordinates>();
        
        firedTiles = new HashSet<MapCoordinates>();
        shipsWrecked = new HashSet<Ship>();
    }

    #region IShipsMap
    public ShipLocation PositionShipInRandomCoordinatesUnsafe(Ship ship)
    {
        bool isInsideMap = false;
        bool isPositionFree = false;

        ShipLocation newShipLocation = new ShipLocation(ship);

        while (!isInsideMap || !isPositionFree)
        {
            var coordinates = rules.GetRandomCoordinates();
            newShipLocation.ChangeLocation(coordinates, randomBool);
            var shipCoordinates = newShipLocation.GetCoordinatesFromShipLocation();
            isInsideMap = IsCoordinatesInsideMap(shipCoordinates);
            isPositionFree = IsCoordinatesFree(shipCoordinates);
        }
        
        PositionShip(newShipLocation);

        return new ShipLocation(newShipLocation);
    }
    
    public void PositionShip(ShipLocation shipLocation)
    {
        Ship newShip = shipLocation.Ship;
        
        if (!rules.shipsInMap.Contains(newShip))
            throw new ShipNotFoundException(newShip);

        var newCoordinates = shipLocation.StartingTile;
        var newIsHorizontal = shipLocation.IsHorizontal;
        var coordinates = MapCoordinates.GetAllCoordinates(newCoordinates, newIsHorizontal, newShip.Size);
        if (!IsCoordinatesInsideMap(coordinates))
        {
            throw new OutOfMapException(newCoordinates, newIsHorizontal, newShip.Size);
        }

        bool isShipPositioned = IsShipPositioned(newShip, out var shipLocationToAdd);
        if (isShipPositioned)
            RemoveShip(shipLocationToAdd);
        
        if (!IsCoordinatesFree(coordinates))
        {
            if(isShipPositioned)
                AddShip(shipLocationToAdd);
            throw new OccupiedTileException(newCoordinates, newIsHorizontal, newShip.Size);
        }

        shipLocationToAdd.ChangeLocation(newCoordinates, newIsHorizontal);

        AddShip(shipLocationToAdd);
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

    public bool IsShipPositioned(Ship ship, out ShipLocation positionedShip)
    {
        foreach (var shipLocation in shipLocations)
        {
            if (!ship.Equals(shipLocation.Ship)) continue;

            positionedShip = new ShipLocation(shipLocation);
            return true;
        }

        positionedShip = new ShipLocation(ship);
        return false;
    }
    #endregion

    private void RemoveShip(ShipLocation removeShipLocation)
    {
        foreach (var coordinates in removeShipLocation.GetCoordinatesFromShipLocation())
        {
            occupiedTiles.Remove(coordinates);
        }

        var removeShip = removeShipLocation.Ship;

        foreach (var shipLocation in shipLocations)
        {
            if (!removeShip.Equals(shipLocation.Ship)) continue;
            
            shipLocations.Remove(shipLocation);
            break;
        }
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

    #region ITargetMap
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
            shipsWrecked.Add(shipLocation.Ship);
            return true;
        }

        shipHitInfo = new ShipHitInfo();
        return false;
    }
    #endregion

    public char[,] GetMapForPrint()
    {
        char[,] map = new char[rules.RowsSize, rules.ColumnsSize];
        for (int r = 0; r < map.GetLength(0); r++) // rows
        {
            for (int c = 0; c < map.GetLength(1); c++) // columns
            {
                map[r, c] = 'O';
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