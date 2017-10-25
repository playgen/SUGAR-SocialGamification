using System;

namespace PlayGen.SUGAR.Server.Model.Interfaces
{
	public interface IModificationHistory
	{
		DateTime DateCreated { get; set; }
		DateTime DateModified { get; set; }
	}
}