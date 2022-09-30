using BattleshipEngine.Players.AIs;

namespace BattleshipEngine.Players
{
    /// <summary>
    /// Extends player and defines the strategy which the AI player will use to place ships and attack the enemy.
    /// </summary>
    public class AIPlayer : Player
    {
        public override bool IsAI => true;

        public BaseAI BaseAi { get; }

        public AIPlayer(string name, BaseAI baseAi) : base(name)
        {
            BaseAi = baseAi;
            BaseAi.AssignPlayer(this);
        }

        public AIPlayer(Player player, BaseAI baseAi) : base(player)
        {
            BaseAi = baseAi;
            BaseAi.AssignPlayer(this);
        }

        public AIPlayer(AIPlayer aiPlayer) : base(aiPlayer)
        {
            BaseAi = aiPlayer.BaseAi;
        }

        public override Player Clone()
        {
            return new AIPlayer(this);
        }
    }
}