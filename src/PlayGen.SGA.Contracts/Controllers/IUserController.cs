namespace PlayGen.SGA.Contracts.Controllers
{
    public interface IUserController
    {
        int Create(string name);

        Actor Get(string name);

        void Delete(int id);
    }
}
