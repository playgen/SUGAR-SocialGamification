namespace PlayGen.SGA.Contracts.Controllers
{
    public interface IAccountController
    {
        int Register(Account newAccount);

        string Login(string name, string password);

        void Delete(int[] id);
    }
}