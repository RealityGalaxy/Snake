using SnakeGame.Models;

namespace SnakeGame.Proxies
{
    public interface ILeaderboard
    {
        void AddScore(int score, string playerName);
        List<KeyValuePair<int, string>> GetTopScores();
        bool IsHighScore(int score);
        void UpdateLeaderboard(List<KeyValuePair<int, string>> scores);
    }
}
