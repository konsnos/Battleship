namespace BattleshipEngine.Maps
{
    public class MapFactory
    {
        public static ICreateMap GetCreateMap(Rules rules)
        {
            return new Map(rules);
        }
    }
}