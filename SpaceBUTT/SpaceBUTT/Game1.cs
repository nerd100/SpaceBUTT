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
       
       
        public Matrix View;
        public Matrix Projection;
        public Vector3 modelPosition = new Vector3(0, 0, 0);
        public GraphicsDevice device;

        Model myModel;
            
        Vector3 cameraPosition = new Vector3(0.0f, 1500.0f, 5000.0f);
        Vector3 cameraView = new Vector3(0.0f, 0.0f, 0.0f);
        Vector3 modelVelocity = new Vector3(0, 0, 0);
        
        float aspectRatio;
        float modelRotationZ = 0.0f;
        float modelRotationX = 0.0f;
        float modelSpeed = 5.0f;
        float screenWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        float screenHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;


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
            spriteBatch = new SpriteBatch(GraphicsDevice);
            myModel = Content.Load<Model>("Ship1");
            aspectRatio = graphics.GraphicsDevice.Viewport.AspectRatio;
            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), aspectRatio, 1.0f, 10000.0f);
            View = Matrix.CreateLookAt(cameraPosition, cameraView, Vector3.Up);    
        
        }


        protected override void UnloadContent()
        {
      
        }

     
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            
            UpdateInput();
                     
            modelPosition += modelVelocity;
     
            modelVelocity *= 0.95f;
            modelRotationX *= 0.95f;
            modelRotationZ *= 0.95f;

            View = Matrix.CreateLookAt(cameraPosition, Vector3.Zero, Vector3.Up);
        
            base.Update(gameTime);
        }


        protected void UpdateInput()
        {
            KeyboardState stat = Keyboard.GetState();
            Vector3 modelVelocityX = Vector3.Zero;
            Vector3 modelVelocityY = Vector3.Zero;

            //Xbox controls
            GamePadState currentState = GamePad.GetState(PlayerIndex.One);
            if (currentState.IsConnected)
            {
                modelRotationZ -= currentState.ThumbSticks.Left.X * 0.02f;
                modelRotationX += currentState.ThumbSticks.Left.Y * 0.02f;
                
                modelVelocityX.X = modelSpeed;
                modelVelocityY.Y = modelSpeed;

                modelVelocityX *= currentState.ThumbSticks.Left.X;
                modelVelocityY *= currentState.ThumbSticks.Left.Y;

                modelVelocity += modelVelocityX;
                modelVelocity += modelVelocityY;

               /* GamePad.SetVibration(PlayerIndex.One,
                    currentState.Triggers.Right,
                    currentState.Triggers.Left);
                */
            }
            else {
                if (stat.IsKeyDown(Keys.Escape)) {
                    this.Exit();
                }
                //keyboard controls
                if (stat.IsKeyDown(Keys.W))
                {
                    modelRotationX += 0.02f;
                    modelVelocityY.Y = modelSpeed;
                    modelVelocityY *= 1;
                    modelVelocity += modelVelocityY;
                }
                if (stat.IsKeyDown(Keys.A))
                {
                    modelRotationZ += 0.02f;
                    modelVelocityX.X -= modelSpeed;
                    modelVelocityX *= 1;
                    modelVelocity += modelVelocityX;                  
                    
                }
                if (stat.IsKeyDown(Keys.S))
                {
                    modelRotationX -= 0.02f;
                    modelVelocityY.Y -= modelSpeed;
                    modelVelocityY *= 1;
                    modelVelocity += modelVelocityY;
                }
                if (stat.IsKeyDown(Keys.D))
                {
                    modelRotationZ -= 0.02f;
                    modelVelocityX.X = modelSpeed;
                    modelVelocityX *= 1;
                    modelVelocity += modelVelocityX;                  
                }
            }
                //screensize TODO: Was anderes überlegen!
                if (modelPosition.Y >= screenHeight + 900)
                {
                    modelPosition.Y = screenHeight + 900;
                }
                if (modelPosition.Y <= -(screenHeight + 900))
                {
                    modelPosition.Y = -(screenHeight + 900);
                }
                if (modelPosition.X >= screenWidth+1000)
                {
                    modelPosition.X = screenWidth+1000;
                }
                if (modelPosition.X <= -(screenWidth + 1000))
                {
                    modelPosition.X  = -(screenWidth + 1000);
                }                     
        }
     

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            DrawModel(myModel);
          
            base.Draw(gameTime);
        }


        private void DrawModel(Model myModel)
        {
            foreach (ModelMesh mesh in myModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = Matrix.Identity * Matrix.CreateRotationY(3.1f) * Matrix.CreateRotationX(modelRotationX) 
                        * Matrix.CreateRotationZ(modelRotationZ) * Matrix.CreateTranslation(modelPosition);
                    effect.View = View;
                    effect.Projection = Projection;
                }
                mesh.Draw();
            }


        }
    }
}
