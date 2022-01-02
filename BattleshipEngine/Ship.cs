namespace BattleshipEngine;

public struct Ship
{
    public string Name { get; private set; }
    public int Size { get; private set; }
    public int Id { get; private set; }

    public Ship(string newName, int newSize, int newId)
    {
        Name = newName;
        Size = newSize;
        Id = newId;
    }

    public bool Equals(Ship other)
    {
        return Name.Equals(other.Name) && Id == other.Id && Size == other.Size;
    }
}