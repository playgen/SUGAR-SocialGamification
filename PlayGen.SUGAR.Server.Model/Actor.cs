using PlayGen.SUGAR.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlayGen.SUGAR.Server.Model
{
    public abstract class Actor
    {
        public int Id { get; set; }

        public string Name { get; set; }

		public string Description { get; set; }

	    public virtual List<ActorRelationship> Requestors { get; set; }

	    public virtual List<ActorRelationship> Acceptors { get; set; }

	    public virtual List<ActorRelationshipRequest> RequestAcceptors { get; set; }

	    public virtual List<ActorRelationshipRequest> RequestRequestors { get; set; }

		public abstract ActorType ActorType { get; }

		[NotMapped]
	    public int GroupRelationshipCount { get; set; }

		[NotMapped]
	    public int UserRelationshipCount { get; set; }
	}
}
