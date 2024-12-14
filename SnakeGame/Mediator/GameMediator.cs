using SnakeGame.Services;

namespace SnakeGame.Mediator
{
    public class GameMediator : IGameMediator
    {
        private readonly GameService _gameService;

        public GameMediator(GameService gameService)
        {
            _gameService = gameService;
        }

        public void BroadcastGameState(object state, int instance)
        {
            _gameService.BroadcastGameState(state, instance);
        }

        public void PlaySound(string sound, int instance)
        {
            _gameService.PlaySound(sound, instance);
        }

        public void UpdateGlobalLeaderboard()
        {
            _gameService.UpdateGlobalLeaderboard();
        }

        public IReadOnlyList<string> GetSubscribersForInstance(int instance)
        {
            return _gameService.GetSubscribersForInstance(instance);
        }

        public int GetInstance(string connectionId)
        {
            return _gameService.GetInstance(connectionId);
        }
    }
}
