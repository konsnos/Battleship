using System.ComponentModel;

namespace BattleshipEngine.Players.AIs
{
    public class AIFactory
    {
        public static BaseAI GetAI(BattleshipSession battleshipSession, AIType aiType)
        {
            return aiType switch
            {
                AIType.Random => new RandomAI(battleshipSession),
                AIType.Hunter => new HunterAI(battleshipSession),
                _ => throw new InvalidEnumArgumentException($"{aiType} doesn't exist")
            };
        }
    }

    public enum AIType
    {
        Random,
        Hunter,
    }
}