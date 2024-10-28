using SnakeGame.Models.FactoryModels.Fruit.Attributes;
using SnakeGame.Services;
using System.Drawing;
using System.Threading;

namespace SnakeGame.Models.FactoryModels.Fruit
{
    public class RainbowFruit : Consumable
    {
        private bool _isAlive = true;
        private CancellationTokenSource _cancellationTokenSource;

        public RainbowFruit(GameInstance instance, FruitAttributes attributes)
        {
            Instance = instance;
            Attributes = attributes;
            GenerateNewPosition();
            StartRainbowing();
        }

        public async void StartRainbowing()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = _cancellationTokenSource.Token;

            // Define a list of colors for the rainbow effect
            string[] rainbowColors = new[] { "#FF0000", "#FF7F00", "#FFFF00", "#00FF00", "#0000FF", "#4B0082", "#9400D3" };
            int colorIndex = 0;

            try
            {
                // Run the color-changing loop while the fruit is alive
                while (_isAlive)
                {
                    // Change the fruit's color
                    Attributes.Color = rainbowColors[colorIndex];

                    // Cycle through the colors
                    colorIndex = (colorIndex + 1) % rainbowColors.Length;

                    // Wait for a short delay before changing the color again (e.g., 300 ms)
                    await Task.Delay(50, token);
                }
            }
            catch (TaskCanceledException)
            {
                // Handle the task cancellation if necessary
            }
        }

        // Method to stop the rainbow process (when the fruit is consumed or removed)
        public void StopRainbowing()
        {
            _isAlive = false;
            _cancellationTokenSource?.Cancel(); // Stop the async loop
        }

        public override bool CanConsume()
        {
            throw new NotImplementedException();
        }

        public override int Consume()
        {
            StopRainbowing ();
            return Attributes.Value;
        }

        public override Consumable Clone()
        {
            Consumable clone = new RainbowFruit(Instance, Attributes)
            {
                IsPoisonous = this.IsPoisonous,
                IsDynamic = this.IsDynamic,
                IsBigConsumable = this.IsBigConsumable
            };
            return clone;
        }
    }
}
