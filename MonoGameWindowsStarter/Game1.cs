using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using System.Linq;

namespace MonoGameWindowsStarter
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public Player player;
        public List<Enemy> enemies = new List<Enemy>();
        public List<Enemy> killedEnemies = new List<Enemy>();
        public List<Bullet> bullets = new List<Bullet>();
        public List<EnemyBullet> EBullets = new List<EnemyBullet>();
        float timer = 2000;
        const float TIMER = 2000;
        bool lose = false;
        Lose loseRect;
        public Random Random = new Random();
        SoundEffect expSFX;
        SpriteFont scoreFont;
        public int score = 0;

        public List<Stone> stones = new List<Stone>();
        SpriteSheet stone_sheet;

        SpriteSheet sheet;
        SpriteSheet enemies_sheet;

        AxisList env;

        KeyboardState oldKeyboardState;
        KeyboardState newKeyboardState;

        Vector2 offset;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            
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
            
            
            //air.Initialize();
            /*foreach(Enemy e in enemies)
            {
                e.Initialize();
            }*/
            
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
            expSFX = Content.Load<SoundEffect>("explosion");
            scoreFont = Content.Load<SpriteFont>("score");
            
            
            //load player
            var t = Content.Load<Texture2D>("Reimu_1");
            sheet = new SpriteSheet(t, 50, 73, 0, 0,2);

            //load stone
            var t_s = Content.Load<Texture2D>("stones");
            stone_sheet = new SpriteSheet(t_s, 42, 40, 1, 2, 0);
            for(int i = 0; i < 10; i++)
            {
                stones.Add(new Stone(this, new BoundingRectangle(new Vector2(Random.Next(10) * 100, -Random.Next(100) * 1000), 42, 40),stone_sheet[Random.Next(0,1)]));
            }

            var playerFrames = from index in Enumerable.Range(0, 23) select sheet[index];
            player = new Player(this, playerFrames);
            player.LoadContent(Content);


            var t_e = Content.Load<Texture2D>("Enemies_1");
            enemies_sheet = new SpriteSheet(t_e, 60, 39, 2, 2, 24);
            for(int i = 0; i < 2; i++)
            {
                enemies.Add(new Enemy(this, new BoundingRectangle(new Vector2(Random.Next(10) * 100, -10), 60, 39), enemies_sheet[Random.Next(0,15)]));
            }

            env = new AxisList();
            foreach (Enemy e in enemies)
            {
                e.LoadContent(Content);
                env.AddGameObject(e);
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
            offset = new Vector2(0, 0) + new Vector2(0, (float)gameTime.TotalGameTime.TotalMilliseconds / 50);
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
                    var enemyQuery = env.QueryRange(0, 1042);
                    if (!bullets[i].IsExist(enemyQuery))
                    {
                        score++;
                        bullets.RemoveAt(i);
                        return;
                    }
                    
                }

                //remove the bullet out of bounds
                for (int i = 0; i < bullets.Count; i++)
                {
                    if (!bullets[i].IsVisible(-offset.Y))
                    {
                        bullets.RemoveAt(i);
                        i--;
                    }

                }
                for (int i = 0; i < EBullets.Count; i++)
                {
                    if (!EBullets[i].IsVisible(-offset.Y))
                    {
                        EBullets.RemoveAt(i);
                        i--;
                    }

                }

                //remove stones out of bounds
                for (int i = 0; i < stones.Count; i++)
                {
                    if (!stones[i].IsVisible(-offset.Y))
                    {
                        stones.RemoveAt(i);
                        i--;
                    }

                }

                //add enemies
                float elapsed = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                timer -= elapsed;

                if (timer < 0 && enemies.Count < 10)
                {
                    Enemy newEnemy = new Enemy(this, new BoundingRectangle(new Vector2(Random.Next(10) * 100, -10 - offset.Y), 60, 39), enemies_sheet[Random.Next(0,15)]);
                    newEnemy.LoadContent(Content);
                    enemies.Add(newEnemy);
                    env.AddGameObject(newEnemy);
                    timer = TIMER;
                }

                 
                
                 //check for lose
                foreach (Enemy e in enemies)
                {
                    e.Update(gameTime);
                    if (e.IsLose(-offset.Y))
                    {
                        lose = true;
                    }
                    if (player.Bounds.CollidesWith(e.Bounds))
                        lose = true;
                    
                }
                foreach(EnemyBullet ebs in EBullets)
                {
                    if (player.Bounds.CollidesWith(ebs.Bounds))
                    {
                        lose = true;
                    }
                }
                
                foreach(Stone s in stones)
                {
                    s.Update(gameTime);
                    if (s.IsCrash(player.Bounds))
                        lose = true;
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

                
                    
                player.Update(gameTime, -offset.Y);
                base.Update(gameTime);

                // Remove killed enemies 
                foreach (Enemy enemy in killedEnemies)
                {
                    enemies.Remove(enemy);
                    env.RemoveGameObject(enemy);
                }
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

            var t = Matrix.CreateTranslation(offset.X, offset.Y, 0);
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, t);
            
            //draw bullets
            foreach (Bullet b in bullets)
            {
                b.Draw(spriteBatch);
            }
            
            //draw player
            player.Draw(spriteBatch);

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

            foreach(Stone s in stones)
            {
                s.Draw(spriteBatch);
            }

            
            spriteBatch.End();

            spriteBatch.Begin();

            //draw lose picture
            if (lose)
            {
                loseRect.Draw(spriteBatch);
            }
            string status = "Score: " + score + "\nEnemies: " + enemies.Count;
            spriteBatch.DrawString(scoreFont, status, new Vector2(956, 10), Color.Black);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
