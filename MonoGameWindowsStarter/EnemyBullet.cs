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

        Texture2D texture;

        public BoundingCircle Bounds;

        const float TIMER = 1000;

        public EnemyBullet(Game1 game)
        {
            this.game = game;
        }

        

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("circle");
        }

        public void Update(GameTime gameTime)
        {
            
            var viewport = game.GraphicsDevice.Viewport;
            var keyboardState = Keyboard.GetState();
            Bounds.Y += 10;

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Bounds, Color.Green);
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

