namespace PlayGen.SUGAR.Common.Authorization
{
	public class Platform
	{
		//Id of data (evaluationData, Achievements etc) not assigned to any actors or games
		public const int GlobalGameId = 0;
		public static readonly int? GlobalActorId = null;
        //Id used to represent having ownership of all objects of a type
        public const int AllId = -1;
	}
}
