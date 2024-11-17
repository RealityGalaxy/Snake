namespace SnakeGame.Models.FactoryModels
{
    public class SmallRock : Obstacle
    {
        public SmallRock()
        {
            Points = new int[2, 2]
            {
                { 1, 1},
                { 1, 1},
            };
        }

        public override Obstacle Clone()
        {
            var clonedRock = new SmallRock
            {
                Points = (int[,])Points.Clone() // Deep copy of the Points array
            };
            return clonedRock;
        }
    }
}
