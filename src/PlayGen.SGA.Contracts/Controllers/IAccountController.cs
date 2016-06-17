namespace PlayGen.SGA.Contracts.Controllers
{
    public interface IAccountController
    {
        void Register(string name, string password);

        string Login(string name, string password);

        void Delete(string[] id);
    }
}