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
            // If the score is already in the leaderboard with the same player name, don't add it again
            if (_scores.ContainsKey(score) && _scores[score].Contains(playerName))
            {
                return; // No update if the same player with the same score exists
            }

            // If it's a new score, add it
            if (IsHighScore(score))
            {
                // Ensure the list is initialized for the given score
                if (!_scores.ContainsKey(score))
                {
                    _scores[score] = new List<string>(); // Initialize the list if it's not already initialized
                }

                _scores[score].Add(playerName);

                // If there are more than the top 10 scores, remove the lowest
                if (_scores.Count > maxScoreCount)
                {
                    // Remove the lowest score (first in the SortedList)
                    _scores.RemoveAt(0);
                }
            }
        }

        public List<KeyValuePair<int, string>> GetTopScores()
        {
            var topScores = new List<KeyValuePair<int, string>>();

            // Flatten the dictionary, as each score can have multiple players
            foreach (var scoreEntry in _scores.OrderByDescending(x => x.Key).Take(maxScoreCount))
            {
                // Ensure that we handle cases where there might be a null list
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
            // Check if it's a high score (if there are less than maxScoreCount scores or if it's higher than the lowest score)
            return _scores.Count < maxScoreCount || score > _scores.Keys.First();
        }

        public void UpdateLeaderboard(List<KeyValuePair<int, string>> scores)
        {
            _scores.Clear();
            foreach (var score in scores)
            {
                // Ensure the list is initialized for the given score
                if (!_scores.ContainsKey(score.Key))
                {
                    _scores[score.Key] = new List<string>(); // Initialize the list if it's not already initialized
                }

                // Add player name to the corresponding score entry
                _scores[score.Key].Add(score.Value);
            }
        }
    }
}
