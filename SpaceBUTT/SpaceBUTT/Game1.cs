using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Threading;

namespace SpaceBUTT
{

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        //Initalition
        enum GameState
        {
            StartMenu,
            Loading,
            Playing,
            Paused,
            Tutorial
        }
        private Texture2D background;
        private Texture2D startButton;
        private Texture2D exitButton;
        private Texture2D endlessButton;
        private Texture2D pauseButton;
        private Texture2D resumeButton;
        private Texture2D loadingScreen;
        private Vector2 backgroundPosition;
        private Vector2 startButtonPosition;
        private Vector2 endlessButtonPosition;
        private Vector2 exitButtonPosition;
        private Vector2 resumeButtonPosition;
        private GameState gameState;
        private Thread backgroundThread;
        private bool isLoading = false;
        private bool isPlayerDead = false;

        MouseState mouseState;
        MouseState previousMouseState;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Spawn spawn = new Spawn();
        Collision collision = new Collision();
        Crosshair cross = new Crosshair();
        Player player = new Player();
        HUD hud = new HUD();
        Effect effect;
        Skybox skybox = new Skybox();
        Station station = new Station();
        Boss1 boss1 = new Boss1();
        

        int spawnEnemyTimer = 0;
        int spawnTimer = 0;
        int spawnTime = 10;
        int spawnEnemyTime = 50;
        int spawnedBoss = 0;

        public int killedEnemies = 0;
        bool spawnBoss2 = false;
        bool spawnBoss = false;
        bool stopSpawn = true;

        bool level0=true;
        bool level1;
        bool level2;
        bool level3;

        int zaehler;
        int i = 0;

        public Matrix View;
        public Matrix Projection;
        public GraphicsDevice device;

        Vector3 cameraPosition = new Vector3(0.0f, 1000.0f, 5000.0f);
        Vector3 cameraView = Vector3.Zero;

        float aspectRatio;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }


        protected override void Initialize()
        {
            IsMouseVisible = true;
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferredBackBufferWidth = 800;
            graphics.ApplyChanges();

            //menu.Initialize(IsMouseVisible,GraphicsDevice.Viewport.Width);

            startButtonPosition = new Vector2((GraphicsDevice.Viewport.Width / 2) - 300, 400);
            endlessButtonPosition = new Vector2((GraphicsDevice.Viewport.Width / 2) - 300, 450);
            exitButtonPosition = new Vector2((GraphicsDevice.Viewport.Width / 2) - 300, 500);
           
            backgroundPosition = new Vector2(0,0); 
            gameState = GameState.StartMenu;

            //get the mouse state
            mouseState = Mouse.GetState();
            previousMouseState = mouseState;
            base.Initialize();
        }



        protected override void LoadContent()
        {
            device = graphics.GraphicsDevice;

            spriteBatch = new SpriteBatch(GraphicsDevice);

            aspectRatio = graphics.GraphicsDevice.Viewport.AspectRatio;
            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(60.0f), aspectRatio, 1.0f, 100000.0f);
            View = Matrix.CreateLookAt(cameraPosition, cameraView, Vector3.Up);
            effect = Content.Load<Effect>("effects");
            hud.LoadContent(Content);
            player.LoadContent(Content);
            cross.LoadContent(Content);
            skybox.LoadContent(Content, effect);
            station.LoadContent(Content);
           // bomb.LoadContent(Content);
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //load the buttonimages into the content pipeline
            startButton = Content.Load<Texture2D>(@"Menü/storybutton");
            endlessButton = Content.Load<Texture2D>(@"Menü/storybutton");
            exitButton = Content.Load<Texture2D>(@"Menü/quitbutton");
            background = Content.Load<Texture2D>(@"Menü/mainmenu");
            //load the loading screen
            loadingScreen = Content.Load<Texture2D>(@"Menü/loadbackround");
        }


        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState stat = Keyboard.GetState();
            if (stat.IsKeyDown(Keys.Escape))
                this.Exit();
            if (stat.IsKeyDown(Keys.NumPad0))
            {
                level0 = true;
                level1 = false;
                level2 = false;
                level3 = false;
            }
            if (stat.IsKeyDown(Keys.NumPad1))
            {
                level0 = false;
                level1 = true;
                level2 = false;
                level3 = false;
            }
            if (stat.IsKeyDown(Keys.NumPad2))
            {
                level0 = false;
                level1 = false;
                level2 = true;
                level3 = false;
            }
            if (stat.IsKeyDown(Keys.NumPad3))
            {
                level0 = false;
                level1 = false;
                level2 = false;
                level3 = true;
            }
               
            GamePadState currentState = GamePad.GetState(PlayerIndex.One);
            if (currentState.IsConnected)
            {
                if (gameState == GameState.Tutorial && (currentState.ThumbSticks.Left.X > 0.5 || currentState.ThumbSticks.Left.X < -0.5 || currentState.ThumbSticks.Left.Y > 0.5 || currentState.ThumbSticks.Left.Y < -0.5) && zaehler == 0)
                    gameState = GameState.Playing;
                if (gameState == GameState.Tutorial && currentState.IsButtonDown(Buttons.A) && zaehler == 1)
                    gameState = GameState.Playing;
                if (gameState == GameState.Tutorial && (currentState.IsButtonDown(Buttons.LeftShoulder) || currentState.IsButtonDown(Buttons.RightShoulder)) && zaehler == 2)
                    gameState = GameState.Playing;
                if (gameState == GameState.Tutorial && currentState.IsButtonDown(Buttons.X) && zaehler == 3)
                    gameState = GameState.Playing;
            }
            else
            {
                if (gameState == GameState.Tutorial && (stat.IsKeyDown(Keys.W) || stat.IsKeyDown(Keys.A) || stat.IsKeyDown(Keys.S) || stat.IsKeyDown(Keys.D)) && zaehler == 0)
                    gameState = GameState.Playing;
                if (gameState == GameState.Tutorial && stat.IsKeyDown(Keys.Space) && zaehler == 1)
                    gameState = GameState.Playing;
                if (gameState == GameState.Tutorial && (stat.IsKeyDown(Keys.Q) || stat.IsKeyDown(Keys.E)) && zaehler == 2)
                    gameState = GameState.Playing;
                if (gameState == GameState.Tutorial && stat.IsKeyDown(Keys.F) && zaehler == 3)
                    gameState = GameState.Playing;
            }
            if (stat.IsKeyDown(Keys.P))
            {
                gameState = GameState.Loading;
                isLoading = false;

            }
            if (gameState == GameState.Loading && !isLoading) //isLoading bool is to prevent the LoadGame method from being called 60 times a seconds
            {
                backgroundThread = new Thread(LoadGame);
                isLoading = true;

                backgroundThread.Start();
            }


            if (player.PlayerHealth <= 0)
            {
                isPlayerDead = true;
                reset();
            }
            if (gameState == GameState.Playing && !isPlayerDead)
            {
                if (stat.IsKeyDown(Keys.D1))
                    spawnEnemyTime = 100;
                if (stat.IsKeyDown(Keys.D2))
                    spawnEnemyTime = 50;
                if (stat.IsKeyDown(Keys.D3))
                    spawnEnemyTime = 10;
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                    this.Exit();


                if (spawnTimer > spawnTime)
                {
                    spawnTimer = 0;
                    spawn.Asteroid(Content);
                }

                if (spawn.boss1.Count == 0)
                {
                    if (spawnEnemyTimer > spawnEnemyTime)
                    {
                        spawnEnemyTimer = 0;
                        spawn.EnemyShip(Content);

                    }
                }
                if (spawnBoss2 && stopSpawn == true)
                {

                    if (hud.rectangleBoss.Width < 700)
                    {
                        spawnBoss = true;
                    }
                    else
                    {
                        spawnBoss = false;
                        stopSpawn = false;


                    }

                }

                if (spawnBoss == true && spawnedBoss == 0)
                {
                    spawn.Boss1(Content);
                    spawnedBoss = 1;
                    // spawnBoss = false;    
                }

                if (level0 == true)
                {
                    collision.Update(gameTime, player, spawn, hud, killedEnemies, boss1);
                    player.Update(gameTime, Content, spawn.asteroid, spawn.enemies);
                    cross.Update(gameTime);
                    hud.Update(gameTime, player.PlayerPosition, spawn.asteroid.Count() + spawn.enemies.Count(), spawnBoss);
                    i++;
                    if (i == 200)
                    {
                        gameState = GameState.Tutorial;
                        zaehler = 0;
                    }
                    if (i == 300)
                    {
                        gameState = GameState.Tutorial;
                        zaehler = 1;
                    }
                    if (i == 400)
                    {
                        gameState = GameState.Tutorial;
                        zaehler = 2;
                    }
                    if (i == 500)
                    {
                        gameState = GameState.Tutorial;
                        zaehler = 3;
                    }
                    if (i == 600)
                    {
                        level0 = false;
                        level1 = true;
                    }

                }
                if (level1 == true)
                {
                    collision.Update(gameTime, player, spawn, hud, killedEnemies, boss1);
                    player.Update(gameTime, Content, spawn.asteroid, spawn.enemies);
                    cross.Update(gameTime);
                    hud.Update(gameTime, player.PlayerPosition, spawn.asteroid.Count() + spawn.enemies.Count(), spawnBoss);
                    spawn.Update(gameTime, Content, player.PlayerPosition);
                    //station.Update(gameTime);
                  //  bomb.Update(gameTime);
                    spawnTimer++;
                    spawnEnemyTimer++;
                    if (killedEnemies > 5)
                    {
                        level1 = false;
                        level2 = true;

                    }
                }
                if (level2 == true)
                {
                    collision.Update(gameTime, player, spawn, hud, killedEnemies, boss1);
                    player.Update(gameTime, Content, spawn.asteroid, spawn.enemies);
                    cross.Update(gameTime);
                    hud.Update(gameTime, player.PlayerPosition, spawn.asteroid.Count() + spawn.enemies.Count(), spawnBoss);
                    spawn.Update(gameTime, Content, player.PlayerPosition);
                    //station.Update(gameTime);
                    spawnBoss2 = true;
                    if (boss1.BossLife <= 0)
                    {
                        level2 = false;
                        level3 = true;
                    }

                }
                if (level3 == true)
                {
                    collision.Update(gameTime, player, spawn, hud, killedEnemies, boss1);
                    player.Update(gameTime, Content, spawn.asteroid, spawn.enemies);
                    cross.Update(gameTime);
                    hud.Update(gameTime, player.PlayerPosition, spawn.asteroid.Count() + spawn.enemies.Count(), spawnBoss);
                    //spawn.Update(gameTime, Content, player.PlayerPosition);
                    station.Update(gameTime);
                    //spawnBoss

                }

                View = Matrix.CreateLookAt(cameraPosition, Vector3.Zero, Vector3.Up);
            }

            mouseState = Mouse.GetState();
            if (previousMouseState.LeftButton == ButtonState.Pressed &&
                mouseState.LeftButton == ButtonState.Released)
            {
                MouseClicked(mouseState.X, mouseState.Y);
            }

            previousMouseState = mouseState;

            if (gameState == GameState.Playing && isLoading)
            {
                LoadGame();
                isLoading = false;
            }

            killedEnemies = collision.IncrementkilledEnemie();


            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            //device.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Blue, 1.0f, 0);

            spriteBatch.Begin();
            if (gameState == GameState.StartMenu)
            {
                spriteBatch.Draw(background, backgroundPosition, Color.White);
                spriteBatch.Draw(startButton, startButtonPosition, Color.White);
                spriteBatch.Draw(exitButton, exitButtonPosition, Color.White);


            }

            //show the loading screen when needed
            if (gameState == GameState.Loading)
            {
                spriteBatch.Draw(loadingScreen, new Vector2((GraphicsDevice.Viewport.Width / 2) - (loadingScreen.Width / 2), (GraphicsDevice.Viewport.Height / 2) - (loadingScreen.Height / 2)), Color.YellowGreen);
            }



            if (gameState == GameState.Playing)
            {

                spriteBatch.Draw(pauseButton, new Vector2(700, 0), Color.White);
                if (level0 == true)
                {
                    skybox.Draw(Projection, View, device);
                    GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                    player.Draw(Projection, View);
                    cross.Draw(Projection, View);
                    hud.Draw(spriteBatch, killedEnemies);

                }
                if (level1 == true)
                {
                    skybox.Draw(Projection, View, device);
                    GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                    player.Draw(Projection, View);
                    cross.Draw(Projection, View);
                    hud.Draw(spriteBatch, killedEnemies);
                    spawn.Draw(Projection, View);
                    //bomb.Draw(Projection, View);
                    // station.Draw(Projection, View);
                }
                if (level2 == true)
                {
                    skybox.Draw(Projection, View, device);
                    GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                    player.Draw(Projection, View);
                    cross.Draw(Projection, View);
                    hud.Draw(spriteBatch, killedEnemies);
                    spawn.Draw(Projection, View);
                    // station.Draw(Projection, View);
                }
                if (level3 == true)
                {
                    skybox.Draw(Projection, View, device);
                    GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                    player.Draw(Projection, View);
                    cross.Draw(Projection, View);
                    hud.Draw(spriteBatch, killedEnemies);
                    //spawn.Draw(Projection, View);
                    station.Draw(Projection, View);
                }

            }
            if (gameState == GameState.Paused)
            {
                spriteBatch.Draw(resumeButton, resumeButtonPosition, Color.White);
            }

            if (gameState == GameState.Tutorial)
            {
                skybox.Draw(Projection, View, device);
                GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                player.Draw(Projection, View);
                cross.Draw(Projection, View);
                hud.Draw(spriteBatch, killedEnemies);
                hud.Draw2(spriteBatch, zaehler);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }


        void MouseClicked(int x, int y)
        {
            //creates a rectangle of 10x10 around the place where the mouse was clicked
            Rectangle mouseClickRect = new Rectangle(x, y, 10, 10);

            //check the startmenu
            if (gameState == GameState.StartMenu)
            {
                Rectangle startButtonRect = new Rectangle((int)startButtonPosition.X, (int)startButtonPosition.Y, 137, 40);
                Rectangle exitButtonRect = new Rectangle((int)exitButtonPosition.X, (int)exitButtonPosition.Y, 137, 40);
                Rectangle endlesssButtonRect = new Rectangle((int)endlessButtonPosition.X, (int)endlessButtonPosition.Y, 137, 40);

                if (mouseClickRect.Intersects(startButtonRect)) //player clicked start button
                {
                    gameState = GameState.Loading;
                    isLoading = false;
                }
                else if (mouseClickRect.Intersects(exitButtonRect)) //player clicked exit button
                {
                    this.Exit();
                }
            }

            //check the pausebutton
            if (gameState == GameState.Playing)
            {
                Rectangle pauseButtonRect = new Rectangle(700, 0, 70, 70);

                if (mouseClickRect.Intersects(pauseButtonRect))
                {
                    gameState = GameState.Paused;
                }
            }

            //check the resumebutton
            if (gameState == GameState.Paused)
            {
                Rectangle resumeButtonRect = new Rectangle((int)resumeButtonPosition.X, (int)resumeButtonPosition.Y, 137, 40);

                if (mouseClickRect.Intersects(resumeButtonRect))
                {
                    gameState = GameState.Playing;
                }
            }
        }

        void LoadGame()
        {
            pauseButton = Content.Load<Texture2D>(@"Menü/pause");
            resumeButton = Content.Load<Texture2D>(@"Menü/resumebutton");
            resumeButtonPosition = new Vector2((GraphicsDevice.Viewport.Width / 2) - (resumeButton.Width / 2),
                                               (GraphicsDevice.Viewport.Height / 2) - (resumeButton.Height / 2));      
            Thread.Sleep(1000);
          
            gameState = GameState.Playing;
            isLoading = false;
        }


        void reset()
        {
            gameState = GameState.StartMenu;
            player.PlayerPosition = Vector3.Zero;
            player.PlayerHealth = 100.0f;
            spawnEnemyTime = 50;
            while (spawn.asteroid.Count() != 0 || spawn.enemies.Count() !=0 || spawn.boss1.Count() != 0)
            {
                for (int i = 0; i < spawn.asteroid.Count(); i++)
                    spawn.asteroid.RemoveAt(i);
                for (int i = 0; i < spawn.enemies.Count(); i++)
                    spawn.enemies.RemoveAt(i);
                for (int i = 0; i < spawn.boss1.Count(); i++)
                    spawn.boss1.RemoveAt(i);
            }
           
            isPlayerDead = false;
            collision.killedEnemies = 0;
            spawnBoss = false;
        }

    }

}
