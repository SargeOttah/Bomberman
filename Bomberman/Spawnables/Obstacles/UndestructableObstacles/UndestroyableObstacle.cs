﻿using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;

namespace Bomberman.Spawnables.Obstacles.UndestructableObstacles
{
    abstract class UndestroyableObstacle : Obstacle
    {
        public abstract Sprite SpawnObstacle();
    }
}
