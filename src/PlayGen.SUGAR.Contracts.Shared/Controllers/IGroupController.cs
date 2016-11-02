using System.Collections.Generic;

namespace PlayGen.SUGAR.Contracts.Controllers
{
	public interface IGroupController
	{
		IEnumerable<ActorResponse> Get();

		IEnumerable<ActorResponse> Get(string[] name);

		ActorResponse Create(ActorRequest actor);

		void Delete(int[] id);
	}
}
