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
            Tutorial,
            Video,
            GameOver,
            Credits
        }
        private Texture2D GameOverScreen;
        private Texture2D CreditScreen;
        private Texture2D background;
        private Texture2D pausebackground;
        private Texture2D startButton;
        private Texture2D exitButton;
        private Texture2D endlessButton;
        private Texture2D ReturnButton;
        private Texture2D QuitButton;
        private Texture2D pauseButton;
        private Texture2D resumeButton;
        private Texture2D loadingScreen;
        private Texture2D CreditButton;
        private Vector2 CreditScreenPosition;
        private Vector2 CreditButtonPosition;
        private Vector2 returnButtonPosition;
        private Vector2 returnButtonPosition2;
        private Vector2 quitButtonPosition;
        private Vector2 GameOverScreenPosition;
        private Vector2 pausebackgroundPosition;
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

        int spawnGeschuetzTimer = 0;
        int spawnEnemyTimer = 0;
        int spawnTimer = 0;
        int spawnGeschuetzTime = 20;
        int spawnTime = 10;
        int spawnEnemyTime = 50;
        int spawnedBoss = 0;
        int spawnBalkenTime = 50;
        int spawnBalkenTimer = 0;

        public int killedEnemies = 0;
        bool endless;
        bool spawnBoss2 = false;
        bool spawnBoss = false;
        bool stopSpawn = true;
        bool sound = false;
        bool level0 = true;
        bool level1;
        bool level2;
        bool level3;

        bool videorun0 = true;
        bool videorun1;
        bool videorun2;
        bool videorun3;

        bool videobla0 = true ;
        bool videobla1;
        bool videobla2;
        bool videobla3;

        int zaehler;
        int zaehler2;
        float Time = 0;

        public Matrix View;
        public Matrix Projection;
        public GraphicsDevice device;

        Vector3 cameraPosition = new Vector3(0.0f, 1000.0f, 5000.0f);
        Vector3 cameraView = Vector3.Zero;

        float aspectRatio;

        //Sound
     //   SoundEffect soundeff;
        public Song song;
        public Song song2;

        //Video
        Video video0;
        Video video1;
        Video video2;
        Video video3;

        VideoPlayer player2;
        VideoPlayer player3;
        VideoPlayer player4;
        VideoPlayer player5;
 

        Texture2D videoTexture;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1280;
        }


        protected override void Initialize()
        {
            IsMouseVisible = true;
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferHeight = 600   ;
            graphics.PreferredBackBufferWidth = 800;
            graphics.ApplyChanges();

            //menu.Initialize(IsMouseVisible,GraphicsDevice.Viewport.Width);

            startButtonPosition = new Vector2((GraphicsDevice.Viewport.Width / 2) - 60, 250);
            endlessButtonPosition = new Vector2((GraphicsDevice.Viewport.Width / 2) - 60, 300);
            CreditButtonPosition = new Vector2((GraphicsDevice.Viewport.Width / 2) - 60, 350);
            exitButtonPosition = new Vector2((GraphicsDevice.Viewport.Width / 2) - 60, 400);
        
            returnButtonPosition = new Vector2(325,390);
            returnButtonPosition2 = new Vector2(450, 500);
            quitButtonPosition = new Vector2(325,450);
            backgroundPosition = new Vector2(0, 0);
            pausebackgroundPosition = new Vector2(0, 0);
            GameOverScreenPosition = new Vector2(0, 0);
            CreditScreenPosition = new Vector2(0, 0);
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
            //Sounds
            song = Content.Load<Song>("Sounds/maintheme1");  // Put the name of your song here instead of "song_title"
            song2 = Content.Load<Song>("Sounds/ingame");
            MediaPlayer.Play(song);
            MediaPlayer.IsRepeating = true;
            

            //Video
            video0 = Content.Load<Video>("Video/Intro");
            video1 = Content.Load<Video>("Video/level1");
            video2 = Content.Load<Video>("Video/level3");
            video3 = Content.Load<Video>("Video/End");
           

            player2 = new VideoPlayer();
            player3 = new VideoPlayer();
            player4 = new VideoPlayer();
            player5 = new VideoPlayer();

            aspectRatio = graphics.GraphicsDevice.Viewport.AspectRatio;
            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(60.0f), aspectRatio, 1.0f, 10000000.0f);
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
            endlessButton = Content.Load<Texture2D>(@"Menü/endlessButton");
            exitButton = Content.Load<Texture2D>(@"Menü/quitbutton");
            background = Content.Load<Texture2D>(@"Menü/mainmenu");
            pausebackground = Content.Load<Texture2D>(@"Menü/pausebackround");
            GameOverScreen = Content.Load<Texture2D>(@"Menü/GameOver");
            ReturnButton = Content.Load<Texture2D>(@"Menü/Return");
            QuitButton = Content.Load<Texture2D>(@"Menü/quitbutton");
            //load the loading screen
            loadingScreen = Content.Load<Texture2D>(@"Menü/loadbackround");
            CreditButton = Content.Load<Texture2D>(@"Menü/Credit");
            CreditScreen = Content.Load<Texture2D>(@"Menü/CreditScreen");

            //soundeff = Content.Load<SoundEffect>("");

        }


        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            

            if (gameState == GameState.Video && videorun0)
            {
                player2.Play(video0);
                videorun0 = false;
                
            }
            if (gameState == GameState.Video && videorun1)
            {
                player3.Play(video1);
                videorun1 = false;
                videobla1 = true;
            }
            if (gameState == GameState.Video && videorun2)
            {
                player4.Play(video2);
                videorun2 = false;
                videobla2 = true;
            }
            if (gameState == GameState.Video && videorun3)
            {
                player5.Play(video3);
                videorun3 = false;
                videobla3 = true;
            }
            if (gameState == GameState.GameOver) 
            {
                reset();
            }


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
                gameState = GameState.GameOver;
                //reset();
            }
            if (gameState == GameState.Playing && !isPlayerDead && endless == true)
            {
                if (sound == true)
                {
                    MediaPlayer.Stop();
                    MediaPlayer.Play(song2);
                    sound = false;
                }

                if (spawnTimer > spawnTime)
                {
                    spawnTimer = 0;
                    spawn.Asteroid(Content);

                }
                if (spawnEnemyTimer > spawnEnemyTime)
                {
                        spawnEnemyTimer = 0;
                        spawn.EnemyShip(Content);  
                }

                    Time += 0.5f;
                    if (Time == 100)
                        spawnEnemyTime = 100;
                    if (Time == 200)
                        spawnEnemyTime = 30;
                    if (Time == 1000)
                        spawnEnemyTime = 10;
                    collision.Update(gameTime, player, spawn, hud, killedEnemies, boss1);
                    player.Update(gameTime, Content, spawn.asteroid, spawn.enemies);
                    cross.Update(gameTime);
                    hud.Update(gameTime, player.PlayerPosition, spawn.asteroid.Count() + spawn.enemies.Count(), spawnBoss);
                    spawn.Update(gameTime, Content, player.PlayerPosition);

                    spawnTimer++;
                    spawnEnemyTimer++;

                
            }



            if (gameState == GameState.Playing && !isPlayerDead&& endless != true)
            {
                

                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                    this.Exit();


                if (spawnTimer > spawnTime && (level1 || level0))
                {
                    spawnTimer = 0;
                    spawn.Asteroid(Content);

                }
                if (spawnGeschuetzTimer > spawnGeschuetzTime && level3)
                {
                    spawnGeschuetzTimer = 0;
                    spawn.Geschuetz(Content);

                }
                if (spawnBalkenTimer > spawnBalkenTime && level3)
                {
                    spawnBalkenTimer = 0;
                    spawn.Balken(Content);
                }

               if (spawn.boss1.Count == 0 && level1)
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
                    if (sound == true)
                    {
                        MediaPlayer.Stop();
                        MediaPlayer.Play(song2);
                        sound = false;
                    }

                    collision.Update(gameTime, player, spawn, hud, killedEnemies, boss1);
                    player.Update(gameTime, Content, spawn.asteroid, spawn.enemies);
                    cross.Update(gameTime);
                    hud.Update(gameTime, player.PlayerPosition, spawn.asteroid.Count() + spawn.enemies.Count(), spawnBoss);
                    spawn.Update(gameTime, Content, player.PlayerPosition);
                    Time++;
                    if (Time == 200)
                    {
                        gameState = GameState.Tutorial;
                        zaehler = 0;
                    }
                    if (Time == 300)
                    {
                        gameState = GameState.Tutorial;
                        zaehler = 1;
                    }
                    if (Time == 400)
                    {
                        gameState = GameState.Tutorial;
                        zaehler = 2;
                    }
                    if (Time == 500)
                    {
                        gameState = GameState.Tutorial;
                        zaehler = 3;
                    }
                    if (Time == 600)
                    {
                        level0 = false;
                        level1 = true;
                    }
                    spawnTimer++;
                  //  spawnEnemyTimer++;
                    spawnGeschuetzTimer++;
                    spawnBalkenTimer++;
                }
                if (level1 == true)    
                {
                    if (sound == true)
                    {
                        MediaPlayer.Stop();
                        MediaPlayer.Play(song2);
                        sound = false;
                    }

                    Time += 0.5f;
                    if (Time == 610)
                        spawnEnemyTime = 100;
                    if (Time == 1000)
                        spawnEnemyTime = 50;
                    if (Time == 1500)
                        spawnEnemyTime = 10;
                    collision.Update(gameTime, player, spawn, hud, killedEnemies, boss1);
                    player.Update(gameTime, Content, spawn.asteroid, spawn.enemies);
                    cross.Update(gameTime);
                    hud.Update(gameTime, player.PlayerPosition, spawn.asteroid.Count() + spawn.enemies.Count(), spawnBoss);
                    spawn.Update(gameTime, Content, player.PlayerPosition);
                    //station.Update(gameTime);
                    //  bomb.Update(gameTime);
                    spawnTimer++;
                    spawnEnemyTimer++;
                  
                    if (Time == 2000)
                    {
                        gameState = GameState.Video;
                        videobla0 = false;
                        videorun1 = true;
                        
                        
                        level1 = false;
                        level2 = true;
                    
                    }
                }
                if (level2 == true)
                {
                    if (sound == true)
                    {
                        MediaPlayer.Stop();
                        MediaPlayer.Play(song2);
                        sound = false;
                    }
                    collision.Update(gameTime, player, spawn, hud, killedEnemies, boss1);
                    player.Update(gameTime, Content, spawn.asteroid, spawn.enemies);
                    cross.Update(gameTime);
                    hud.Update(gameTime, player.PlayerPosition, spawn.asteroid.Count() + spawn.enemies.Count(), spawnBoss);
                    spawn.Update(gameTime, Content, player.PlayerPosition);
                    //station.Update(gameTime);
                    spawnBoss2 = true;
                    if (boss1.BossLife <= 0)
                    {
                        gameState = GameState.Video;
                        videobla1 = false;
                        videorun2 = true;
                        level2 = false;
                        level3 = true;
                    }
                    spawnTimer++;
                   
                    spawnGeschuetzTimer++;
                    spawnBalkenTimer++;
                }
                if (level3 == true)
                {
                    if (sound == true)
                    {
                        MediaPlayer.Stop();
                        MediaPlayer.Play(song2);
                        sound = false;
                    }
                    collision.Update(gameTime, player, spawn, hud, killedEnemies, boss1);
                    player.Update(gameTime, Content, spawn.asteroid, spawn.enemies);
                    cross.Update(gameTime);
                    hud.Update(gameTime, player.PlayerPosition, spawn.asteroid.Count() + spawn.enemies.Count(), spawnBoss);
                    spawn.Update(gameTime, Content, player.PlayerPosition);
                    station.Update(gameTime);
                    //spawnBoss
                    if (zaehler2 == 2000)
                    {
                        gameState = GameState.Video;
                        videobla2 = false;
                        videorun3 = true;
                    }
                    zaehler2++;
                    spawnTimer++;
                   
                    spawnGeschuetzTimer++;
                    spawnBalkenTimer++;
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
            // Only call GetTexture if a video is playing or paused
            graphics.GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            if (gameState == GameState.Credits)
            {
                spriteBatch.Draw(CreditScreen, CreditScreenPosition, Color.White);
                spriteBatch.Draw(ReturnButton, returnButtonPosition2, Color.White);
            }
            if (gameState == GameState.GameOver) 
            {
                spriteBatch.Draw(GameOverScreen, GameOverScreenPosition, Color.White);
                spriteBatch.Draw(ReturnButton, returnButtonPosition, Color.White);
                spriteBatch.Draw(QuitButton, quitButtonPosition, Color.White);
            }
            if (gameState == GameState.Video)
            {
                MediaPlayer.Stop();
                if (gameState == GameState.Video && videobla0)
                {
                    if (player2.State != MediaState.Stopped)
                        videoTexture = player2.GetTexture();

                    // Drawing to the rectangle will stretch the 
                    // video to fill the screen
                    Rectangle screen = new Rectangle(GraphicsDevice.Viewport.X,
                        GraphicsDevice.Viewport.Y,
                        GraphicsDevice.Viewport.Width,
                        GraphicsDevice.Viewport.Height);

                    // Draw the video, if we have a texture to draw.


                    spriteBatch.Draw(videoTexture, screen, Color.White);


                    if (player2.State == MediaState.Stopped)
                    {
                        gameState = GameState.Playing;
                        player2.Stop();
                        sound = true;
                       
                      
                    }
                }
                if (gameState == GameState.Video && videobla1)
                {
                    if (player3.State != MediaState.Stopped)
                        videoTexture = player3.GetTexture();

                    // Drawing to the rectangle will stretch the 
                    // video to fill the screen
                    Rectangle screen = new Rectangle(GraphicsDevice.Viewport.X,
                        GraphicsDevice.Viewport.Y,
                        GraphicsDevice.Viewport.Width,
                        GraphicsDevice.Viewport.Height);

                    // Draw the video, if we have a texture to draw.

                    spriteBatch.Draw(videoTexture, screen, Color.White);

                    if (player3.State == MediaState.Stopped)
                    {
                        gameState = GameState.Playing;
                        player3.Stop();
                        sound = true;
                    }
                }
                if (gameState == GameState.Video && videobla2)
                {
                    if (player4.State != MediaState.Stopped)
                        videoTexture = player4.GetTexture();

                    // Drawing to the rectangle will stretch the 
                    // video to fill the screen
                    Rectangle screen = new Rectangle(GraphicsDevice.Viewport.X,
                        GraphicsDevice.Viewport.Y,
                        GraphicsDevice.Viewport.Width,
                        GraphicsDevice.Viewport.Height);

                    // Draw the video, if we have a texture to draw.

                    spriteBatch.Draw(videoTexture, screen, Color.White);

                    if (player4.State == MediaState.Stopped)
                    {
                        gameState = GameState.Playing;
                        player4.Stop();
                        sound = true;
                    }
                }
                if (gameState == GameState.Video && videobla3)
                {
                    if (player5.State != MediaState.Stopped)
                        videoTexture = player5.GetTexture();

                    // Drawing to the rectangle will stretch the 
                    // video to fill the screen
                    Rectangle screen = new Rectangle(GraphicsDevice.Viewport.X,
                        GraphicsDevice.Viewport.Y,
                        GraphicsDevice.Viewport.Width,
                        GraphicsDevice.Viewport.Height);

                    // Draw the video, if we have a texture to draw.

                    spriteBatch.Draw(videoTexture, screen, Color.White);

                    if (player5.State == MediaState.Stopped)
                    {
                        gameState = GameState.StartMenu;
                        player5.Stop();
                        sound = true;
                    }
                }
            }
            else
            {


                if (gameState == GameState.StartMenu)
                {


                    // Drawing to the rectangle will stretch the 
                    // video to fill the screen


                    spriteBatch.Draw(background, backgroundPosition, Color.White);
                    spriteBatch.Draw(startButton, startButtonPosition, Color.White);
                    spriteBatch.Draw(endlessButton, endlessButtonPosition, Color.White);
                    spriteBatch.Draw(exitButton, exitButtonPosition, Color.White);
                    spriteBatch.Draw(CreditButton, CreditButtonPosition, Color.White);
                    //spriteBatch.Draw(videoPlayer.GetTexture(), new Rectangle(0, 0, myVideoFile.Width, myVideoFile.Height), Color.CornflowerBlue);

                }

                //show the loading screen when needed
                if (gameState == GameState.Loading)
                {
                    spriteBatch.Draw(loadingScreen, new Vector2((GraphicsDevice.Viewport.Width / 2) - (loadingScreen.Width / 2), (GraphicsDevice.Viewport.Height / 2) - (loadingScreen.Height / 2)), Color.YellowGreen);
                }

                if (gameState == GameState.Playing && endless == true) 
                {
                    skybox.Draw(Projection, View, device);
                    GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                    player.Draw(Projection, View);
                    cross.Draw(Projection, View);
                    hud.Draw(spriteBatch, killedEnemies);
                    spawn.Draw(Projection, View);
                }

                if (gameState == GameState.Playing && endless != true)
                {

                    spriteBatch.Draw(pauseButton, new Vector2(700, 0), Color.White);
                    if (level0 == true)
                    {

                        skybox.Draw(Projection, View, device);
                        GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                        player.Draw(Projection, View);
                        cross.Draw(Projection, View);
                        hud.Draw(spriteBatch, killedEnemies);
                        spawn.Draw(Projection, View);

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
                        spawn.Draw(Projection, View);
                        station.Draw(Projection, View);
                    }

                }
                if (gameState == GameState.Paused)
                {
                    spriteBatch.Draw(pausebackground, pausebackgroundPosition, Color.White);
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

                
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }


        void MouseClicked(int x, int y)
        {
            //creates a rectangle of 10x10 around the place where the mouse was clicked
            Rectangle mouseClickRect = new Rectangle(x, y, 10, 10);

            //check the startmenu
            if (gameState == GameState.GameOver) 
            {
                Rectangle returnButtonRect = new Rectangle((int)returnButtonPosition.X, (int)returnButtonPosition.Y, 137, 40);
                Rectangle quitButtonRect = new Rectangle((int)quitButtonPosition.X, (int)quitButtonPosition.Y, 137, 40);
                if (mouseClickRect.Intersects(returnButtonRect)&& endless) //player clicked start button
                {
                    reset();
                    gameState = GameState.Playing;
                    endless = true;
                    isLoading = false;
                }
                if (mouseClickRect.Intersects(quitButtonRect) && endless) //player clicked start button
                {
                    reset();
                    gameState = GameState.StartMenu;
                    endless = false;
                    isLoading = false;
                }
                if (mouseClickRect.Intersects(returnButtonRect) && endless == false) //player clicked start button
                {
                    reset();
                    gameState = GameState.Playing;
                    endless = false;
                    isLoading = true;
                }
                if (mouseClickRect.Intersects(quitButtonRect) && endless== false) //player clicked start button
                {
                    reset();
                    gameState = GameState.StartMenu;
                    endless = false;
                    isLoading = false;
                }
            }
            if (gameState == GameState.Credits)
            {
                Rectangle Return = new Rectangle((int)returnButtonPosition2.X, (int)returnButtonPosition2.Y, 137, 40);
                if (mouseClickRect.Intersects(Return)) //player clicked start button
                {
                    gameState = GameState.StartMenu;
                    
                }
            }

            if (gameState == GameState.StartMenu)
            {
                Rectangle startButtonRect = new Rectangle((int)startButtonPosition.X, (int)startButtonPosition.Y, 137, 40);
                Rectangle endlessButtonRect = new Rectangle((int)endlessButtonPosition.X, (int)endlessButtonPosition.Y, 137, 40);
                Rectangle exitButtonRect = new Rectangle((int)exitButtonPosition.X, (int)exitButtonPosition.Y, 137, 40);
                Rectangle endlesssButtonRect = new Rectangle((int)endlessButtonPosition.X, (int)endlessButtonPosition.Y, 137, 40);
                Rectangle CreditButtonRect = new Rectangle((int)CreditButtonPosition.X, (int)CreditButtonPosition.Y, 137, 40);

                if (mouseClickRect.Intersects(startButtonRect)) //player clicked start button
                {
                    gameState = GameState.Loading;
                    isLoading = false;
                }
                if (mouseClickRect.Intersects(CreditButtonRect)) //player clicked start button
                {
                    gameState = GameState.Credits;
                }
                else if (mouseClickRect.Intersects(exitButtonRect)) //player clicked exit button
                {
                    this.Exit();
                }
                else if (mouseClickRect.Intersects(endlessButtonRect)) //player clicked exit button
                {
                    gameState = GameState.Playing;
                    endless = true;
                    sound = true;
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
            pauseButton = Content.Load<Texture2D>(@"Menü/pause2");
            resumeButton = Content.Load<Texture2D>(@"Menü/resumebutton");
            resumeButtonPosition = new Vector2((GraphicsDevice.Viewport.Width / 2) - (resumeButton.Width / 2),
                                               (GraphicsDevice.Viewport.Height / 2) - (resumeButton.Height / 2));
            Thread.Sleep(1000);

            gameState = GameState.Video;
            isLoading = false;
        }


        void reset()
        {
           // gameState = GameState.GameOver;
            player.PlayerPosition = Vector3.Zero;
            player.PlayerHealth = 100.0f;
            spawnEnemyTime = 50;
            while (spawn.asteroid.Count() != 0 || spawn.enemies.Count() != 0 || spawn.boss1.Count() != 0)
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
