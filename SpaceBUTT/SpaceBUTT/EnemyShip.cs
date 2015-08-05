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
 
    public class EnemyShip 
    {
       public Model Enemy;
       public Vector3 EnemyPos = Vector3.Zero;
       public int EnemySpeed;
       public Spawn shoot1 = new Spawn();

       public int EnemyHealth = 2;

     
       int shootTime = 100;
       int shootTimer = 0;

        public EnemyShip(Model m, Vector3 ePos)
        {
            EnemyPos = ePos;
            Enemy = m;
            EnemySpeed = 50;
        }


        public void Update(GameTime gameTime, ContentManager Content, Vector3 PlayerPos)
        {
            if ( shootTimer >= shootTime)
            {
                shootTimer = 0;
                Shoot(Content, EnemyPos, PlayerPos);

            }

          
            shoot1.Update(gameTime, Content, PlayerPos);
            EnemyPos.Z += EnemySpeed;  
            shootTimer++;
           
        }

        public void Shoot(ContentManager Content, Vector3 EnemyPos, Vector3 PlayerPos)
        {
            shoot1.EnemyLaser(Content, EnemyPos, PlayerPos);
        }


        public BoundingSphere getBoundingSphere()
        {
            BoundingSphere sphere = new BoundingSphere();

            foreach (ModelMesh mesh in Enemy.Meshes)
            {
                if (sphere.Radius == 0)
                    sphere = mesh.BoundingSphere;
                else
                    sphere = BoundingSphere.CreateMerged(sphere, mesh.BoundingSphere);
            }

            sphere.Center = EnemyPos;

            sphere.Radius *= 0.5f;
            return sphere;
        }



        public void Draw(Matrix Projection, Matrix View)
        {
            shoot1.Draw(Projection, View);
            DrawModel(Projection, View);
        }

        public void DrawModel(Matrix Projection, Matrix View)
        {  
            foreach (ModelMesh mesh in Enemy.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {

                    effect.EnableDefaultLighting();
                    effect.World = Matrix.Identity * Matrix.CreateRotationY(MathHelper.ToRadians(270))
                        * Matrix.CreateScale(1) * Matrix.CreateTranslation(EnemyPos);
                    effect.View = View;
                    effect.Projection = Projection;
                }
                mesh.Draw();
            }
        }

    }
}
