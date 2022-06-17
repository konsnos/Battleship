namespace BattleshipEngine.BattleshipEngineExceptions
{
    public class MissingPlayerInformationException : Exception
    {
        public string PlayerName { get; }

        public MissingPlayerInformationException(string playerName, string message) : base(message)
        {
            PlayerName = playerName;
        }
    }
}