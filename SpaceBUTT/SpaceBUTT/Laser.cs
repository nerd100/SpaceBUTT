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
   public class Laser
    {
        public Model laser;     
        public Vector3 laserPos;
      
        private int laserSpeed;

       

        public Laser(Model m,Vector3 ePos)
        {
            laserPos = ePos;
            laser = m;
            laserSpeed = 500;
        }

       

        
        public void Update(GameTime gameTime)
        {

            laserPos.Z -= laserSpeed;

            getBoundingSphere();

        }

        public BoundingSphere getBoundingSphere()
        {
            BoundingSphere sphere = new BoundingSphere();

            foreach (ModelMesh mesh in laser.Meshes)
            {
                if (sphere.Radius == 0)
                    sphere = mesh.BoundingSphere;
                else
                    sphere = BoundingSphere.CreateMerged(sphere, mesh.BoundingSphere);
            }

            sphere.Center = laserPos;

            sphere.Radius *= 2.0f;
            return sphere;
        }


    public void Draw(Matrix Projection, Matrix View)
        {
            foreach (ModelMesh mesh in laser.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    
                    effect.EnableDefaultLighting();
                    effect.World = Matrix.Identity * Matrix.CreateRotationY(MathHelper.ToRadians(180)) * Matrix.CreateScale(5)      
                         * Matrix.CreateTranslation(laserPos);
                    effect.View = View;
                    effect.Projection = Projection;
                }
                mesh.Draw();
            }

        }

    }
}
