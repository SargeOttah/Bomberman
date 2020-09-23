﻿using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;
using SFML.System;
using System.Reflection;
using System.Threading.Tasks;

using Bomberman.Collisions;
//using Bomberman.Objects;
//using Bomberman.Utilities;

namespace Bomberman
{
    class Player : Sprite
    {
        public float Health { get; private set; } = 100;
        public Vector2f Speed { get; set; } = new Vector2f(0.0f, 0.0f);
        public bool IsDead { get; private set; } = false;
        public float SpeedMultiplier { get; private set; } = 1;

        public Player() { }

        public void Translate(float xOffset, float yOffset)
        {
            this.Position = new Vector2f(this.Position.X + xOffset * SpeedMultiplier, this.Position.Y + yOffset * SpeedMultiplier);
        }
        public bool CheckMovementCollision(float xOffset, float yOffset, Sprite targetCollider)
        {
            Translate(xOffset, yOffset);
            if (CollisionTester.BoundingBoxTest(this, targetCollider))
            {
                Translate(-xOffset, -yOffset);
                return true;
            }
            else
            {
                Translate(-xOffset, -yOffset);
                return false;
            }
        }
        public void IncreaseMovementSpeed(float multiplier, float durationInMilis)
        {
            SpeedMultiplier = 2;
            Task.Delay((int)durationInMilis).ContinueWith(o => SpeedMultiplier = 1);
        }

    }
}