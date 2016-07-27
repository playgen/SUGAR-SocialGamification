namespace PlayGen.SUGAR.Data.Model
{
	public class Account
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public string Password { get; set; }

		public int UserId { get; set; }

		public virtual User User { get; set; }
	}
}
