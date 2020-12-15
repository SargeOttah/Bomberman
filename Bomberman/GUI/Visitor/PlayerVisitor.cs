using System;
using System.Collections.Generic;
using System.Text;
using Bomberman.Spawnables.Enemies;
using Bomberman.Spawnables.Weapons;

namespace Bomberman.GUI.Visitor
{
    public class PlayerVisitor : IVisitor
    {
        int bombCount = 0;
        int playerCount = 1;

        public PlayerVisitor()
        {}
    
        public void visit(Player player)
        {
            playerCount++;
        }

        public void visit(Bomb bomb)
        {
            bombCount++;
        }

        public void Add(int amount)
        {
            playerCount += amount;
        }

        public int GetAmount()
        {
            return playerCount;
        }
        public void ResetData()
        {
            playerCount = 1;
        }
        public string getData()
        {
            string report = $"Players: {playerCount}, total bombs placed: {bombCount}";
            return report;
        }

    }
}
