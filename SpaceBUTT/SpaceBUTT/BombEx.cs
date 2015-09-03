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
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class BombEx 
    {
        public Model bombex;
        public Vector3 bombexPos;


        public BombEx(Model m,Vector3 ePos)
        {
            bombexPos = ePos;
            bombex = m;
           
       
        }

        public  void Update(GameTime gameTime)
        {
            bombexPos.Z -= 200;
            getBoundingSphere();
        }

        public BoundingSphere getBoundingSphere()
        {
            BoundingSphere sphere = new BoundingSphere();
            foreach (ModelMesh mesh in bombex.Meshes)
            {
                if (sphere.Radius == 0)
                    sphere = mesh.BoundingSphere;
                else
                    sphere = BoundingSphere.CreateMerged(sphere, mesh.BoundingSphere);
            }
            sphere.Center = bombexPos;
            sphere.Radius *= 5;
            return sphere;
        }

        public void Draw(Matrix Projection, Matrix View)
        {
            foreach (ModelMesh mesh in bombex.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    
                    effect.EnableDefaultLighting();
                    effect.World = Matrix.Identity * Matrix.CreateRotationY(MathHelper.ToRadians(90))*Matrix.CreateScale(10)
                         * Matrix.CreateTranslation(bombexPos);
                    effect.View = View;
                    effect.Projection = Projection;
                }
                mesh.Draw();
            }

        }
    }
}
