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
    public class Enemy
    {
        Game1 game;

        public BoundingRectangle Bounds;

        Texture2D texture;

        TimeSpan timer = new TimeSpan(0);

        public Enemy(Game1 game)
        {
            this.game = game;
        }

        public void Initialize()
        {
            Bounds.Width = 100;
            Bounds.Height = 90;
            Bounds.X = 100 * game.Random.Next(10);
            Bounds.Y = 0;
        }

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("airplane2");
        }

        public void Update(GameTime gameTime)
        {
            Bounds.Y += (int)gameTime.TotalGameTime.TotalMinutes + 3;
            timer += gameTime.ElapsedGameTime;

            if(timer.TotalSeconds > game.Random.Next(1, 5))
            {
                EnemyBullet eb = new EnemyBullet(game);
                eb.LoadContent(game.Content);
                eb.Bounds.X = Bounds.X + 50;
                eb.Bounds.Y = Bounds.Y + Bounds.Height + 3;
                eb.Bounds.Radius = 5;
                game.EBullets.Add(eb);
                timer = new TimeSpan(0);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Bounds, Color.Blue);
        }

        public bool Alive(BoundingCircle c)
        {
            if (Bounds.CollidesWith(c))
                return false;
            else
                return true;
        }

        public bool IsLose()
        {
            if (Bounds.Y > game.GraphicsDevice.Viewport.Height)
                return true;
            else
                return false;
        }

        
    }
}
