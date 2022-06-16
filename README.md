# Battleship Engine plug in

The goal of this project is to create a battleship game plug-in.

## Todo
* Add more strategies for AI

## How to use

### 1. Create rules
Rules contain map size and ships to be added in maps.

A default ruleset can be created with an empty constructor. This will create a map size of 10 and add the following ships
1. Carrier, size 5, 1 time
2. Battleship, size 4, 1 time
3. Destroyer, size 3, 2 times
4. Submarine, size 3, 2 times
5. Patrol Boat, size 2, 2 times

Rules can be created manually by defining columns and rows and then adding the ships

```
var rules = new Rules(4, 4);
rules.AddShip("Cruiser", 3, 2);
```

### 2. Create the session
A session must then be created to the game. This will handle turns and win conditions.

```
BattleshipSession battleshipSession = new BattleshipSession(rules);
```

### 3. Add the players
The game can be played with 2 players. Theoritically more can be added but this hasn't been tested.

Players can be either AI or Human. AI players can be given a strategy class that defines their way of play.

An array with the player names will also need to be created that defines the player order.

```
AIPlayer aiPlayer = new AIPlayer("Random Strategy", new RandomStrategy());
HumanPlayer humanPlayer = new HumanPlayer("Konstantinos");

string[] playerOrder = { aiPlayer.Name, humanPlayer.Name };

battleshipSession.SetPlayers(new[] { aiPlayer }, new[] { humanPlayer }, playerOrder);
```


### 4. Set ships in human maps
Human maps must be created and be assigned their ships. AI players will create them on their own based on their Strategy class.

```
Map map = new Map(rules);

map.PositionShip(new ShipLocation(rules.ShipsInMap[0], new MapCoordinates(0, 0), true));
map.PositionShip(new ShipLocation(rules.ShipsInMap[1], new MapCoordinates(1, 1), false));

var humanPlayerMaps = new Dictionary<string, Map>()
{
    { humanPlayer.Name, map }
};

battleshipSession.SetMaps(humanPlayerMaps);
```

### 5. Start the session
By calling `battleshipSession.Start();` the turn is set and player input is expected. In case anything from the previous commands didn't run correctly an exception will be thrown.

Event `OnTurnChanged` will be invoked signaling the start of the first turn.

### 6. Play turns
AI turns must be called for execution with `battleshipSession.PlayAITurn()`.
Human turns must be the target and the fire coordinates.
```
battleshipSession.PlayHumanTurn(aiPlayer, new MapCoordinates(0, 0));
```

Whenever a turn is executed event `OnPlayerTurnResultExecuted` will be invoked with the result of the turn.


### 7. Game end
Finally whenever event `OnSessionCompleted` is invoked there is a winner!