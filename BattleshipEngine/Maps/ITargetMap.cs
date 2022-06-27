namespace BattleshipEngine.Maps
{
    public interface ITargetMap
    {
        public IEnumerable<Ship> ShipsWrecked { get; }
        public int ShipsRemaining { get; }
        public IEnumerable<MapCoordinates> FiredTiles { get; }

        bool AreCoordinatesFiredAt(MapCoordinates fireCoordinates);

        bool FireToCoordinates(MapCoordinates fireCoordinates, out ShipHitInfo shipHitInfo);

        public char[,] GetFiredCoordinatesForPrint();
    }
}