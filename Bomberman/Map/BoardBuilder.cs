﻿using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;
using SFML.Window;
using Bomberman.Spawnables.Obstacles;
using Bomberman.Spawnables.Enemies;
using SFML.System;

namespace Bomberman.Map
{
    class BoardBuilder
    {
        EnemyFactory enemyFactory = new EnemyFactory();
        public List<Enemy> _enemies = new List<Enemy>();
        public float spriteScale = 0.2f;
        private Enemy enemyZombie { get; }
        private Enemy enemyGhost { get; }
        private Enemy enemySkeleton { get; }
        public BoardBuilder()
        {
            enemyZombie     = enemyFactory.createEnemy("Zombie");
            enemyGhost      = enemyFactory.createEnemy("Ghost");
            enemySkeleton   = enemyFactory.createEnemy("Skeleton");
        }
        public void AddZombie(Vector2f pos, Vector2f scale)
        {
            var zombieEnemy = enemyZombie.Clone();
            zombieEnemy.Position(pos.X, pos.Y);
            zombieEnemy.Scale(scale.X, scale.Y);
            _enemies.Add(zombieEnemy);
        }
        public void AddGhost(Vector2f pos, Vector2f scale)
        {
            var ghostEnemy = enemyGhost.Clone();
            ghostEnemy.Position(pos.X, pos.Y);
            ghostEnemy.Scale(scale.X, scale.Y);
            _enemies.Add(ghostEnemy);
        }
        public void AddSkeleton(Vector2f pos, Vector2f scale)
        {
            var skeletonEnemy = enemySkeleton.Clone();
            skeletonEnemy.Position(pos.X, pos.Y);
            skeletonEnemy.Scale(scale.X, scale.Y);
            _enemies.Add(skeletonEnemy);
        }
    }
}
