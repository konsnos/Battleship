using System.Text;
using BattleshipEngine;
using BattleshipEngine.Maps;
using BattleshipEngine.Players;
using BattleshipEngine.Players.Strategies;

Console.WriteLine("Setting up session");

var rules = new Rules();

BattleshipSession battleshipSession = new BattleshipSession(rules);

AIPlayer aiPlayer = new AIPlayer("Random Strategy", new RandomStrategy());
AIPlayer humanPlayer = new AIPlayer("Konstantinos", new RandomStrategy());

battleshipSession.SetPlayers(new[] { aiPlayer, humanPlayer }, Array.Empty<HumanPlayer>(), new[] { aiPlayer.Name, humanPlayer.Name });
Console.WriteLine("Players set");

Map map = new Map(rules);

foreach (var ship in rules.ShipsInMap)
{
    map.PositionShipInRandomCoordinatesUnsafe(ship);
}

var humanPlayerMaps = new Dictionary<string, Map>()
{
    { humanPlayer.Name, map }
};

battleshipSession.SetMaps(humanPlayerMaps);
Console.WriteLine("Maps set");

battleshipSession.OnPlayerTurnResultExecuted += OnPlayerTurnResultExecuted;
battleshipSession.OnTurnChanged += OnTurnChanged;
battleshipSession.OnSessionCompleted += OnSessionCompleted;
Console.WriteLine("Starting...");
battleshipSession.Start();

while (battleshipSession.SessionState == SessionState.WaitingForPlayerTurn)
{
    if (battleshipSession.CurrentPlayer.IsAI)
    {
        // Console.WriteLine($"Playing {battleshipSession.CurrentPlayer.Name}");
        battleshipSession.PlayAITurn();
        
    }
    else
    {
        //todo: implement player input
        Console.WriteLine("Not implemented human input");
    }
}

Console.WriteLine("Program ended...");

void OnPlayerTurnResultExecuted(PlayerTurnResult playerTurnResult)
{
    if (playerTurnResult.ShipHitInfo.IsShipWrecked)
    {
        Console.WriteLine($"Player {playerTurnResult.PlayerAction.Player.Name} turn {battleshipSession.CurrentTurn} wrecked ship of {playerTurnResult.PlayerAction.EnemyPlayer.Name} at {playerTurnResult.PlayerAction.FireCoordinates}");
        // Console.WriteLine($"Hit {playerTurnResult.Hit}, ship wrecked {playerTurnResult.ShipHitInfo.IsShipWrecked}");
    }
}

void OnTurnChanged(int turn)
{
    
}

void OnSessionCompleted(Player winner)
{
    battleshipSession.OnPlayerTurnResultExecuted -= OnPlayerTurnResultExecuted;
    battleshipSession.OnTurnChanged -= OnTurnChanged;
    battleshipSession.OnSessionCompleted -= OnSessionCompleted;
    
    Console.WriteLine($"Session completed. Winner {winner.Name}");
}

void PrintMap(char[,] mapChars)
{
    Console.WriteLine("Map");
    for (int r = 0; r < mapChars.GetLength(0); r++)
    {
        var sb = new StringBuilder();

        for (int c = 0; c < mapChars.GetLength(1); c++)
        {
            sb.Append(mapChars[r, c]);
        }

        Console.WriteLine(sb);
    }

    Console.WriteLine();
}