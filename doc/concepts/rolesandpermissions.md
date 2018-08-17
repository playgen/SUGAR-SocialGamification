# Roles and Permissions

## Overview
User actions are controlled by their associated **permissions, known as Claims**. If an account does not have the right claim to authorize their activity, SUGAR will block their request. A **Role** is a collection of permissions that an user can have. The default admin will have all roles assigned to them on Database seeding, whereas new logins will be given the Account and User Role, providing claims to their own account.

## Default Roles
SUGAR provides 6 Roles as default, these are defined in the table below

Role | Claims
 --- | ---
Global | Create/Get/Update/Delete Account Source<br/>Create/Get/Delete Actor Claim<br/>Create/Get/Delete Actor Role<br/>Create Game<br/>Create/Update Match<br/>Create/Get Role<br/>Create/Get/Delete User
Game | Create/Get/Update/Delete Achievement<br/>Create/Get/Delete Actor Claim<br/>Create/Get/Delete Actor Role<br/>Update/Delete Game<br/>Create/Get Game Data<br/>Create/Update/Delete Leaderboard<br/>Create/Update Match<br/>Create/Get/Update Resource<br/>Create/Get Role
Group | Create/Get/Delete Actor Claim<br/>Create/Get Actor Data<br/>Create/Get/Delete Actor Role<br/>Create/Get/Update Alliance Request<br/>Delete Alliance<br/>Create/Get Game Data<br/>Update/Delete Group<br/>Create/Get/Update Group Member Request<br/>Delete Group Member<br/>Create/Update Match<br/>Create/Get/Update Resource<br/>Create/Get Role
User | Create/Get Actor Data<br/>Create/Get Game Data<br/>Create/Get/Update Group Member Request<br/>Delete Group Member<br/>Create/Update Match<br/>Create/Update Resource<br/>Update User<br/>Create/Get/Update User Friend Request<br/>Delete User Friend
Account | Delete Account
Role | Delete Role<br/>Create/Get/Delete Role Claim

## Adding new roles
New roles can be added by using the REST API to firstly create a new Role and then assign it the claims required, with the claims being assigned by its Id to the Id of the newly created Role.

Additionally Playgen.SUGAR.Server.Core/Authorization/ClaimController.cs will automatically create new Claims and a RoleClaim for the default roles listed above when the API is started if any AuthorizationAttributes on WebAPI methods contain Claims that do not yet exist in the database.

For example, the attribute below will result in the creation the Delete-Group Claim, with a RoleClaim created for the Group Role:
    
``` c#
    [Authorization(ClaimScope.Group, AuthorizationAction.Delete, AuthorizationEntity.Group)]
```