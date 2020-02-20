using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace MonoGameWindowsStarter
{
    public class EnemyBullet
    {
        Game1 game;
        public BoundingRectangle Bounds;

        Sprite sprite;


        public EnemyBullet(Game1 game, BoundingRectangle bounds, Sprite sprite)
        {
            this.game = game;
            this.Bounds = bounds;
            this.sprite = sprite;
            
        }

        

        /*public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("circle");
        }*/

        public void Update(GameTime gameTime)
        {
            
            Bounds.Y += 10;

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch, new Vector2(Bounds.X, Bounds.Y), Color.White);
        }

        public bool IsExist(BoundingRectangle r)
        {
            if (Bounds.CollidesWith(r))
                return false;
            else
                return true;
        }

        public bool IsVisible()
        {
            if (Bounds.Y > game.GraphicsDevice.Viewport.Height)
                return false;
            else
                return true;
        }
    }
}

