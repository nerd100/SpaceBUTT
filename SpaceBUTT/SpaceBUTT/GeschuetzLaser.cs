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
    
    public class GeschuetzLaser 
    {

        public Model Laser;
        public Vector3 GeschuetzLaserPos;
      
        private int EnemyLaserSpeed=600;
      
        public Vector3 PlayerPosition;
        Random rnd = new Random();
        public int p;
         public GeschuetzLaser(Model m, Vector3 ePos, Vector3 PlayerPos)
        {
            GeschuetzLaserPos = ePos;
            Laser = m;
            PlayerPosition = PlayerPos;
            this.p = rnd.Next(-100, +100);
        }
        public void Update(GameTime gameTime)
        {
            
                float x = PlayerPosition.X - GeschuetzLaserPos.X;
                float y = PlayerPosition.Y - GeschuetzLaserPos.Y;
                float z = PlayerPosition.Z - GeschuetzLaserPos.Z;

                float u = (float)Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2) + Math.Pow(z, 2));

                if (z / u <= 0)
                {
                    z = 10000;
                }

                GeschuetzLaserPos.X += (x / u) * EnemyLaserSpeed;
                GeschuetzLaserPos.Y += (y / u) * EnemyLaserSpeed;
                GeschuetzLaserPos.Z += (z / u) * EnemyLaserSpeed;

         
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

            sphere.Center = GeschuetzLaserPos;

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
                         * Matrix.CreateTranslation(GeschuetzLaserPos);
                    effect.View = View;
                    effect.Projection = Projection;
                }
                mesh.Draw();
            }

        }
    }
}
