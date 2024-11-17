using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using SnakeGame.Factories;
using SnakeGame.Hubs;
using SnakeGame.Models;
using SnakeGame.Models.FactoryModels;
using SnakeGame.Models.FactoryModels.Fruit;

namespace SnakeGame.Services
{
    public class GameService : BackgroundService
    {
        public static GameService Instance { get; private set; }
        private readonly IHubContext<GameHub> _hubContext;
        public List<Subscriber> Subscribers { get; private set; } = new List<Subscriber>();
        public GameInstance[] GameInstances { get; private set; } = new GameInstance[4];

        public GameService(IHubContext<GameHub> hubContext)
        {
            _hubContext = hubContext;
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
            return Subscribers.Single(x => x.ConnectionId == connectionId).InstanceNumber;
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
        }


        public override Task StartAsync(CancellationToken cancellationToken)
        {
            for(int i = 0; i < 4; i++)
            {
                GameInstances[i] = new GameInstance(i);
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
    }
}
