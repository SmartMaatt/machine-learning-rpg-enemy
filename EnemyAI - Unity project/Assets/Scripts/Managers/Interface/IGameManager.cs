public interface IGameManager
{
    ManagerStatus status { get; }

    void Startup();
    void LockApp(string reason);
}
