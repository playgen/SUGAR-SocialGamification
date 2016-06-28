using System;

namespace PlayGen.SUGAR.Data.Model.Interfaces
{
	public interface IModificationHistory
	{
		DateTime DateCreated { get; set; }
		DateTime DateModified { get; set; }
	}
}