namespace BattleshipEngine.Maps
{
    public interface ITargetMap
    {
        public int Rows { get; }
        public int Columns { get; }
        public IEnumerable<Ship> ShipsWrecked { get; }
        public int ShipsRemaining { get; }
        public IEnumerable<MapCoordinates> FiredTiles { get; }

        bool AreCoordinatesFiredAt(MapCoordinates fireCoordinates);

        bool AreCoordinatesValid(MapCoordinates mapCoordinates);

        bool FireToCoordinates(MapCoordinates fireCoordinates, out ShipHitInfo shipHitInfo);

        public char[,] GetFiredCoordinatesForPrint();
    }
}