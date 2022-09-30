namespace BattleshipEngine.Players
{
    /// <summary>
    /// Abstract class that contains the name of the player and if it's AI.
    /// </summary>
    public abstract class Player : IEquatable<Player>
    {
        public string Name { get; }

        public abstract bool IsAI { get; }

        protected Player(string name)
        {
            Name = name;
        }

        protected Player(Player player)
        {
            Name = player.Name;
        }

        public abstract Player Clone();

        public bool Equals(Player? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Name == other.Name;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Player)obj);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}