namespace PlayGen.SUGAR.Data.Model
{
	public class Account
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public string Password { get; set; }

		public int AccountSourceId { get; set; }

		public virtual AccountSource AccountSource { get; set; }

		public int UserId { get; set; }

		public virtual User User { get; set; }

		public bool Deleted { get; set; }

		public override string ToString()
		{
			return $"Id: {Id}, Name: {Name}, AccountSource: {AccountSource}, UserId: {UserId}, Deleted: {Deleted}";
		}
	}
}
