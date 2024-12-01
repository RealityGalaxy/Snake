using SnakeGame.Libraries;

namespace SnakeGame.Adapters
{
    public class SoundPlayer : ISoundPlayer
    {
        private readonly SoundManager _soundManager;

        public SoundPlayer()
        {
            string soundDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sounds");
            _soundManager = new SoundManager(soundDirectory);
        }

        public string PlaySound(string sound)
        {
            var soundPath = _soundManager.GetSoundPath(sound);

            if (soundPath == null)
            {
                throw new Exception($"Sound '{sound}' not found.");
            }

            return soundPath;

        }
    }
}
