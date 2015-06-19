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
   public class Player
    {
        Model myModel;
        public Spawn shoot = new Spawn();

        int shootTime = 10;
        int shootTimer = 0;

        public Vector3 modelPosition = new Vector3(0, 0, 0);
        Vector3 modelVelocity = new Vector3(0, 0, 0);
        float modelRotationZ = 0.0f;
        float modelRotationX = 0.0f;
        float modelSpeed = 7.0f;

        bool BarrelRoll = true;
        int BarrelRollTimer = 0;
        int BarrelRollTime = 120;

        float screenWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        float screenHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;


        public void LoadContent(ContentManager Content)
        {   
            myModel = Content.Load<Model>("Ship1");            
        }


        public void Update(GameTime gameTime,ContentManager Content, List<Asteroid> enemies)
        {
            KeyboardState stat = Keyboard.GetState();
                      
            modelPosition += modelVelocity;

            UpdateInput(Content,enemies);

            modelVelocity *= 0.95f;
            modelRotationX *= 0.95f;
            modelRotationZ *= 0.95f;
            getBoundingSphere();
            shoot.Update(gameTime,Content,modelPosition);
            shootTimer++;

            if (BarrelRollTimer >= BarrelRollTime)
            {
                BarrelRollTimer = 0;
                BarrelRoll = true;
            }
            BarrelRollTimer++;

        }

        public void UpdateInput(ContentManager Content,List<Asteroid> enemies)
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

                 GamePad.SetVibration(PlayerIndex.One,
                     currentState.Triggers.Right,
                     currentState.Triggers.Left);  
            }
            else
            {     
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
                if (stat.IsKeyDown(Keys.F) && BarrelRoll == true) //BarrelRoll
                {
                    BarrelRoll = false;
                    modelRotationZ -= MathHelper.ToRadians(360);
                    modelVelocity.X -= 100;
                    
                }
                if (stat.IsKeyDown(Keys.G) && BarrelRoll == true) //BarrelRoll
                {
                    BarrelRoll = false;
                    modelRotationZ = MathHelper.ToRadians(360);
                    modelVelocity.X += 100;

                }
                if (stat.IsKeyDown(Keys.Space) && shootTimer >= shootTime)
                {
                    shootTimer = 0;
                    shoot.LoadContent(Content, modelPosition);
                    

                }
                if (stat.IsKeyDown(Keys.E))
                {
                    for (int i = 0; i < enemies.Count(); i++)
                        enemies.RemoveAt(i);
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
            if (modelPosition.X >= screenWidth + 1000)
            {
                modelPosition.X = screenWidth + 1000;
            }
            if (modelPosition.X <= -(screenWidth + 1000))
            {
                modelPosition.X = -(screenWidth + 1000);
            }
        }


        public BoundingSphere getBoundingSphere()
        {
            BoundingSphere sphere = new BoundingSphere();

            foreach (ModelMesh mesh in myModel.Meshes)
            {
                if (sphere.Radius == 0)
                    sphere = mesh.BoundingSphere;
                else
                    sphere = BoundingSphere.CreateMerged(sphere, mesh.BoundingSphere);
            }

            sphere.Center = modelPosition;

            sphere.Radius *= 0.5f;
            return sphere;
        }




        public void Draw(Matrix proj,Matrix view)
        {
            shoot.Draw(proj, view);
            DrawModel(myModel,proj,view); 
        }

        private void DrawModel(Model myModel,Matrix Projection, Matrix View)
        {
            foreach (ModelMesh mesh in myModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = Matrix.Identity * Matrix.CreateRotationY(MathHelper.ToRadians(180)) * Matrix.CreateRotationX(modelRotationX)
                        * Matrix.CreateRotationZ(modelRotationZ) * Matrix.CreateTranslation(modelPosition);
                    effect.View = View;
                    effect.Projection = Projection;
                }
                mesh.Draw();
            }

        }
    }
}
