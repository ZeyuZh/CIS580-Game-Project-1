using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace MonoGameWindowsStarter
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Airplane air;
        List<Enemy> enemies = new List<Enemy>();
        public List<Bullet> bullets = new List<Bullet>();
        public List<EnemyBullet> EBullets = new List<EnemyBullet>();
        float timer = 2000;
        const float TIMER = 2000;
        bool lose = false;
        Lose loseRect;
        public Random Random = new Random();


        KeyboardState oldKeyboardState;
        KeyboardState newKeyboardState;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            air = new Airplane(this);
            for (int i = 0; i < 2; i++)
            {
                enemies.Add(new Enemy(this));
            }
            loseRect = new Lose(this);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferWidth = 1042;
            graphics.PreferredBackBufferHeight = 768;
            graphics.ApplyChanges();
            
            foreach(Bullet bullet in bullets)
            {
                bullet.Bounds.X = air.Bounds.X;
                bullet.Bounds.Y = air.Bounds.Y - 3;
                bullet.Bounds.Radius = 5;
            }
            air.Initialize();
            foreach(Enemy e in enemies)
            {
                e.Initialize();
            }
            
            loseRect.Initialize();
            

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            
            air.LoadContent(Content);
            foreach (Enemy e in enemies)
            {
                e.LoadContent(Content);
            }
            
            loseRect.LoadContent(Content);
            
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            
            newKeyboardState = Keyboard.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (newKeyboardState.IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            if (!lose)
            {
                
                //remove collised bullet and enemies
                for (int i = 0; i < bullets.Count; i++)
                {
                    for (int j = 0; j < enemies.Count; j++)
                    {
                        if (!bullets[i].IsExist(enemies[j].Bounds))
                        {
                            enemies.RemoveAt(j);
                            bullets.RemoveAt(i);
                            i--;
                            return;
                        }

                    }
                }

                //remove the bullet out of bounds
                for (int i = 0; i < bullets.Count; i++)
                {
                    if (!bullets[i].IsVisible())
                    {
                        bullets.RemoveAt(i);
                        i--;
                    }

                }
                for (int i = 0; i < EBullets.Count; i++)
                {
                    if (!EBullets[i].IsVisible())
                    {
                        EBullets.RemoveAt(i);
                        i--;
                    }

                }
                
                //add enemies
                float elapsed = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                timer -= elapsed;

                if (timer < 0 && enemies.Count < 10)
                {
                    Enemy newEnemy = new Enemy(this);
                    newEnemy.LoadContent(Content);
                    newEnemy.Initialize();
                    enemies.Add(newEnemy);
                    timer = TIMER;
                }

                 
                
                 //check for lose
                foreach (Enemy e in enemies)
                {
                    e.Update(gameTime);
                    if (e.IsLose())
                    {
                        lose = true;
                    }
                    if (e.Bounds.CollidesWith(air.Bounds))
                        lose = true;
                    
                }
                foreach(EnemyBullet ebs in EBullets)
                {
                    if (!ebs.IsExist(air.Bounds))
                    {
                        lose = true;
                    }
                }
                
               
                //update
                foreach (EnemyBullet ebs in EBullets)
                {
                    ebs.Update(gameTime);
                }
                foreach (Bullet b in bullets)
                {
                    b.Update(gameTime);
                }
                air.Update(gameTime);
                base.Update(gameTime);
            }
            
            oldKeyboardState = newKeyboardState;
        }

        
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            // TODO: Add your drawing code here
            spriteBatch.Begin();

            //draw bullets
            foreach (Bullet b in bullets)
            {
                b.Draw(spriteBatch);
            }
            
            //draw airplane
            air.Draw(spriteBatch);

            //draw enemy bullets
            foreach (EnemyBullet eb in EBullets)
            {
                eb.Draw(spriteBatch);
            }

            //draw enemies
            foreach (Enemy e in enemies)
            {
                e.Draw(spriteBatch);
            }

            //draw lose picture
            if (lose)
            {
                loseRect.Draw(spriteBatch);
            }

            spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}
