using System;
using System.ComponentModel.DataAnnotations;

namespace PlayGen.SUGAR.Contracts
{
	/// <summary>
	/// Encapsulates resource details from the server.
	/// </summary>
	public class ResourceResponse
	{
		/// <summary>
		/// The id of the Actor which this Resource relates to.
		/// </summary>
		[Required]
		public int ActorId { get; set; }

		/// <summary>
		/// The id of the Game which this Resource relates to.
		/// </summary>
		[Required]
		public int GameId { get; set; }

		/// <summary>
		/// The identifier/name of the Resource.
		/// </summary>
		public string Key { get; set; }

		/// <summary>
		/// The amount of the Resource belonging to the actor/game.
		/// </summary>
		public long Quantity { get; set; }

		/// <summary>
		/// The DateTime at which this Resource was created.
		/// </summary>
		public DateTime DateCreated { get; set; }

		/// <summary>
		/// The DateTime at which this data was last edited.
		/// </summary>
		public DateTime DateModified { get; set; }
	}
}
