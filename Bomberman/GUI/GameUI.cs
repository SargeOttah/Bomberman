using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Bomberman.GUI
{
    class GameUI
    {
        // players | should be pointers?
        public Player mainPlayer { get; set; }
        public List<Player> otherPlayers { get; set; }

        RenderWindow renderWindow { get; set; }

        //public Dev devUI { get; set; }
        public GameScore scoreBoard { get; set; }
        public GameScore scoreUI { get; set; }


        public GameUI() { }
        public GameUI(RenderWindow renderWindow, Player mainPlayer, List<Player> otherPlayers)
        {
            this.mainPlayer = mainPlayer;
            this.otherPlayers = otherPlayers;
            this.renderWindow = renderWindow;
        }
    }
}
