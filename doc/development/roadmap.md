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

### Matchmaking

Matching players based on arbitrary criteria using game data evaluation 

* Score matching
* History matching
* Plugin external matching algorithms (to support matching functions from RAGE)
* Integration with Tournaments

### Tournaments

Providing a variety of team based game dynamics, tournaments can be configured as:

* Round-robin
* Knock-out
* Combination of both

### Challenge system

* Issue challenges to players, groups or globally
* Set time and arbitary criteria restrictions on challenge eligibility

### Portable achievement system

Integration with achievement systems on popular app stores and game distribution platforms.

### Rewards
* [Resources](xref:resource) can be awarded for participation in Matches and Tournaments.

### Group Permissioning

* Group roles with configurable permissions
* Group resource accessibility restrictions
* Group Alliance
* Extended Group Achievements

See [Groups] (/features/group.md)

### Modular Evaluators

Providing an API to allow developers to create evaluator implementations that can be loaded at runtime via configuration and query the game data storage and/or generate rewards.

## Tech

### Standalone API

Create an offline and/or in memory <xref:gameData> store and expose criteria evaluation functions.

Implement state machine and game logic decisions using GameData evaluation

### WebSockets

Add push messaging and event/message aggregation to API.
