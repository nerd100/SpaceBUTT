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
   
    class Crosshair
    {

        Model crosshair;
        public Vector3 CrossPos = new Vector3(0, 0, -5000);
        Vector3 modelVelocity = new Vector3(0, 0, 0);
        float modelRotationZ = 0.0f;
        float modelRotationX = 0.0f;
        float modelSpeed = 7.0f;

        float screenWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        float screenHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

        public void LoadContent(ContentManager Content)
        {          
            crosshair = Content.Load<Model>("Crosshair");   
        }

        public void Update(GameTime gameTime)
        {
            UpdateInput();
            CrossPos += modelVelocity;

            modelVelocity *= 0.95f;
            modelRotationX *= 0.95f;
            modelRotationZ *= 0.95f;

        }
        public void UpdateInput()
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
            }
            //screensize TODO: Was anderes überlegen!
            if (CrossPos.Y >= screenHeight + 900)
            {
                CrossPos.Y = screenHeight + 900;
            }
            if (CrossPos.Y <= -(screenHeight + 900))
            {
                CrossPos.Y = -(screenHeight + 900);
            }
            if (CrossPos.X >= screenWidth + 1000)
            {
                CrossPos.X = screenWidth + 1000;
            }
            if (CrossPos.X <= -(screenWidth + 1000))
            {
                CrossPos.X = -(screenWidth + 1000);
            }
        }

        public void Draw(Matrix proj, Matrix view)
        {
            DrawCross(crosshair,proj,view);
            
        }


        private void DrawCross(Model crosshair, Matrix Projection, Matrix View)
        {
            foreach (ModelMesh mesh in crosshair.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = Matrix.Identity * Matrix.CreateScale(3)
                        * Matrix.CreateRotationX(MathHelper.ToRadians(90)) * Matrix.CreateTranslation(CrossPos);
                    effect.View = View;
                    effect.Projection = Projection;
                }
                mesh.Draw();
            }


        }

      
    }
}
