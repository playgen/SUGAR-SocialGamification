# SUGAR Release Change Log
See below details for each release of SUGAR and the notable changes made.

### Next Version
The following will be available in the next version of SUGAR

### 1.3.1
- Fix Resources not returning updated amount for existing resources
- Expanded Unity client docs and startup guide

### 1.3.0
- Add Count as a CriteriaQueryType
- Add default group user claims to allow members to manage group resources (Get, Create, Update)
- Update builds for new changes
- Update SUGAR Unity docs for more detail on usage of SUGAR Manager

### 1.2.0
- Add documentation for Login Token functionality
- Add Resources.TryTake from group for upcoming functionality of allowing users to attempt to take resources based on their claims to the group.
- Add more robust tests for login token

### 1.1.0
- Add Login Tokens added to retrieve a token after a successful login or register to use for later login attempts.
- Add SavedPrefsHander interface added to save login tokens in your application
- Add Roles and Permissions information to docs
- Update Performance of GameData Addition and Leaderboard retrieval 
- Update Class to create for Achievements, Leaderboards and Skills to fix Client Issues
- Update Client async requests to allow for them to be disabled
- Update Docs for new changes and fixed a number of routing bugs
- Update tests for new changes
- Fix Admin Docs images which were missing
- Fix Description not being returned correctly for Groups
- Fix All Tests completing successfully

### 1.0.0
- Add public release of SUGAR 1.0.0
- Discontinued SUGAR 0.x.x Support