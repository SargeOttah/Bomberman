using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;
using Bomberman.GUI.Visitor;
using Bomberman.GUI;

namespace Bomberman.Spawnables.Enemies
{
    public abstract class Enemy
    {
        private string name { get; set; }
        private int damage { get; set; }

        public Sprite sprite { get; set; }

        public string getName() { return this.name; }
        public void setName(string name) { this.name = name;  }

        public int getDamage() { return this.damage; }

        public void setDamage(int damage) { this.damage = damage; }

        public Sprite getSprite () { return this.sprite; }
        public void setSprite(Sprite sprite) { this.sprite = sprite; }

        public void Position(float x, float y)
        {
            this.sprite.Position = new Vector2f(x, y);
        }

        public void Scale(float x, float y)
        {
            this.sprite.Scale = new Vector2f(x, x);
        }


        // Enemy type prototype method
        public abstract Enemy Clone();
    }
}
