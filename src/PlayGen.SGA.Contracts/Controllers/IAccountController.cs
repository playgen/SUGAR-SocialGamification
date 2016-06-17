namespace PlayGen.SGA.Contracts.Controllers
{
    public interface IAccountController
    {
        AccountResponse Register(AccountRequest newAccount);

        AccountResponse Login(AccountRequest account);

        void Delete(int[] id);
    }
}