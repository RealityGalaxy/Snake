﻿using SnakeGame.Builders;
using SnakeGame.Models.FactoryModels;
using SnakeGame.Models.FactoryModels.Fruit;
using SnakeGame.Services;

namespace SnakeGame.Factories
{
    public class Level3Factory : ILevelFactory
    {
        public Obstacle generateObstacle()
        {
            Random rand = new Random();
            int next = rand.Next(3);
            switch (next)
            {
                case 0:
                    return new Tunnel();
                case 1:
                    return new Rock();
                case 2:
                    return new Room();
            }
            return new Rock();
        }

        public Consumable generateConsumable(GameInstance instance)
        {
            ConsumableBuilder builder = new(instance);
            Random foodRand = new Random();
            int roll = foodRand.Next(0, 10);
            int poisonRoll = foodRand.Next(0, 10);
            if (roll >= 9)
            {
                builder.SetType(typeof(RainbowFruit));
            }
            else if (roll >= 7)
            {
                builder.SetType(typeof(BigApple));
            }
            else if (roll >= 4)
            {
                builder.SetType(typeof(Watermelon));
            }
            else
            {
                builder.SetType(typeof(Lemon));
            }
            if (poisonRoll >= 9)
            {
                builder.SetPoison(true);
            }
            return builder.Build();
        }

        public Map generateMap(GameInstance instance)
        {
            return new Level3Map(instance);
        }
    }
}
