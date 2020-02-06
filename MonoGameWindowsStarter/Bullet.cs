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
    public class Bullet
    {
        Game1 game;

        Texture2D texture;

        public BoundingCircle Bounds;


        

        public Bullet(Game1 game)
        {
            this.game = game;
        }

        /*public void Initialize()
        {
            // Set the bullet's radius
            Bounds.Radius = 5;

            // position the bullet in the center of the screen
            Bounds.X = 30;
            Bounds.Y = 720;

            
        }*/

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("circle");
        }

        public void Update(GameTime gameTime)
        {
            var viewport = game.GraphicsDevice.Viewport;
            var keyboardState = Keyboard.GetState();
            Bounds.Y -= 10;
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Bounds, Color.Black);
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
            if (Bounds.Y < 0)
                return false;
            else
                return true;
        }
    }
}
