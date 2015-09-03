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
        public Spawn bomb = new Spawn();
        public Vector3 PlayerPosition = new Vector3(0, 0, 0);

        int shootTime = 10;
        int shootTimer = 0;

        Vector3 modelVelocity = new Vector3(0, 0, 0);
        float modelRotationZ = 0.0f;
        float modelRotationX = 0.0f;
        float modelSpeed = 7.0f;

        bool BarrelRoll = true;
        int BarrelRollTimer = 0;
        int BarrelRollTime = 120;

        float screenWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        float screenHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

        private float _PlayerHealth = 100.0f;
        public float PlayerHealth{
            get { return _PlayerHealth; }
            set { _PlayerHealth = value; }
        }



        public void LoadContent(ContentManager Content)
        {   
            myModel = Content.Load<Model>("Model/Ship1");            
        }


        public void Update(GameTime gameTime, ContentManager Content, List<Asteroid> asteroids, List<EnemyShip> enemies)
        {
            KeyboardState stat = Keyboard.GetState();

            if (BarrelRollTimer >= BarrelRollTime)
            {
                BarrelRollTimer = 0;
                BarrelRoll = true; 
            }


            PlayerPosition += modelVelocity;

            UpdateInput(Content,asteroids,enemies);

            modelVelocity *= 0.95f;
            modelRotationX *= 0.95f;
            modelRotationZ *= 0.95f;

            getBoundingSphere();
            shoot.Update(gameTime, Content, PlayerPosition);
            bomb.Update(gameTime, Content, PlayerPosition);
            shootTimer++;

           
            BarrelRollTimer++;

        }

        public void UpdateInput(ContentManager Content,List<Asteroid> asteroids, List<EnemyShip>enemies)
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
                
                 if (currentState.IsButtonDown(Buttons.RightShoulder) && BarrelRoll == true)
                 {
                     BarrelRoll = false;
                     modelRotationZ = MathHelper.ToRadians(360);
                     modelVelocity.X += 100;   
                 }
                 if (currentState.IsButtonDown(Buttons.LeftShoulder) && BarrelRoll == true)
                 {
                     BarrelRoll = false;
                     modelRotationZ -= MathHelper.ToRadians(360);
                     modelVelocity.X -= 100;
                 }
                 if (currentState.IsButtonDown(Buttons.A) && shootTimer >= shootTime)
                 {
                     shootTimer = 0;
                     shoot.Laser(Content, PlayerPosition);

                 }

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
                if (stat.IsKeyDown(Keys.Q) && BarrelRoll == true) //BarrelRoll
                {
                    BarrelRoll = false;
                    modelRotationZ -= MathHelper.ToRadians(360);
                    modelVelocity.X -= 100;
                    
                }
                if (stat.IsKeyDown(Keys.E) && BarrelRoll == true) //BarrelRoll
                {
                    BarrelRoll = false;
                    modelRotationZ = MathHelper.ToRadians(360);
                    modelVelocity.X += 100;

                }
                if (stat.IsKeyDown(Keys.Space) && shootTimer >= shootTime)
                {
                    shootTimer = 0;
                    shoot.Laser(Content, PlayerPosition);
                   

                }
                if (stat.IsKeyDown(Keys.F))
                {
                    bomb.BombEx(Content,PlayerPosition);
                   // for (int i = 0; i < asteroids.Count(); i++)
                   //     asteroids.RemoveAt(i);                    
                   // for (int i = 0; i < enemies.Count(); i++)
                    //        enemies.RemoveAt(i);
                }
                
            }
            //screensize TODO: Was anderes überlegen!
            if (PlayerPosition.Y >= screenHeight + 900)
            {
                PlayerPosition.Y = screenHeight + 900;
            }
            if (PlayerPosition.Y <= -(screenHeight + 900))
            {
                PlayerPosition.Y = -(screenHeight + 900);
            }
            if (PlayerPosition.X >= screenWidth + 1000)
            {
                PlayerPosition.X = screenWidth + 1000;
            }
            if (PlayerPosition.X <= -(screenWidth + 1000))
            {
                PlayerPosition.X = -(screenWidth + 1000);
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

            sphere.Center = PlayerPosition;

            sphere.Radius *= 0.5f;
            return sphere;
        }




        public void Draw(Matrix proj,Matrix view)
        {
            shoot.Draw(proj, view);
            bomb.Draw(proj, view);
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
                        * Matrix.CreateRotationZ(modelRotationZ) * Matrix.CreateTranslation(PlayerPosition);
                    effect.View = View;
                    effect.Projection = Projection;
                }
                mesh.Draw();
            }

        }
    }
}
