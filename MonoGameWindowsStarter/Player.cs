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
        Left,
        Right,
        Up,
        Down,
    }

    public class Player
    {
        Game1 game;

        //public BoundingRectangle Bounds;

        KeyboardState oldKeyboardState;
        KeyboardState newKeyboardState;

        Texture2D texture;
        SoundEffect shootSFX;
        const int ANIMATION_FRAME_RATE = 125;
        const float PLAYER_SPEED = 1000;

        Sprite[] frames;
        int currentFrame = 0;


        const int FRAME_WIDTH = 50;
        const int FRAME_HEIGHT = 73;
        
        public Vector2 position = new Vector2(521,679);

        State state = State.Idle;
        TimeSpan timer;

        SpriteEffects spriteEffects = SpriteEffects.None;
        Color color = Color.White;

        SpriteSheet bullets_sheet;

        Vector2 origin = new Vector2(25, 36);

        public BoundingRectangle Bounds => new BoundingRectangle(position - origin, FRAME_WIDTH, FRAME_HEIGHT);

        public Player(Game1 game, IEnumerable<Sprite> frames)
        {
            this.game = game;

            this.frames = frames.ToArray();
            state = State.Left;
        }

        
        public void LoadContent(ContentManager content)
        {
            //texture = content.Load<Texture2D>("Reimu_1");
            //shootSFX = content.Load<SoundEffect>("shooting");
            var bullet_t = content.Load<Texture2D>("bullets_1");
            bullets_sheet = new SpriteSheet(bullet_t, 28, 30, 0, 3, 2);
        }

        public void Update(GameTime gameTime, float scrollDistance)
        {
            newKeyboardState = Keyboard.GetState();
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            
            // Move the air left if the up key is pressed
            if (newKeyboardState.IsKeyDown(Keys.Left))
            {
                // move left
                position.X -= delta * PLAYER_SPEED;
                state = State.Left;
            }
            else if (newKeyboardState.IsKeyDown(Keys.Right))
            {
                // move right
                position.X += delta * PLAYER_SPEED;
                state = State.Right;
            }
            else if (newKeyboardState.IsKeyDown(Keys.Up))
            {
                // move right
                position.Y -= delta * PLAYER_SPEED;
                state = State.Up;
            }
            else if (newKeyboardState.IsKeyDown(Keys.Down))
            {
                // move right
                position.Y += delta * PLAYER_SPEED;
                state = State.Down;
            }
            else
            {
                state = State.Idle;
            }

            switch (state)
            {
                case State.Idle:
                    currentFrame = 0;
                    timer = new TimeSpan(0);
                    break;
                case State.Left:
                    timer += gameTime.ElapsedGameTime;
                    spriteEffects = SpriteEffects.None;
                    if(timer.TotalMilliseconds > ANIMATION_FRAME_RATE * 2)
                    {
                        timer = new TimeSpan(0);
                    }
                    currentFrame = (int)Math.Floor(timer.TotalMilliseconds / ANIMATION_FRAME_RATE) + 11;
                    break;
                case State.Right:
                    timer += gameTime.ElapsedGameTime;
                    spriteEffects = SpriteEffects.FlipHorizontally;
                    if (timer.TotalMilliseconds > ANIMATION_FRAME_RATE * 2)
                    {
                        timer = new TimeSpan(0);
                    }
                    currentFrame = (int)Math.Floor(timer.TotalMilliseconds / ANIMATION_FRAME_RATE) + 11;
                    break;
                case State.Up:
                case State.Down:
                    timer += gameTime.ElapsedGameTime;
                    spriteEffects = SpriteEffects.None;
                    if (timer.TotalMilliseconds > ANIMATION_FRAME_RATE * 3)
                    {
                        timer = new TimeSpan(0);
                    }
                    currentFrame = (int)Math.Floor(timer.TotalMilliseconds / ANIMATION_FRAME_RATE) + 3;
                    break;
            }

            // Stop the paddle from going off-screen
            if (position.X < 3)
            {
                position.X = 3;
            }
            else if (position.X > game.GraphicsDevice.Viewport.Width - 50)
            {
                position.X = game.GraphicsDevice.Viewport.Width - 50;
            }
            else if (position.Y > game.GraphicsDevice.Viewport.Height - 70 + scrollDistance)
            {
                position.Y = game.GraphicsDevice.Viewport.Height - 70 + scrollDistance;
            }
            else if(position.Y < scrollDistance)
            {
                position.Y = scrollDistance;
            }

            //Fire
            if (newKeyboardState.IsKeyDown(Keys.Space) && oldKeyboardState.IsKeyUp(Keys.Space))
            {
               
                game.bullets.Add(new Bullet(game,new BoundingRectangle(Bounds.X, Bounds.Y-3,15,30),bullets_sheet[25]));
                //shootSFX.Play();
                
            }
            
            
            oldKeyboardState = newKeyboardState;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            frames[currentFrame].Draw(spriteBatch, position, color, 0, origin, 2, spriteEffects, 1);
        }
        
    }
}
