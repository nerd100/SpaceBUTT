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
   
    class Crosshair
    {

        Model crosshair;
        Vector3 CrossPos = new Vector3(0, -5, 0);
   


        public void LoadContent(ContentManager Content)
        {          
            crosshair = Content.Load<Model>("Crosshair");   
        }

     
        public void Update()
        {      
           
        }

        public void Draw(Matrix proj, Matrix view)
        {
            DrawCross(crosshair,proj,view);
            
        }


        private void DrawCross(Model crosshair, Matrix Projection, Matrix View)
        {
            foreach (ModelMesh mesh in crosshair.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = Matrix.Identity * Matrix.CreateTranslation(CrossPos) * Matrix.CreateScale(5)
                        * Matrix.CreateRotationX(MathHelper.ToRadians(90));
                    effect.View = View;
                    effect.Projection = Projection;
                }
                mesh.Draw();
            }


        }

      
    }
}
