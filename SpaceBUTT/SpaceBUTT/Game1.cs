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


namespace SpaceBUTT
{

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Spawn spawn = new Spawn();
        
        Crosshair cross = new Crosshair();
        Player player = new Player();
        Shoot shoot = new Shoot();
        int spawnTimer = 0;
        int spawnTime = 100;

        public Matrix View;
        public Matrix Projection;      
        public GraphicsDevice device;
         
        Vector3 cameraPosition = new Vector3(0.0f, 1000.0f, 5000.0f);
        Vector3 cameraView = new Vector3(0.0f, 0.0f, 0.0f);
     
        float aspectRatio;
           

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }


        protected override void Initialize()
        {
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferredBackBufferWidth = 800;
            graphics.ApplyChanges();
           
            base.Initialize();
        }

      
        protected override void LoadContent()
        {
            device = graphics.GraphicsDevice;
           
            spriteBatch = new SpriteBatch(GraphicsDevice);
             
            aspectRatio = graphics.GraphicsDevice.Viewport.AspectRatio;
            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), aspectRatio, 1.0f, 100000.0f);
            View = Matrix.CreateLookAt(cameraPosition, cameraView, Vector3.Up);

            player.LoadContent(Content);
            cross.LoadContent(Content);
      
        }


        protected override void UnloadContent()
        {     
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState stat = Keyboard.GetState();
            if (stat.IsKeyDown(Keys.Escape))          
                this.Exit();                
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            if (stat.IsKeyDown(Keys.Space))
                shoot.LoadContent(Content, player.modelPosition);  
            if (spawnTimer > spawnTime)
              {
                  spawnTimer = 0;
                  spawn.LoadContent(Content);
              }

            for (int j = 0; j < shoot.laser.Count(); j++)
            {
                collisionCheck2(shoot.laser[j].getBoundingSphere());
            }

            collisionCheck(player.getBoundingSphere());
             

            player.Update(gameTime);
            cross.Update(gameTime);
            spawn.Update(gameTime);
            shoot.Update(gameTime);

            spawnTimer++;

            View = Matrix.CreateLookAt(cameraPosition, Vector3.Zero, Vector3.Up);

          base.Update(gameTime);
        }

      

        public bool collisionCheck(BoundingSphere sphere)
        {
            for(int i = 0;i < spawn.enemies.Count();i++)
            if (spawn.enemies[i].getBoundingSphere().Intersects(sphere) )
            {
                //blow up ship
                spawn.enemies.RemoveAt(i);
                 //exit the loop
            }
            return false;
        }

        public bool collisionCheck2(BoundingSphere sphere)
        {

            for (int i = 0; i < spawn.enemies.Count(); i++)
                if (spawn.enemies[i].getBoundingSphere().Intersects(sphere))
                {
                    //blow up ship
                    spawn.enemies.RemoveAt(i);
                    //exit the loop
                }

            return false;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            
            player.Draw(Projection, View);
            cross.Draw(Projection,View);            
            spawn.Draw(Projection, View);
            shoot.Draw(Projection, View);
            base.Draw(gameTime);
        }
       
    }
}
