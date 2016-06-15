namespace PlayGen.SGA.Contracts.Controllers
{
    public interface IGameController
    {
        int Create(string name);

        Game Get(string name);

        void Delete(int id);
    }
}