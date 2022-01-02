namespace BattleshipEngine;

public struct Ship
{
    public string Name { get; private set; }
    public int Size { get; private set; }

    public Ship(string newName, int newSize)
    {
        Name = newName;
        Size = newSize;
    }
}