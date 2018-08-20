---
uid: account
---

# Account
Accounts are used by Users to register and login to SUGAR.
Each account is associated with a user. A user may have multiple accounts but an account may only have one user.

Each account stores a unique login name and password for the associated user.

## Features
* CRUD Account
* CRUD Account Metadata
	* Account Name
	* Account Password
	* Account User (User)

## API
* Client
	* <xref:PlayGen.SUGAR.Client.AccountClient>
* Contracts
	* <xref:PlayGen.SUGAR.Contracts.AccountRequest>
	* <xref:PlayGen.SUGAR.Contracts.AccountResponse>
* WebAPI
	* <xref:PlayGen.SUGAR.Server.WebAPI.Controllers.AccountController>
   
# Account Sources
SUGAR supports accounts from multiple sources, allowing for users to be grouped by where they are logging in from. The default [Account Source](http://docs.sugarengine.org/api/PlayGen.SUGAR.Server.Model.AccountSource.html) is "SUGAR", but you can add your own through the [admin panel](../features/admin/accountsources.md) or [C# API](http://docs.sugarengine.org/api/PlayGen.SUGAR.Server.WebAPI.Controllers.AccountSourceController.html).
