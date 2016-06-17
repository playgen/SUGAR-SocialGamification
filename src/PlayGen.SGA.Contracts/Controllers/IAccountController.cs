namespace PlayGen.SGA.Contracts.Controllers
{
    public interface IAccountController
    {
        void Register(Account newAccount);

        void Login(Account account);

        void Delete(int[] id);
    }
}