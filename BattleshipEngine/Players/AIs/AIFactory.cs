using System.ComponentModel;

namespace BattleshipEngine.Players.AIs
{
    public class AIFactory
    {
        public static BaseAI GetAI(AIType aiType)
        {
            return aiType switch
            {
                AIType.Random => new RandomAI(),
                _ => throw new InvalidEnumArgumentException($"{aiType} doesn't exist")
            };
        }
    }

    public enum AIType
    {
        Random
    }
}