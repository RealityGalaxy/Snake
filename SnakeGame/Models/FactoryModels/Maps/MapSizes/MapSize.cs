namespace SnakeGame.Models.FactoryModels.Maps.MapSizes
{
    public class MapSize
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public MapSize(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }
}
