using BattleshipEngine.BattleshipEngineExceptions;
using BattleshipEngine.Interfaces;

namespace BattleshipEngine.Maps;

public class Map : ICreateMap, ITargetMap
{
    private Rules _rules;
    public IEnumerable<Ship> ShipsInMap => _rules.ShipsInMap;

    private HashSet<ShipLocation> _shipLocations;
    private HashSet<MapCoordinates> _occupiedTiles;
    
    private HashSet<MapCoordinates> _firedTiles;
    public IEnumerable<MapCoordinates> FiredTiles => _firedTiles;
    private HashSet<Ship> _shipsWrecked;
    public IEnumerable<Ship> ShipsWrecked => _shipsWrecked;
    public int ShipsRemaining => ShipsInMap.Count() - _shipsWrecked.Count;

    private static readonly Random Random = new Random();
    private static bool RandomBool => Random.Next(2) > 0;

    private static readonly Random _random = new Random();

    public Map(Rules newRules)
    {
        _rules = newRules;
        _shipLocations = new HashSet<ShipLocation>(_rules.ShipsInMap.Count);
        _occupiedTiles = new HashSet<MapCoordinates>();
        
        _firedTiles = new HashSet<MapCoordinates>();
        _shipsWrecked = new HashSet<Ship>();
    }

    private Map(Map other) : this(other._rules) { }

    #region IShipsMap
    public ShipLocation PositionShipInRandomCoordinatesUnsafe(Ship ship)
    {
        bool isInsideMap = false;
        bool isPositionFree = false;

        ShipLocation shipLocation = new ShipLocation(ship);

        while (!isInsideMap || !isPositionFree)
        {
            bool isHorizontal = RandomBool;
            var coordinates = GetRandomPositionForShip(isHorizontal, ship.Size);
            shipLocation.ChangeLocation(coordinates, isHorizontal);
            isInsideMap = AreCoordinatesInsideMap(shipLocation.OccupiedCoordinates);
            isPositionFree = AreCoordinatesFree(shipLocation.OccupiedCoordinates);
        }
        
        PositionShip(shipLocation);

        return new ShipLocation(shipLocation);
    }
    
    public void PositionShip(ShipLocation shipLocation)
    {
        Ship ship = shipLocation.Ship;
        
        if (!_rules.ShipsInMap.Contains(ship))
            throw new ShipNotFoundException(ship);

        var startingCoordinates = shipLocation.StartingTile;
        var isHorizontal = shipLocation.IsHorizontal;
        var coordinates = MapCoordinates.GetAllCoordinates(startingCoordinates, isHorizontal, ship.Size);
        if (!AreCoordinatesInsideMap(coordinates))
        {
            throw new OutOfMapException(startingCoordinates, isHorizontal, ship.Size);
        }

        bool isShipPositioned = IsShipPositioned(ship, out var shipLocationToAdd);
        if (isShipPositioned)
            RemoveShip(shipLocationToAdd);
        
        if (!AreCoordinatesFree(coordinates))
        {
            if(isShipPositioned)
                AddShip(shipLocationToAdd);
            throw new OccupiedTileException(startingCoordinates, isHorizontal, ship.Size);
        }

        shipLocationToAdd.ChangeLocation(startingCoordinates, isHorizontal);

        AddShip(shipLocationToAdd);
    }
    
    public bool AreCoordinatesInsideMap(MapCoordinates[] coordinatesArray)
    {
        foreach (var coordinates in coordinatesArray)
        {
            if (coordinates.Column >= _rules.ColumnsSize || coordinates.Row >= _rules.RowsSize)
            {
                return false;
            }
        }

        return true;
    }

    public bool AreCoordinatesFree(MapCoordinates[] coordinatesArray)
    {
        foreach (var coordinates in coordinatesArray)
        {
            if (_occupiedTiles.Contains(coordinates))
            {
                return false;
            }
        }

        return true;
    }

    public bool IsShipPositioned(Ship ship, out ShipLocation shipLocation)
    {
        foreach (var currentShipLocation in _shipLocations)
        {
            if (!ship.Equals(currentShipLocation.Ship)) continue;

            shipLocation = new ShipLocation(currentShipLocation);
            return true;
        }

        shipLocation = new ShipLocation(ship);
        return false;
    }
    #endregion

    private void RemoveShip(ShipLocation removeShipLocation)
    {
        foreach (var coordinates in removeShipLocation.OccupiedCoordinates)
        {
            _occupiedTiles.Remove(coordinates);
        }

        var removeShip = removeShipLocation.Ship;

        foreach (var shipLocation in _shipLocations)
        {
            if (!removeShip.Equals(shipLocation.Ship)) continue;
            
            _shipLocations.Remove(shipLocation);
            break;
        }
    }

    private void AddShip(ShipLocation shipLocation)
    {
        var coordinatesToAdd = shipLocation.OccupiedCoordinates;
        foreach (var coordinateToAdd in coordinatesToAdd)
        {
            _occupiedTiles.Add(coordinateToAdd);
        }
        _shipLocations.Add(shipLocation);
    }

    #region ITargetMap
    public bool AreCoordinatesFiredAt(MapCoordinates fireCoordinates)
    {
        return _firedTiles.Contains(fireCoordinates);
    }

    public bool FireToCoordinates(MapCoordinates fireCoordinates, out ShipHitInfo shipHitInfo)
    {
        if (fireCoordinates.Row >= _rules.RowsSize || fireCoordinates.Column >= _rules.ColumnsSize)
        {
            shipHitInfo = new ShipHitInfo();
            return false;
        }
        
        _firedTiles.Add(fireCoordinates);
        if (!_occupiedTiles.Contains(fireCoordinates))
        {
            shipHitInfo = new ShipHitInfo();
            return false;
        }
        
        foreach (var shipLocation in _shipLocations)
        {
            if (!shipLocation.CheckIfTileIsOccupied(fireCoordinates)) continue;

            bool isShipWrecked = true;
            foreach (var tile in shipLocation.OccupiedCoordinates)
            {
                if (!_firedTiles.Contains(tile))
                {
                    isShipWrecked = false;
                    break;
                }
            }

            shipHitInfo = new ShipHitInfo(shipLocation.Ship, fireCoordinates, isShipWrecked);
            if(isShipWrecked)
                _shipsWrecked.Add(shipLocation.Ship);
            return true;
        }

        throw new Exception("Internal error on calculations on occupied coordinates");
    }
    #endregion

    public MapCoordinates GetRandomCoordinates()
    {
        return new MapCoordinates(_random.Next(_rules.ColumnsSize), _random.Next(_rules.RowsSize));
    }
    
    public MapCoordinates GetRandomPositionForShip(bool isHorizontal, int size)
    {
        int columnSize = isHorizontal ? _rules.ColumnsSize - size : _rules.ColumnsSize;
        int rowSize = isHorizontal ? _rules.RowsSize : _rules.RowsSize - size;
        return new MapCoordinates(_random.Next(columnSize), _random.Next(rowSize));
    }

    private char[,] GetMapForPrint()
    {
        char[,] map = new char[_rules.RowsSize, _rules.ColumnsSize];
        for (int r = 0; r < map.GetLength(0); r++) // rows
        {
            for (int c = 0; c < map.GetLength(1); c++) // columns
            {
                map[r, c] = 'O';
            }
        }

        return map;
    }

    public char[,] GetShipPositionsForPrint()
    {
        char[,] map = GetMapForPrint();

        foreach (var shipLocation in _shipLocations)
        {
            var shipCoordinates = shipLocation.OccupiedCoordinates;
            char shipFirstCharacter = shipLocation.Ship.Name[0];
            foreach (var shipCoordinate in shipCoordinates)
            {
                map[shipCoordinate.Row, shipCoordinate.Column] = shipFirstCharacter;
            }
        }

        return map;
    }

    public char[,] GetFiredCoordinatesForPrint()
    {
        char[,] map = GetMapForPrint();
        
        const char firedCharacter = 'H';
        const char wreckCharacter = 'X';
        foreach (var mapCoordinates in _firedTiles)
        {
            map[mapCoordinates.Row, mapCoordinates.Column] = _occupiedTiles.Contains(mapCoordinates) ? wreckCharacter : firedCharacter;
        }

        return map;
    }

    public Map Clone()
    {
        return new Map(this);
    }
}