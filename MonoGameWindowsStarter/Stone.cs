using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace MonoGameWindowsStarter
{
    public class Stone
    {
        Game1 game;
        public BoundingRectangle Bounds;

        Sprite sprite;

        public Stone(Game1 game, BoundingRectangle bounds, Sprite sprite)
        {
            this.game = game;

            this.Bounds = bounds;
            this.sprite = sprite;

        }

        public void Update(GameTime gameTime)
        {
            Bounds.Y += 3;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch, new Vector2(Bounds.X, Bounds.Y), Color.White);
        }

        public bool IsVisible(float scrollDistance)
        {
            if (Bounds.Y > game.GraphicsDevice.Viewport.Height + scrollDistance)
                return false;
            else
                return true;
        }

        public bool IsCrash(BoundingRectangle player)
        {
            if (Bounds.CollidesWith(player))
                return true;
            else return false;
        }
    }
}
