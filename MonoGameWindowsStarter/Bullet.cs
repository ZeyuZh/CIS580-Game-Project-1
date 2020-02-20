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
        public BoundingRectangle Bounds;

        Sprite sprite;


        public Bullet(Game1 game, BoundingRectangle bounds, Sprite sprite)
        {
            this.game = game;
            
            this.Bounds = bounds;
            this.sprite = sprite;
            
        }


        
        public void Update(GameTime gameTime)
        {
            
            Bounds.Y -= 10;
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch, new Vector2(Bounds.X, Bounds.Y), Color.White);
        }

        public bool IsExist(IEnumerable<IBoundable> enemyQuery)
        {
            
            foreach (Enemy enemy in enemyQuery)
            {
                if (Bounds.CollidesWith(enemy.Bounds))
                {
                    game.enemies.Remove(enemy);
                    return false;
                }
                
            }

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
