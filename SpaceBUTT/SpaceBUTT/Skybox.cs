/*using System;
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
    public class Skybox 
    {
        Skybox skybox;
        float angle = 0;
        float distance = 20;
        
       public void LoadContent(ContentManager Content)
       {

           skybox = new Skybox("skybox", Content);

          
       }
  

        protected override void Update(GameTime gameTime)
        {  

            
            
        }
        public void Draw(GameTime gameTime, Matrix view, Matrix projection, GraphicsDevice graphics)
        {
           

            RasterizerState originalRasterizerState = graphics.GraphicsDevice.RasterizerState;
            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            graphics.GraphicsDevice.RasterizerState = rasterizerState;

            skybox.Draw(view, projection, cameraPosition);

            graphics.GraphicsDevice.RasterizerState = originalRasterizerState;

            DrawModel(model,  view, projection);

           
        }

        private void DrawModel(Model model, Matrix view, Matrix projection)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;                  
                    effect.View = view;
                    effect.Projection = projection;
                }
                mesh.Draw();
            }
        }

           
     }


    }
*/