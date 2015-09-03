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

    public class Station
    {
        Model station;
        Vector3 StationPosition = new Vector3(0, -5000, -300000);
        float StationSpeed = 100.0f;

        public void LoadContent(ContentManager Content)
        {
            station = Content.Load<Model>("Model/Station");
        }


        public void Update(GameTime gameTime)
        {
            StationPosition.Z += StationSpeed;
        }

        public void Draw(Matrix Projection, Matrix View)
        {
            foreach (ModelMesh mesh in station.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {

                    effect.EnableDefaultLighting();
                    effect.World = Matrix.Identity * Matrix.CreateRotationY(MathHelper.ToRadians(90)) * Matrix.CreateScale(50) * Matrix.CreateTranslation(StationPosition);
                    effect.View = View;
                    effect.Projection = Projection;
                }
                mesh.Draw();
            }

        }
    }
}
