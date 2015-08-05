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
            Paused
        }
    
        private Texture2D startButton;
        private Texture2D exitButton;
        private Texture2D pauseButton;
        private Texture2D resumeButton;
        private Texture2D loadingScreen;
        private Vector2 startButtonPosition;
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

        int spawnEnemyTimer = 0;
        int spawnTimer = 0;
        int spawnTime = 10;
        int spawnEnemyTime = 50;
        int spawnedBoss = 0;

        public int killedEnemies = 0;

        bool spawnBoss = false;
        bool stopSpawn = true; 

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
            startButtonPosition = new Vector2((GraphicsDevice.Viewport.Width / 2) - 50, 200);
            exitButtonPosition = new Vector2((GraphicsDevice.Viewport.Width / 2) - 50, 250);  
    
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
          

            spriteBatch = new SpriteBatch(GraphicsDevice);

            //load the buttonimages into the content pipeline
            startButton = Content.Load<Texture2D>(@"Menü/start");
            exitButton = Content.Load<Texture2D>(@"Menü/exit");

            //load the loading screen
            loadingScreen = Content.Load<Texture2D>(@"Menü/loading");
        }


        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState stat = Keyboard.GetState();
            if (stat.IsKeyDown(Keys.Escape))
                this.Exit();

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


            if (player.playerHealth <= 0)
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

                        if (spawn.boss1.Count == 0 )
                        {
                            if (spawnEnemyTimer > spawnEnemyTime)
                            {
                                spawnEnemyTimer = 0;
                                spawn.EnemyShip(Content);

                            }
                        }
                        if (killedEnemies >= 0 && stopSpawn == true)
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
                  
                  
                    collision.Update(gameTime, player, spawn, hud, killedEnemies);
                    player.Update(gameTime, Content, spawn.asteroid, spawn.enemies);
                    cross.Update(gameTime);
                    spawn.Update(gameTime, Content, player.modelPosition);
                    hud.Update(gameTime, player.modelPosition, spawn.asteroid.Count() + spawn.enemies.Count(),spawnBoss);

                    spawnTimer++;
                    spawnEnemyTimer++;
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

                 skybox.Draw(Projection, View, device);

                 GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                 player.Draw(Projection, View);
                 cross.Draw(Projection, View);
                 spawn.Draw(Projection, View);
                 hud.Draw(spriteBatch, killedEnemies);
                 
             }
             if (gameState == GameState.Paused)
             {
                 spriteBatch.Draw(resumeButton, resumeButtonPosition, Color.White);
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
                Rectangle startButtonRect = new Rectangle((int)startButtonPosition.X, (int)startButtonPosition.Y, 100, 20);
                Rectangle exitButtonRect = new Rectangle((int)exitButtonPosition.X, (int)exitButtonPosition.Y, 100, 20);

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
                Rectangle resumeButtonRect = new Rectangle((int)resumeButtonPosition.X, (int)resumeButtonPosition.Y, 100, 20);

                if (mouseClickRect.Intersects(resumeButtonRect))
                {
                    gameState = GameState.Playing;
                }
            }
        }

        void LoadGame()
        {
            pauseButton = Content.Load<Texture2D>(@"Menü/pause");
            resumeButton = Content.Load<Texture2D>(@"Menü/resume");
            resumeButtonPosition = new Vector2((GraphicsDevice.Viewport.Width / 2) - (resumeButton.Width / 2),
                                               (GraphicsDevice.Viewport.Height / 2) - (resumeButton.Height / 2));      
            Thread.Sleep(1000);
          
            gameState = GameState.Playing;
            isLoading = false;
        }


        void reset()
        {
            gameState = GameState.StartMenu;
            player.modelPosition = Vector3.Zero;
            player.playerHealth = 100.0f;
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
