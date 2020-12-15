using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;
using SFML.System;
using Bomberman.GUI.Visitor;

namespace Bomberman.GUI
{
    public class DebugGUI : Drawable
    {
        public Text debugText = new Text("", new Font(Properties.Resources.arial), 20);

        public PlayerVisitor pVisitor = new PlayerVisitor();

        public DebugGUI(RenderWindow _renderWindow)
        {
            debugText.Position = new Vector2f(10, (_renderWindow.Size.Y - 30));
            debugText.Style = Text.Styles.Bold;
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            debugText.DisplayedString = pVisitor.getData();
            target.Draw(debugText);
        }

    }
}
