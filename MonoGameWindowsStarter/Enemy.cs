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

        TimeSpan timer = new TimeSpan(0);

        SpriteSheet bullets_sheet;

        Sprite sprite;

        public BoundingRectangle Bounds => bounds;

        public Enemy(Game1 game, BoundingRectangle bounds, Sprite sprite)
        {
            
            this.game = game;
            this.bounds = bounds;
            this.sprite = sprite;
        }

        

        public void LoadContent(ContentManager content)
        {
            
            var bullet_t = content.Load<Texture2D>("bullets_1");
            bullets_sheet = new SpriteSheet(bullet_t, 28, 30, 0, 3, 2);
        }

        public void Update(GameTime gameTime)
        {
            
            bounds.Y += (int)gameTime.TotalGameTime.TotalMinutes;
            timer += gameTime.ElapsedGameTime;

            if(timer.TotalSeconds > game.Random.Next(1, 5))
            {
                int b = game.Random.Next(33, 38);
                game.EBullets.Add(new EnemyBullet(game, new BoundingRectangle(Bounds.X + 22, Bounds.Y + Bounds.Height + 3, 15, 30), bullets_sheet[b]));
                
                timer = new TimeSpan(0);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch, new Vector2(Bounds.X, Bounds.Y), Color.White);
        }

        

        public bool IsLose(float scrollDistance)
        {
            if (Bounds.Y > game.GraphicsDevice.Viewport.Height + scrollDistance)
                return true;
            else
                return false;
        }

        
    }
}
