using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;


namespace MonoGameWindowsStarter
{
    public class Lose
    {
        Game1 game;

        public BoundingRectangle Bounds;

        Texture2D texture;
        public Lose(Game1 game)
        {
            this.game = game;
        }

        public void Initialize()
        {
            Bounds.Width = 1042;
            Bounds.Height = 768;
            Bounds.X = 0;
            Bounds.Y = 0;
        }

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("lose");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Bounds, Color.White);
        }
    }
}
