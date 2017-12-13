---
uid: session
---

# Session
When a user logs in to a game, a new, unique session is created.
When the user logs out, that session is ended.

Sessions will automatically be ended if there has been no activity within a certain period.

The "Heartbeat" method can be used to keep sessions active.

## Features
* Login
* Logout
* Heartbeat

## API
* Client
	* <xref:PlayGen.SUGAR.Client.SessionClient>
* Contracts
	* <xref:PlayGen.SUGAR.Contracts.AccountRequest>
	* <xref:PlayGen.SUGAR.Contracts.AccountResponse>
* WebAPI
	* <xref:PlayGen.SUGAR.Server.WebAPI.Controllers.SessionController>