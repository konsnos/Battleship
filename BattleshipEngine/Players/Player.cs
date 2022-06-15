namespace BattleshipEngine.Players;

public abstract class Player
{
    public string Name { get; private set; }

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