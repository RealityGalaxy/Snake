namespace SnakeGame.Models.FactoryModels
{
    public class Rock : Obstacle
    {
        public Rock()
        {
            Points = new int[4, 4]
            {
                { 0, 1, 1, 0},
                { 1, 1, 1, 1},
                { 1, 1, 1, 1},
                { 0, 1, 1, 0},
            };
        }

        public override Obstacle Clone()
        {
            var clonedRock = new Rock
            {
                Points = (int[,])Points.Clone() // Deep copy of the Points array
            };
            return clonedRock;
        }
    }
}
