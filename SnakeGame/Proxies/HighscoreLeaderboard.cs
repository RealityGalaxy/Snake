using SnakeGame.Models;
using System.Linq;
using System.Runtime.CompilerServices;

namespace SnakeGame.Proxies
{
    public class HighscoreLeaderboard : ILeaderboard
    {
        private readonly SortedList<int, List<string>> _scores = new SortedList<int, List<string>>();
        private int maxScoreCount = 10;

        public void AddScore(int score, string playerName)
        {
            if (_scores.ContainsKey(score) && _scores[score].Contains(playerName)) return;

            if (IsHighScore(score))
            {
                if (!_scores.ContainsKey(score))
                {
                    _scores[score] = new List<string>(); 
                }
                _scores[score].Add(playerName);

                if (_scores.Count > maxScoreCount)
                {
                    _scores.RemoveAt(0);
                }
            }
        }

        public List<KeyValuePair<int, string>> GetTopScores()
        {
            var topScores = new List<KeyValuePair<int, string>>();

            foreach (var scoreEntry in _scores.OrderByDescending(x => x.Key).Take(maxScoreCount))
            {
                if (scoreEntry.Value != null)
                {
                    foreach (var playerName in scoreEntry.Value)
                    {
                        topScores.Add(new KeyValuePair<int, string>(scoreEntry.Key, playerName));
                    }
                }
            }

            return topScores;
        }

        public bool IsHighScore(int score)
        {
            return _scores.Count < maxScoreCount || score > _scores.Keys.First();
        }

        public void UpdateLeaderboard(List<KeyValuePair<int, string>> scores)
        {
            _scores.Clear();
            foreach (var score in scores)
            {
                if (!_scores.ContainsKey(score.Key))
                {
                    _scores[score.Key] = new List<string>(); // Initialize the list if it's not already initialized
                }

                _scores[score.Key].Add(score.Value);
            }
        }
    }
}
