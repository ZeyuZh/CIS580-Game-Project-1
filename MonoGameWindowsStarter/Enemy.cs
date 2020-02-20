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
    public class Enemy: IBoundable
    {
        Game1 game;

       BoundingRectangle bounds;

        Texture2D texture;

        TimeSpan timer = new TimeSpan(0);

        SpriteSheet bullets_sheet;

        public BoundingRectangle Bounds => bounds;

        public Enemy(Game1 game)
        {
            this.game = game;
        }

        public void Initialize()
        {
            bounds.Width = 100;
            bounds.Height = 90;
            bounds.X = 100 * game.Random.Next(10);
            bounds.Y = 0;
        }

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("airplane2");
            var bullet_t = content.Load<Texture2D>("bullets_1");
            bullets_sheet = new SpriteSheet(bullet_t, 28, 30, 0, 2, 2);
        }

        public void Update(GameTime gameTime)
        {
            
           /* bounds.Y += (int)gameTime.TotalGameTime.TotalMinutes + 3;
            timer += gameTime.ElapsedGameTime;

            if(timer.TotalSeconds > game.Random.Next(1, 5))
            {
                
                game.EBullets.Add(new EnemyBullet(game, new BoundingRectangle(Bounds.X + 35, Bounds.Y + Bounds.Height + 3, 15, 30), bullets_sheet[33]));
                
                timer = new TimeSpan(0);
            }*/
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
