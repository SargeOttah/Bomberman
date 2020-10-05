using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bomberman.Enemies
{
    abstract class Enemy
    {
        private string name { get; set; }
        private int damage { get; set; }

        private Sprite sprite { get; set; }

        public string getName() { return this.name; }
        public void setName(string name) { this.name = name;  }

        public int getDamage() { return this.damage; }

        public void setDamage(int damage) { this.damage = damage; }

        public Sprite getSprite () { return this.sprite; }
        public void setSprite(Sprite sprite) { this.sprite = sprite; }

        public void Position(int x, int y)
        {
            this.sprite.Position = new Vector2f(x, y);
        }

        public void Scale(float x, float y)
        {
            this.sprite.Scale = new Vector2f(x, x);
        }

        //public abstract void attack();
        //public abstract void move();
    }
}
