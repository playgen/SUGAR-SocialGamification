---
uid: group
---

# Group
Groups are actors representing collection of actors. They may be individual or multiple game persistent. Groups can be moderated through administrator tools or users, or set up and managed through the game as described by the game designers. 

[Relationships](relationship.md) describe which actors belong to which groups. Groups can have associated achievements,  which can be set for all members of a group to complete. 

Actors can join, leave or add another actor to a group. 


## Features
* CRUD Groups
* CRUD Group Metadata
	* Group Name
	* Group Description
	* Group Icon
* Update Group Membership
* Search Group (ID/name/Actor)



## API
* Client
    * <xref:PlayGen.SUGAR.Client.GroupClient>
* Contracts
    * <xref:PlayGen.SUGAR.Contracts.Shared.ActorResponse>
    * <xref:PlayGen.SUGAR.Contracts.Shared.ActorRequest>

    
## Examples
* Create a group
	This example will show how to create a group called "Wildlings" using the <xref:PlayGen.SUGAR.Client.GroupClient>'s Create function, passing an <xref:PlayGen.SUGAR.Contracts.Shared.ActorRequest> as the parameter and storing the group's id returned inside the <xref:PlayGen.SUGAR.Contracts.Shared.ActorResponse> object.

```cs
		public SUGARClient sugarClient = new SUGARClient(BaseUri);
		private GroupClient _groupClient;
		private int _groupId;

		private void CreateGroup() 
		{
			// create instance of the game client
			_groupClient = sugarClient.Group;

			// create an ActorRequest
			var actorRequest = new ActorRequest 
			{
				Name = "Wildlings"
			};

			// create the group and store the response
			var actorResponse = _groupClient.Create(actorRequest);

			// store the id of the game for use in other functions
			_groupId = actorResponse.Id;
		}
```

* Retreiving a game

	Checking if a Group exists or finding the id of a Group may be desired functionalities. This is done using <xref:PlayGen.SUGAR.Client.GroupClient>'s Get function and passing the name of the group to match.

```cs 
		private bool CheckGroupExists() 
		{
			// check for the game and store the responses
			var actorResponses = _groupClient.Get("Wildlings");

			
			foreach (response in actorResponses) 
			{
				// check if the name matches the desired game exactly
				if (response.Name == "Wildlings") 
				{	
					Console.WriteLine("Sorry, the group name has been taken, try another one");
					return false;
				}
			}

			return true;
		}
```

## Roadmap
* Groups Alliance. 
Provide the ability for relationship between groups. To form an <xref:alliance>
* Group leader.
Providing the ability for individual actors to control the group membership, to add or remove other actors.

* Extended group achievements.
Provide the ability to set the parameters such as number of actors required to meet the achievement criteria before it’s considered complete. 
