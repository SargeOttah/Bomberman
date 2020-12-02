using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System.Drawing;

namespace Bomberman.GUI
{
    class GameScore : Drawable
    {
        public Text scoreText = new Text("", new Font(Properties.Resources.arial), 30);
        public List<Tuple<string, int>> score; // id | score 


        public GameScore(RenderWindow _renderWindow, List<Player> players, string mainId)
        {
            scoreText.Position = new Vector2f((_renderWindow.Size.X / 2) - 50, 10);
            scoreText.Style = Text.Styles.Bold;
            score = new List<Tuple<string, int>>();
            score.Add(Tuple.Create((string)mainId, (int)0));

            players.ForEach(x=> score.Add(Tuple.Create((string)x.connectionId, (int)0)));

            //score.Add(Tuple.Create((string)"P1", (int)0));
            //score.Add(Tuple.Create((string)"P2", (int)0));
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            string pattern = "";
            score.ForEach(x=> pattern += $"{x.Item2} - ");
            scoreText.DisplayedString = pattern;
            target.Draw(scoreText);
        }

        public void UpdateScore(string id)
        {
            for (int i = 0; i < score.Count; i++)
            {
                if (score[i].Item1 == id)
                {
                    score[i] = Tuple.Create(id, score[i].Item2 + 10);
                }
            }
            //if (id == "P1")
            //{
            //    score[1] = Tuple.Create(id, score[1].Item2 + 1);
            //}
            //else
            //{
            //    score[0] = Tuple.Create(id, score[0].Item2 + 1);
            //}
        }

        public Text getScoreText(PointF Pos)
        {
            scoreText.DisplayedString = $"0 - 0";
            return scoreText;
        }


    }
}
