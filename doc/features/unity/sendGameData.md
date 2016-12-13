# Sending GameData

For SUGAR to evaluate achievement progresses or leaderboard standings, <xref:gameData> must be submitted to the server.
There is no script component on the core controller gameObject for this. Instead there is a static reference to the GameDataUnityClass via SUGARManager.cs.

Whenever any data that is to be evaluated by SUGAR needs to be submitted. call: ``SUGARManager.GameData.Send(key, value);`` from your game code.
