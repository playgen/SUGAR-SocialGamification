# Roles and Permissions

## Overview
Account actions are controlled by their associated **permissions, known as Claims**, if an account does not have the right claim to authorize their activity, SUGAR will block their request. A **Role** is a collection of permissions that an account can have. The default admin will have all roles assigned to them on Database seeding, whereas new logins will be given the Account and User Role, providing claims to their own account.

## Default Roles
SUGAR provides 6 Roles as default, these are defined in the table below

Role | Claims
 --- | ---
Global | Create/Update/Delete Account Source<br/>Create/Get/Delete Actor Claim<br/>Create/Get/Delete Actor Role<br/>Create Game<br/>Create Group<br/>Create/Get Role<br/>Create/Get/Delete User
Game | Create/Get/Update/Delete Achievement<br/>Create/Get/Delete Actor Claim<br/>Create/Get/Delete Actor Role<br/>Update/Delete Game<br/>Create/Get Game Data<br/>Create/Update/Delete Leaderboard<br/>Create Resource<br/>Create Role
Group | Create/Get/Delete Actor Claim<br/>Create/Get Actor Data<br/>Create/Get/Delete Actor Role<br/>Create Game Data<br/>Update/Delete Group<br/>Create/Get/Update Group Member Request<br/> Delete Group Member<br/>Create/Update Resource<br/>Create Role
User | Get Actor Data<br/>Create/Get Actor Data<br/>Create Game Data<br/>Create/Get Group Member Request<br/>Delete Group Member<br/>Create/Update Resource<br/>Update User<br/>Create/Get/Update Friend Request<br/>Delete User Friend
Account | Delete Account
Role | Delete Role<br/>Create/Get/Delete Role Claim

## Adding new roles
In order to add new roles, this can either be done by the REST API to firstly create a new Role, and then assign it the claims required, the claims must be assigned by Id to the Id of the newly created Role.

Additionally Playgen.SUGAR.Server.Core/Authorization/ClaimController.cs, will automatically create new Roles when the API is started for AuthorizationAttributes, for example:
    
``` c#
    [Authorization(ClaimScope.Group, AuthorizationAction.Delete, AuthorizationEntity.Group)]
```

Will create the GroupDelete Claim for ClaimScope Group. 