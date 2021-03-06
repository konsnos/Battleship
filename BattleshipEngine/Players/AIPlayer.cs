using BattleshipEngine.Players.Strategies;

namespace BattleshipEngine.Players
{
    /// <summary>
    /// Extends player and defines the strategy which the AI player will use to place ships and attack the enemy.
    /// </summary>
    public class AIPlayer : Player
    {
        public override bool IsAI => true;

        public Strategy Strategy { get; }

        public AIPlayer(string name, Strategy strategy) : base(name)
        {
            Strategy = strategy;
        }

        public AIPlayer(Player player, Strategy strategy) : base(player)
        {
            Strategy = strategy;
        }

        public AIPlayer(AIPlayer aiPlayer) : base(aiPlayer)
        {
            Strategy = aiPlayer.Strategy;
        }

        public override Player Clone()
        {
            return new AIPlayer(this);
        }
    }
}