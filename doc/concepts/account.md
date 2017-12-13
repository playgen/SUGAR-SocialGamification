---
uid: account
---

# Account
Accounts are used by the User's to register and login to SUGAR.
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