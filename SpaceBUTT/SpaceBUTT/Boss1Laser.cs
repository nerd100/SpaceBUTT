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
    public class Boss1Laser{
        public Model Laser;
        public Vector3 Boss1LaserPos;
        bool wall;
        private int EnemyLaserSpeed=300;
      
        public Vector3 PlayerPosition;
        Random rnd = new Random();
        public int p;
        public Boss1Laser(Model m, Vector3 ePos, Vector3 PlayerPos)
        {
            Boss1LaserPos = ePos;
            Laser = m;
            PlayerPosition = PlayerPos;
            this.p = rnd.Next(-100, 100);
        }

        public Boss1Laser(Model m, Vector3 ePos,bool wall)
        {
            Boss1LaserPos = ePos;
            Laser = m;
            this.wall = wall;
        }

        public void Update(GameTime gameTime)
        {
            if (wall == true)
            {
                    Boss1LaserPos.Z += 500;
                  
            }
            else
            {
                float x = PlayerPosition.X - Boss1LaserPos.X;
                float y = PlayerPosition.Y - Boss1LaserPos.Y;
                float z = PlayerPosition.Z - Boss1LaserPos.Z;

                float u = (float)Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2) + Math.Pow(z, 2));

                if (z / u <= 0)
                {
                    z = 10000;
                }

                Boss1LaserPos.X += (x / u) * EnemyLaserSpeed + p;
                Boss1LaserPos.Y += (y / u) * EnemyLaserSpeed + p;
                Boss1LaserPos.Z += (z / u) * EnemyLaserSpeed;

            }
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

            sphere.Center = Boss1LaserPos;

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
                         * Matrix.CreateTranslation(Boss1LaserPos);
                    effect.View = View;
                    effect.Projection = Projection;
                }
                mesh.Draw();
            }

        }

    }
}
