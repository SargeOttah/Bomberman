using System;
using System.Collections.Generic;
using System.Text;

namespace Bomberman.GUI.Logger
{
    public class Message
    {
        private string message;
        int level;
        public Message(int level, string message)
        {
            this.level = level;
            this.message = message;
        }

        public int getLevel() { return this.level; }
        public string getMessage() { return this.message; }
    }
}
