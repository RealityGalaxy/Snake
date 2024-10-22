namespace SnakeGame.Hubs
{
    public class Subscriber
    {
        public string ConnectionId { get; set; }
        public int InstanceNumber { get; set; } = 0;

        public Subscriber(string connectionId)
        {
            ConnectionId = connectionId;
        }
    }
}
