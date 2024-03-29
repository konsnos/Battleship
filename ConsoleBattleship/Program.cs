﻿using System.Text;
using BattleshipEngine;
using BattleshipEngine.Maps;
using BattleshipEngine.Players;
using BattleshipEngine.Players.AIs;

Console.WriteLine("Setting up session");

var rules = new Rules(4, 4);
rules.AddShip("Cruiser", 3, 2);

var battleshipSession = new BattleshipSession(rules);

var aiPlayer = new AIPlayer("Hunter AI", AIFactory.GetAI(battleshipSession, AIType.Hunter));
var humanPlayer = new HumanPlayer("Konstantinos");

string[] playerOrder = { aiPlayer.Name, humanPlayer.Name };

battleshipSession.SetPlayers(new[] { aiPlayer }, new[] { humanPlayer }, playerOrder);
Console.WriteLine("Players set");

var playerMap = MapFactory.GetCreateMap(rules);

// map.PositionShip(new ShipLocation(rules.ShipsInMap[0], new MapCoordinates(0, 0), true));
// map.PositionShip(new ShipLocation(rules.ShipsInMap[1], new MapCoordinates(1, 1), false));

foreach (var ship in rules.ShipsInMap)
{
    var positionedShip = playerMap.PositionShipInRandomCoordinatesUnsafe(ship);
    Console.WriteLine(
        $"Player positioned {positionedShip.Ship.Name}, at {positionedShip.StartingTile}, horizontal {positionedShip.IsHorizontal}");
}

var humanPlayerMaps = new Dictionary<string, ICreateMap>()
{
    { humanPlayer.Name, playerMap }
};

battleshipSession.SetMaps(humanPlayerMaps);
Console.WriteLine("Maps set");

int currentTurn = 0;

battleshipSession.PlayerTurnResultExecuted += OnPlayerTurnResultExecuted;
battleshipSession.TurnChanged += OnTurnChanged;
battleshipSession.SessionCompleted += OnSessionCompleted;
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
        var enemyMap = battleshipSession.GetTargetMap(aiPlayer.Name);

        Console.WriteLine($"{aiPlayer.Name} target map");
        PrintMap(enemyMap.GetFiredCoordinatesForPrint());

        MapCoordinates fireCoordinates = new MapCoordinates();
        bool foundCoordinates = false;
        do
        {
            Console.Write($"{battleshipSession.CurrentPlayer.Name} set fire coordinates (column row): ");
            string input = Console.ReadLine() ?? string.Empty;

            string[] inputs = input.Split(" ");
            if (inputs.Length < 2)
                continue;
            bool success = int.TryParse(inputs[0], out int column);
            if (!success)
                continue;

            success = int.TryParse(inputs[1], out int row);
            if (!success)
                continue;

            fireCoordinates.ChangeCoordinates(column, row);
            bool firedAt = enemyMap.AreCoordinatesFiredAt(fireCoordinates);
            if (firedAt)
                continue;
            foundCoordinates = true;
        } while (!foundCoordinates);

        bool successfulTurn = battleshipSession.PlayHumanTurn(aiPlayer, fireCoordinates);

        if (successfulTurn) continue;

        Console.WriteLine("Illegal move. Try again");
    }
}

Console.WriteLine("Program ended...");

void OnPlayerTurnResultExecuted(PlayerTurnResult playerTurnResult)
{
    Console.Write(
        $"{playerTurnResult.PlayerAction.Player.Name} attacked {playerTurnResult.PlayerAction.EnemyPlayer.Name} at {playerTurnResult.PlayerAction.FireCoordinates}, at turn {currentTurn}.");

    if (playerTurnResult.Hit)
    {
        Console.Write(" Hit!");
    }

    Console.WriteLine();
}

void OnTurnChanged(int turn)
{
    currentTurn = turn;
}

void OnSessionCompleted(Player winner)
{
    battleshipSession.PlayerTurnResultExecuted -= OnPlayerTurnResultExecuted;
    battleshipSession.TurnChanged -= OnTurnChanged;
    battleshipSession.SessionCompleted -= OnSessionCompleted;

    Console.WriteLine($"Session completed at turn {currentTurn}.\nWinner: {winner.Name}");
}

void PrintMap(char[,] mapChars)
{
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