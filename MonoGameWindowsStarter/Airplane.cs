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
    public class Airplane
    {
        Game1 game;

        public BoundingRectangle Bounds;

       

        Texture2D texture;

        public Airplane(Game1 game)
        {
            this.game = game;
        }

        public void Initialize()
        {
            Bounds.Width = 95;
            Bounds.Height = 90;
            Bounds.X = game.GraphicsDevice.Viewport.Width / 2;
            Bounds.Y = game.GraphicsDevice.Viewport.Height - Bounds.Height;
        }

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("airplane");
        }

        public void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();

            // Move the air left if the up key is pressed
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                // move left
                Bounds.X -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            }

            // Move the air right if the down key is pressed
            if (keyboardState.IsKeyDown(Keys.Right))
            {
                // move right
                Bounds.X += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            }

            // Stop the paddle from going off-screen
            if (Bounds.X < 0)
            {
                Bounds.X = 0;
            }
            if (Bounds.X > game.GraphicsDevice.Viewport.Width - Bounds.Width)
            {
                Bounds.X = game.GraphicsDevice.Viewport.Width - Bounds.Width;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Bounds, Color.Green);
        }
    }
}
