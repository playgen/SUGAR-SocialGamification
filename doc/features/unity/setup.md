# Setting up the database

 The asset package provides a modifiable editor script which will setup the game, achievements and leaderboards in the database. The SeedSUGARDatabase.cs script is found in "Assets/Plugins/SUGAR/Scripts". To run the script, select Tools > Seed SUGAR Database. This will show a popup to login as admin.

 In it's default state, the script will connect to the database specified in the base address field of the SUGAR Unity manager component and update the Game Id field with the row that matches the Game Token. If such a token does not exist in the database, a new entry will be made and that enty's Id will be placed into the Game Id field. 

To set up achievements and leaderboards, some editing of the script is required.   

The CreateAchievements() function contains a commented out template for creating an achievements. Multiple achievements can be made by duplicating the code. For information about each of the required fields please refer to the <xref:criteria> section.

The CreateLeaderboards() function has the same purpose but for generating leaderboards. Please refer to the <xref:leaderboardStandings> section for my information.





