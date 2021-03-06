namespace BattleshipEngine
{
    /// <summary>
    /// Defines ship name, side and its id.
    /// </summary>
    public readonly struct Ship
    {
        public string Name { get; }
        public int Size { get; }
        public int Id { get; }

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
}