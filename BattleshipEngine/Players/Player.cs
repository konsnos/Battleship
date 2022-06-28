namespace BattleshipEngine.Players
{
    /// <summary>
    /// Abstract class that contains the name of the player and if it's AI.
    /// </summary>
    public abstract class Player
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
    }
}