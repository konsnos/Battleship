namespace BattleshipEngine.Maps
{
    public struct MapCoordinates : IEquatable<MapCoordinates>
    {
        public int Column { get; private set; }

        public int Row { get; private set; }

        public MapCoordinates(int column, int row)
        {
            Column = column;
            Row = row;
        }

        public void ChangeCoordinates(int column, int row)
        {
            Column = column;
            Row = row;
        }

        public override string ToString()
        {
            return $"[{Column},{Row}]";
        }

        /// <summary>
        /// Returns an array of coordinates that take up the space.
        /// </summary>
        /// <param name="startingCoordinates">Starting coordinates.</param>
        /// <param name="isHorizontal">If the size will be calculated horizontal or vertical.</param>
        /// <param name="size">Amount of coordinates required.</param>
        /// <returns>An array of coordinates.</returns>
        public static MapCoordinates[] GetCoordinates(MapCoordinates startingCoordinates, bool isHorizontal,
            int size)
        {
            var mapCoordinatesArray = new MapCoordinates[size];

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