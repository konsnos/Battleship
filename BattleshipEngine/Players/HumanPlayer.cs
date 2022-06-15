namespace BattleshipEngine.Players;

public class HumanPlayer : Player
{
    public override bool IsAI => false;
    
    public HumanPlayer(string name) : base(name)
    {
    }

    public HumanPlayer(Player player) : base(player)
    {
    }

    public override Player Clone()
    {
        return new HumanPlayer(this);
    }
}