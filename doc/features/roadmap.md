---
uid: roadmap
---

# Roadmap
This page provides an overview of upcoming development for the platform. 

## Features

### Matches 
* Track play sessions and associated data
* Schedule competitive/cooperative games
* Pre-configure player to game role mapping
* Provide synchronisation mechanism for match/round/session start
* Awarding Resources for completion of matches

### Matchmaking
* Matchmaking system to match players based on arbitrary criteria (eg. score, previous games)
* Plugin external matching algorithms (to support matching functions from [RAGE](http://rageproject.eu/))
* Integration with Tournaments

### Tournaments
* Create a variety of team based game dynamics for tournaments that can be configured as: Round-robin, Knock-out or a combination of both
* Awarding Resources for completion of Tournaments

### Achievements
* Integration with achievement systems on popular app stores and game distribution platforms.
* Challenge System, limited time achievements which will act as challenges for players, can track which actors started challenge, who completed the challenge and who abandoned it

### Group Permissioning
* Group roles with configurable permissions
* Group resource accessibility restrictions
* Group Alliance
* Extended Group Achievements

### Resources
* History of owners for resources
* Ability to rate resources
* Trading System, actors will be able to propose trades to other actors that will need Accepting before the trade is made
* Read/write access management for group resources

### Standalone API
* Create an offline and/or in memory <xref:gameData> store and expose criteria evaluation functions.
* Implement state machine and game logic decisions using GameData evaluation

### WebSockets
* Add push messaging and event/message aggregation to API.