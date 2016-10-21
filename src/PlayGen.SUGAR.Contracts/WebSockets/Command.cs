namespace PlayGen.SUGAR.Contracts.WebSockets
{
    public class Command
    {
        public string Route { get; set; }

        public IRequest Request { get; set; }
    }
}
