using NAudio.Wave;

namespace SnakeGame.Libraries
{
    public class SoundManager
    {
        private readonly Dictionary<string, string> _sounds;
        private IWavePlayer _waveOutDevice;
        private AudioFileReader _audioFileReader;

        public SoundManager(string directory)
        {
            Console.WriteLine("Initializing sound player...");
            _sounds = new Dictionary<string, string>();
            LoadSounds(directory);
            Console.WriteLine("Sound library initialized.");
        }

        private void LoadSounds(string directory)
        {
            Console.WriteLine("Loading sounds...");
            if (!Directory.Exists(directory))
            {
                Console.WriteLine("Sounds directory not found: " + directory);
                return;
            }

            string[] soundFiles = Directory.GetFiles(directory, "*.mp3");

            foreach (string soundFile in soundFiles)
            {
                string soundName = Path.GetFileNameWithoutExtension(soundFile);
                _sounds.Add(soundName, soundFile);
                Console.WriteLine($"Loaded sound: '{soundName}' from {soundFile}");
            }
        }

        public void PlaySound(string soundFile)
        {
            Stop(); // Stop any currently playing sound

            _waveOutDevice = new WaveOutEvent();
            _audioFileReader = new AudioFileReader(soundFile);
            _waveOutDevice.Init(_audioFileReader);
            _waveOutDevice.Play();
        }

        public void Stop()
        {
            if (_audioFileReader != null)
            {
                _audioFileReader.Dispose();
                _audioFileReader = null;
            }
        }

        public string GetSoundPath(string soundName)
        {
            return _sounds.ContainsKey(soundName) ? Path.GetFileName(_sounds[soundName]) : null;
        }
    }
}
