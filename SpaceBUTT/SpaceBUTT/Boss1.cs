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

    public class Boss1
    {
        public Model Boss1Model;
        public Vector3 Boss1Pos;
        public int Boss1Speed;
        public float BossRotation = 0;
        public Spawn shoot3 = new Spawn();
        public float bosslife = 100.0f;
        int shootTime = 10;
        int shootTimer = 0;

        public float getBossLife(){
            return bosslife;       
        }
        public void setBosslife(float bosslife) {
            this.bosslife = bosslife;
        }

        public Boss1() 
        { 
        }

        public Boss1(Model m, Vector3 ePos)
        {
            Boss1Model = m;
            Boss1Pos = ePos;
        }


        public void Update(GameTime gameTime , ContentManager Content, Vector3 PlayerPos)
        {
            if (Boss1Pos.Z <= -5000)
            {
                Boss1Pos.Z += 50;

            }
            BossRotation += 0.01f;

            if (shootTimer >= shootTime)
            {
                shootTimer = 0;
                Shoot(Content, Boss1Pos, PlayerPos);

            }
            shoot3.Update(gameTime, Content, PlayerPos);
            shootTimer++; 
        }

        public void Shoot(ContentManager Content, Vector3 Boss1Pos, Vector3 PlayerPos)
        {
            shoot3.Boss1Laser(Content, Boss1Pos, PlayerPos);
        }


        public BoundingSphere getBoundingSphere()
        {
            BoundingSphere sphere = new BoundingSphere();

            foreach (ModelMesh mesh in Boss1Model.Meshes)
            {
                if (sphere.Radius == 0)
                    sphere = mesh.BoundingSphere;
                else
                    sphere = BoundingSphere.CreateMerged(sphere, mesh.BoundingSphere);
            }

            sphere.Center = Boss1Pos;

            sphere.Radius *= 50.0f;
            return sphere;
        }

        public void Draw(Matrix Projection, Matrix View)
        {
            shoot3.Draw(Projection, View);
            DrawModel(Projection, View);
        }

        public void DrawModel(Matrix Projection, Matrix View)
        {
            foreach (ModelMesh mesh in Boss1Model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {

                    effect.EnableDefaultLighting();
                    effect.World = Matrix.Identity * Matrix.CreateRotationY(MathHelper.ToRadians(270))* Matrix.CreateRotationZ(BossRotation)
                        * Matrix.CreateScale(20) * Matrix.CreateTranslation(Boss1Pos);
                    effect.View = View;
                    effect.Projection = Projection;
                }
                mesh.Draw();
            }
        }
    }
}
