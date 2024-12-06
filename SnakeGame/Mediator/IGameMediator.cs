namespace SnakeGame.Mediator
{
    public interface IGameMediator
    {
        void BroadcastGameState(object state, int instance);
        void PlaySound(string sound, int instance);
        void UpdateGlobalLeaderboard();
        IReadOnlyList<string> GetSubscribersForInstance(int instance);
        int GetInstance(string connectionId);
    }
}
