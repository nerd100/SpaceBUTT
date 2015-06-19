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
    public class EnemyLaser
    {
        public Model Laser;
        public Vector3 EnemyLaserPos;

        private int EnemyLaserSpeed=200;
        
        public Vector3 PlayerPosition;


        public EnemyLaser(Model m, Vector3 ePos, Vector3 PlayerPos)
        {
            EnemyLaserPos = ePos;
            Laser = m;
            PlayerPosition = PlayerPos;
        }

        public void Update(GameTime gameTime)
        {
            
            float x = PlayerPosition.X - EnemyLaserPos.X;
            float y = PlayerPosition.Y - EnemyLaserPos.Y;
            float z = PlayerPosition.Z - EnemyLaserPos.Z;
            
            float u =(float) Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2)+ Math.Pow(z, 2));

            if (z / u <= 0)
            {
                z = 10000;
              
            }

            EnemyLaserPos.X += (x / u) * EnemyLaserSpeed;
            EnemyLaserPos.Y += (y / u) * EnemyLaserSpeed;
            EnemyLaserPos.Z += (z / u) * EnemyLaserSpeed;

            
          

            

            getBoundingSphere();
        }

        public BoundingSphere getBoundingSphere()
        {
            BoundingSphere sphere = new BoundingSphere();

            foreach (ModelMesh mesh in Laser.Meshes)
            {
                if (sphere.Radius == 0)
                    sphere = mesh.BoundingSphere;
                else
                    sphere = BoundingSphere.CreateMerged(sphere, mesh.BoundingSphere);
            }

            sphere.Center = EnemyLaserPos;

            sphere.Radius *= 1.0f;
            return sphere;
        }


        public void Draw(Matrix Projection, Matrix View)
        {
            foreach (ModelMesh mesh in Laser.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {

                    effect.EnableDefaultLighting();
                    effect.World = Matrix.Identity * Matrix.CreateRotationY(MathHelper.ToRadians(180)) * Matrix.CreateScale(5)
                         * Matrix.CreateTranslation(EnemyLaserPos);
                    effect.View = View;
                    effect.Projection = Projection;
                }
                mesh.Draw();
            }

        }

    }
}
