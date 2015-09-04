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

    public class Geschuetz
    {

        Model geschuetz;
        public Vector3 geschuetzPos;
        int geschuetzSpeed = 500;

        public Spawn shoot2 = new Spawn();
        int shootTime = 5;
        int shootTimer = 0;

        public Geschuetz(Model m, Vector3 ePos)
        {
            geschuetzPos = ePos;
            geschuetz = m;

        }


        public void Update(GameTime gameTime, ContentManager Content, Vector3 PlayerPos)
        {
            if (shootTimer >= shootTime)
            {
                shootTimer = 0;
                Shoot(Content, geschuetzPos, PlayerPos);

            }


            shoot2.Update(gameTime, Content, PlayerPos);
            geschuetzPos.Z += geschuetzSpeed;
            shootTimer++;

        }
        public void Shoot(ContentManager Content, Vector3 geschuetzPos, Vector3 PlayerPos)
        {
            shoot2.GeschuetzLaser(Content, geschuetzPos, PlayerPos);

        }
        public void Draw(Matrix Projection, Matrix View)
        {
            shoot2.Draw(Projection, View);
            DrawModel(Projection, View);
        }

        public void DrawModel(Matrix Projection, Matrix View)
        {
            foreach (ModelMesh mesh in geschuetz.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {

                    effect.EnableDefaultLighting();
                    effect.World = Matrix.Identity * Matrix.CreateScale(10)
                        * Matrix.CreateTranslation(geschuetzPos);
                    effect.View = View;
                    effect.Projection = Projection;
                }
                mesh.Draw();
            }
        }
    }
}
