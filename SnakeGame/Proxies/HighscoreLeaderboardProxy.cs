using SnakeGame.Models;
using System.Runtime.InteropServices;

namespace SnakeGame.Proxies
{
    public class HighscoreLeaderboardProxy : ILeaderboard
    {
        private readonly HighscoreLeaderboard _realLeaderboard = new HighscoreLeaderboard();
        private List<KeyValuePair<int, string>> _cachedTopScores;
        private bool _isCacheValid = false;

        public void AddScore(int score, string playerName)
        {
            _realLeaderboard.AddScore(score, playerName);
            _isCacheValid = false; // Invalidate cache
        }

        public List<KeyValuePair<int, string>> GetTopScores()
        {
            if (!_isCacheValid)
            {
                _cachedTopScores = _realLeaderboard.GetTopScores();
                _isCacheValid = true;
            }
            return _cachedTopScores;
        }

        public bool IsHighScore(int score)
        {
            return _realLeaderboard.IsHighScore(score);
        }

        public void UpdateLeaderboard(List<KeyValuePair<int, string>> scores)
        {
            // No need to update the individual leaderboard; this method does nothing
        }
    }

}
