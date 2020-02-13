using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace MonoGameWindowsStarter
{
    enum State
    {
        Idle = 0,
        Shooting = 1,
    }

    public class Airplane
    {
        Game1 game;

        public BoundingRectangle Bounds;

        KeyboardState oldKeyboardState;
        KeyboardState newKeyboardState;

        Texture2D texture;
        SoundEffect shootSFX;
        const int ANIMATION_FRAME_RATE = 124;
        const float PLAYER_SPEED = 1000;
        const int FRAME_WIDTH = 91;
        const int FRAME_HEIGHT = 89;
        int frame;
        Vector2 position;
        State state;
        TimeSpan timer;

        public Airplane(Game1 game)
        {
            this.game = game;
            position = new Vector2(1042 / 2, 768 - 89);
            state = State.Idle;
            timer = new TimeSpan(0);
        }

        
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("air1");
            shootSFX = content.Load<SoundEffect>("shooting");
        }

        public void Update(GameTime gameTime)
        {
            newKeyboardState = Keyboard.GetState();
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            
            // Move the air left if the up key is pressed
            if (newKeyboardState.IsKeyDown(Keys.Left))
            {
                // move left
                position.X -= delta * PLAYER_SPEED;
            }

            // Move the air right if the down key is pressed
            if (newKeyboardState.IsKeyDown(Keys.Right))
            {
                // move right
                position.X += delta * PLAYER_SPEED;
            }

            // Stop the paddle from going off-screen
            if (position.X < 0)
            {
                position.X = 0;
            }
            if (position.X > game.GraphicsDevice.Viewport.Width - 100)
            {
                position.X = game.GraphicsDevice.Viewport.Width - 100;
            }

            //Fire
            if (newKeyboardState.IsKeyDown(Keys.Space) && oldKeyboardState.IsKeyUp(Keys.Space))
            {
                Bullet newBullet = new Bullet(game);
                newBullet.LoadContent(game.Content);
                newBullet.Bounds.X = position.X + 47;
                newBullet.Bounds.Y = position.Y - 3;
                newBullet.Bounds.Radius = 5;
                game.bullets.Add(newBullet);
                shootSFX.Play();
                state = State.Shooting;
                frame %= 2;
            }
            if (state != State.Idle) timer += gameTime.ElapsedGameTime;
            while (timer.TotalMilliseconds > ANIMATION_FRAME_RATE)
            {
                 state = State.Idle;
                 timer -= new TimeSpan(0, 0, 0, 0, ANIMATION_FRAME_RATE);  
            }
            
            oldKeyboardState = newKeyboardState;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var source = new Rectangle(frame * FRAME_WIDTH, (int)state % 2 * FRAME_HEIGHT, FRAME_WIDTH, FRAME_HEIGHT);
            spriteBatch.Draw(texture, position,source, Color.CornflowerBlue);
        }

        public bool IsDestroy(BoundingCircle c)
        {
            var closestX = Math.Max(Math.Min(c.X, position.X + FRAME_WIDTH), position.X);
            var closestY = Math.Max(Math.Min(c.Y, position.Y + FRAME_HEIGHT), position.Y);
            return (Math.Pow(c.Radius, 2) >= Math.Pow(closestX - c.X, 2) + Math.Pow(closestY - c.Y, 2));
        }

        public bool IsCrash(BoundingRectangle b)
        {
            return !(position.X > b.X + b.Width
                  || position.X + FRAME_WIDTH < b.X
                  || position.Y > b.Y + b.Height
                  || position.Y + FRAME_HEIGHT < b.Y);
        }
    }
}
