namespace BattleshipEngine.Maps
{
    public struct MapCoordinates : IEquatable<MapCoordinates>
    {
        public int Column { private set; get; }

        public int Row { private set; get; }

        public MapCoordinates(int newColumn, int newRow)
        {
            Column = newColumn;
            Row = newRow;
        }

        public override string ToString()
        {
            return $"[{Column},{Row}]";
        }

        public static MapCoordinates[] GetAllCoordinates(MapCoordinates startingCoordinates, bool isHorizontal,
            int size)
        {
            MapCoordinates[] mapCoordinatesArray = new MapCoordinates[size];

            mapCoordinatesArray[0] = startingCoordinates;
            for (int i = 1; i < size; i++)
            {
                var mapCoordinates = new MapCoordinates(isHorizontal
                        ? startingCoordinates.Column + i
                        : startingCoordinates.Column,
                    isHorizontal ? startingCoordinates.Row : startingCoordinates.Row + i
                );
                mapCoordinatesArray[i] = mapCoordinates;
            }

            return mapCoordinatesArray;
        }

        public bool Equals(MapCoordinates other)
        {
            return Column == other.Column && Row == other.Row;
        }

        public override bool Equals(object? obj)
        {
            return obj is MapCoordinates other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Column, Row);
        }
    }
}