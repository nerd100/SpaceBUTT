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
   
    public class Balken 
    {
        Model balken;
        public Vector3 balkenPos;
        int balkenSpeed = 500;
        float balkenRot;

        public Balken(Model m,Vector3 ePos)
        {
            balkenPos = ePos;
            balken = m;

        }

      
        public  void Update(GameTime gameTime)
        {
            balkenPos.Z += balkenSpeed;
            getBoundingSphere();
            balkenRot += 0.1f;
        }

        public void Draw(Matrix Projection, Matrix View)
        {
            foreach (ModelMesh mesh in balken.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {

                    effect.EnableDefaultLighting();
                    effect.World = Matrix.Identity * Matrix.CreateScale(5) * Matrix.CreateRotationX(balkenRot) * Matrix.CreateRotationY(balkenRot) * Matrix.CreateRotationZ(balkenRot)
                        * Matrix.CreateTranslation(balkenPos);
                    effect.View = View;
                    effect.Projection = Projection;
                }
                mesh.Draw();
            }
        }
        public BoundingSphere getBoundingSphere()
        {
            BoundingSphere sphere = new BoundingSphere();

            foreach (ModelMesh mesh in balken.Meshes)
            {
                if (sphere.Radius == 0)
                    sphere = mesh.BoundingSphere;
                else
                    sphere = BoundingSphere.CreateMerged(sphere, mesh.BoundingSphere);
            }

            sphere.Center = new Vector3(balkenPos.X,balkenPos.Y,balkenPos.Z);
            
            sphere.Radius *= 1f;
            return sphere;
        }
    }
}
