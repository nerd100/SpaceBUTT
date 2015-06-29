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
    public class Asteroid
    {
        public Model asteroid;
        public Matrix world = Matrix.Identity;
        public float asteroidRotationX;       
        public Vector3 asteroidPos;
        
        Random rnd = new Random();
        private int scale;
        private int streuung;
        private int asteroidSpeed;

        public Asteroid(Model m,Vector3 ePos)
        {
            asteroidPos = ePos;
            asteroid = m;
            scale = rnd.Next(3,6);
            streuung = rnd.Next(-5,5);
            asteroidSpeed = rnd.Next(10, 100);
        }

        
        public void Update(GameTime gameTime)
        {
            asteroidRotationX += 0.01f;
            asteroidPos.Z += asteroidSpeed;
            asteroidPos.X += streuung;
            asteroidPos.Y += streuung;

            getBoundingSphere();
        }

        public BoundingSphere getBoundingSphere()
        {
            BoundingSphere sphere = new BoundingSphere();

            foreach (ModelMesh mesh in asteroid.Meshes)
            {
                if (sphere.Radius == 0)
                    sphere = mesh.BoundingSphere;
                else
                    sphere = BoundingSphere.CreateMerged(sphere, mesh.BoundingSphere);
            }

            sphere.Center = asteroidPos;

            sphere.Radius *= 2;
            return sphere;
        }
       

    public void Draw(Matrix Projection, Matrix View)
        {
            foreach (ModelMesh mesh in asteroid.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    
                    effect.EnableDefaultLighting();
                    effect.World = Matrix.Identity * Matrix.CreateRotationY(MathHelper.ToRadians(180))
                        * Matrix.CreateScale(scale) * Matrix.CreateRotationX(asteroidRotationX)
                         * Matrix.CreateTranslation(asteroidPos);
                    effect.View = View;
                    effect.Projection = Projection;
                }
                mesh.Draw();
            }

        }

    }
}
