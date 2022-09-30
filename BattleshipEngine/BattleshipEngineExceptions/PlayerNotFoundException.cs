namespace BattleshipEngine.BattleshipEngineExceptions
{
    public class PlayerNotFoundException : Exception
    {
        private string _name;

        public PlayerNotFoundException(string name) : base($"Player with name {name} was not found")
        {
            _name = name;
        }
    }
}