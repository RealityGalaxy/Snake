using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.SignalR;
using SnakeGame.Hubs;
using SnakeGame.Mediator;
using SnakeGame.Proxies;
using System.Collections.Generic;

namespace SnakeGame.Services
{
    public class GameService : BackgroundService
    {
        public static GameService Instance { get; private set; }
        private readonly IHubContext<GameHub> _hubContext;
        private readonly ILeaderboard _globalLeaderboard;
        public List<Subscriber> Subscribers { get; private set; } = new List<Subscriber>();
        public GameInstance[] GameInstances { get; private set; } = new GameInstance[4];

        public GameService(IHubContext<GameHub> hubContext, ILeaderboard globalLeaderboard)
        {
            _hubContext = hubContext;
            _globalLeaderboard = globalLeaderboard;
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                return;
            }
        }

        public IReadOnlyList<string> GetSubscribersForInstance(int instance)
        {
            return Subscribers.Where(x => x.InstanceNumber == instance).Select(x => x.ConnectionId).ToList();
        }

        public int GetInstance(string connectionId)
        {
            var subscriber = Subscribers.SingleOrDefault(x => x.ConnectionId == connectionId);
            if (subscriber == null)
            {
                throw new InvalidOperationException($"No subscriber found with ConnectionId: {connectionId}");
            }
            return subscriber.InstanceNumber;
        }

        public int Unsubscribe(string connectionId)
        {
            var Subscriber = Subscribers.Single(x => x.ConnectionId == connectionId);

            Subscribers.Remove(Subscriber);

            return Subscriber.InstanceNumber;
        }

        public void Subscribe(string connectionId, int instance)
        {
            var existingSub = Subscribers.FirstOrDefault(x => x.ConnectionId == connectionId);
            if (existingSub != null)
            {
                existingSub.InstanceNumber = instance;
            }
            else
            {
                var Subscriber = new Subscriber(connectionId);
                Subscriber.InstanceNumber = instance;
                Subscribers.Add(Subscriber);
            }
        }

        public async void BroadcastGameState(object state, int instance)
        {
            var SubscribersList = GetSubscribersForInstance(instance);
            await _hubContext.Clients.Clients(SubscribersList).SendAsync("ReceiveGameState", state);
            await _hubContext.Clients.Clients(SubscribersList).SendAsync("ReceiveLeaderboard", GameInstances[instance]._leaderboard.GetTopScores());
        }


        public override Task StartAsync(CancellationToken cancellationToken)
        {
            IGameMediator mediator = new GameMediator(this);
            for (int i = 0; i < 4; i++)
            {
                GameInstances[i] = new GameInstance(i, mediator);
                GameInstances[i].StartTimer();
            }
            return base.StartAsync(cancellationToken);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.CompletedTask;
        }

        public async Task PlaySound(string sound, int instance)
        {
            var SubscribersList = GetSubscribersForInstance(instance);
            await _hubContext.Clients.Clients(SubscribersList).SendAsync("PlaySound", sound);
        }

        public async Task UpdateGlobalLeaderboard()
        {
            var globalTopScores = new List<KeyValuePair<int, string>>();

            // Collect top scores from all game instances
            foreach (var gameInstance in GameInstances)
            {
                if (gameInstance != null)
                {
                    var instanceTopScores = gameInstance._leaderboard.GetTopScores();
                    globalTopScores.AddRange(instanceTopScores);
                }
            }

            // Sort by score in descending order and take the top 10
            var sortedGlobalTopScores = globalTopScores
                .OrderByDescending(score => score.Key)
                .Take(10)  // Limit to the top 10 scores across all instances
                .ToList();

            _globalLeaderboard.UpdateLeaderboard(sortedGlobalTopScores);
            await BroadcastGlobalLeaderboard();
        }
        public async Task BroadcastGlobalLeaderboard()
        {
            var globalTopScores = _globalLeaderboard.GetTopScores();
            foreach (var subscriber in Subscribers)
            {
                await _hubContext.Clients.Client(subscriber.ConnectionId).SendAsync("UpdateGlobalLeaderboard", globalTopScores);
            }
        }
    }
}
