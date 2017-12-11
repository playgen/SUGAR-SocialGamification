using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using PlayGen.SUGAR.Common;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.Core.Controllers
{
    public class RelationshipController
    {
	    private readonly ILogger _logger;
	    private readonly EntityFramework.Controllers.RelationshipController _relationshipDbController;

	    public RelationshipController(
		    ILogger<RelationshipController> logger,
			EntityFramework.Controllers.RelationshipController relationshipDbController)
	    {
		    _logger = logger;
		    _relationshipDbController = relationshipDbController;
	    }

		/// <summary>
		/// Get relationship requests from an actor type to an actor
		/// </summary>
		/// <param name="actorId">The recipient of the request</param>
		/// <param name="fromActorType">The actor type that sent the request</param>
		/// <returns></returns>
		public List<Actor> GetRequests(int actorId, ActorType fromActorType)
	    {
		    var requesting = _relationshipDbController.GetRequests(actorId, fromActorType);

		    _logger.LogInformation($"{requesting?.Count} Requests for ActorId: {actorId}");

		    return requesting;
	    }

		/// <summary>
		/// Get relationship requests that an actor has sent to an actor type
		/// </summary>
		/// <param name="actorId">The actor who has sent requests</param>
		/// <param name="toActorType">The actor type that has received the requests</param>
		/// <returns></returns>
	    public List<Actor> GetSentRequests(int actorId, ActorType toActorType)
	    {
		    var requests = _relationshipDbController.GetSentRequests(actorId, toActorType);

		    _logger.LogInformation($"{requests?.Count} Sent Requests for ActorId: {requests}");

		    return requests;
	    }

		/// <summary>
		/// Get relationships shared between an actor and other actor types
		/// </summary>
		/// <param name="actorId">The actor to get list of relationsips with</param>
		/// <param name="actorType">The tyoe of actor that relationship is shared with</param>
		/// <returns></returns>
	    public List<Actor> GetRelationships(int actorId, ActorType actorType)
	    {
		    var relationships = _relationshipDbController.GetRelationships(actorId, actorType);

		    _logger.LogInformation($"{relationships?.Count} relationships for ActorId: {actorId}");

		    return relationships;
	    }

		/// <summary>
		/// Get a count of relationships shared between an actor and other actor types
		/// </summary>
		/// <param name="actorId">The actor to get a list of relationships with</param>
		/// <param name="actorType">The type of actor that relationship is shared with</param>
		/// <returns></returns>
	    public int GetRelationshipCount(int actorId, ActorType actorType)
	    {
		    var count = _relationshipDbController.GetRelationshipCount(actorId, actorType);
		    return count;
	    }

		/// <summary>
		/// Create a new relationship between actores
		/// </summary>
		/// <param name="newRelationship"></param>
		/// <param name="autoAccept">If the relationship is accepted immediately</param>
	    public void CreateRequest(ActorRelationship newRelationship, bool autoAccept)
	    {
		    if (autoAccept)
		    {
			    _relationshipDbController.CreateRelationship(newRelationship);
		    }
		    else
		    {
			    _relationshipDbController.CreateRelationshipRequest(newRelationship);
		    }

		    _logger.LogInformation($"{newRelationship?.RequestorId} -> {newRelationship?.AcceptorId}, Auto Accept: {autoAccept}");
	    }

	    /// <summary>
	    /// Update an existing relationship request between actors
	    /// </summary>
	    /// <param name="newRelationship"></param>
	    /// <param name="accepted">If the relationship has been accepted by one of the actors</param>
		public void UpdateRequest(ActorRelationship relationship, bool accepted)
	    {
		    _relationshipDbController.UpdateRequest(relationship, accepted);

		    _logger.LogInformation($"{relationship?.RequestorId} -> {relationship?.AcceptorId}, Accepted: {accepted}");
	    }

		/// <summary>
		/// Update an existing relationship between actors
		/// </summary>
		/// <param name="relationship"></param>
	    public void Update(ActorRelationship relationship)
	    {
			//todo Check if user is only group admin
		    _relationshipDbController.Update(relationship);
		    //todo Remove ActorRole for group if user has permissions

		    _logger.LogInformation($"{relationship?.RequestorId} -> {relationship?.AcceptorId}");
	    }
	}
}
