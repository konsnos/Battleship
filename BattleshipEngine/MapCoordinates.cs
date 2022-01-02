namespace BattleshipEngine;

public struct MapCoordinates
{
    public int column;
    public int row;

    public MapCoordinates(int newColumn, int newRow)
    {
        column = newColumn;
        row = newRow;
    }

    public bool Equals(MapCoordinates other)
    {
        return column == other.column && row == other.row;
    }

    public override string ToString()
    {
        return $"[{column},{row}]";
    }

    public static MapCoordinates[] GetAllCoordinates(MapCoordinates startingCoordinates, bool isHorizontal, int size)
    {
        MapCoordinates[] mapCoordinatesArray = new MapCoordinates[size];

        mapCoordinatesArray[0] = startingCoordinates;
        for (int i = 1; i < size; i++)
        {
            var mapCoordinates = new MapCoordinates(isHorizontal
                    ? startingCoordinates.column + i
                    : startingCoordinates.column,
                isHorizontal ? startingCoordinates.row : startingCoordinates.row + i
            );
            mapCoordinatesArray[i] = mapCoordinates;
        }

        return mapCoordinatesArray;
    }
}