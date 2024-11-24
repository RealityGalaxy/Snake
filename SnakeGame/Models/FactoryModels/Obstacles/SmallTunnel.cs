﻿namespace SnakeGame.Models.FactoryModels
{
    public class SmallTunnel : Obstacle
    {
        public SmallTunnel()
        {
            Random rand = new Random();
            if(rand.Next(2) == 0)
            {
                Points = new int [4, 4]
                {
                    { 1, 0, 0, 1},
                    { 1, 0, 0, 1},
                    { 1, 0, 0, 1},
                    { 1, 0, 0, 1},
                };
            }
            else
            {
                Points = new int[4, 4]
                {
                    { 1, 1, 1, 1},
                    { 0, 0, 0, 0},
                    { 0, 0, 0, 0},
                    { 1, 1, 1, 1},
                };
            }
        }

        public override Obstacle Clone()
        {
            var clonedTunnel = new SmallTunnel
            {
                Points = (int[,])Points.Clone() // Deep copy of the Points array
            };
            return clonedTunnel;
        }
    }
}
