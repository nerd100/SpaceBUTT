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
    public class Shoot 
    {
        
        public List<Laser> laser = new List<Laser>();
       

        public void LoadContent(ContentManager Content,Vector3 modelPos)
        {
            Model bullet = Content.Load<Model>("Laser");
        
            laser.Add(new Laser(bullet,modelPos));
        
        }


        public void Update(GameTime gameTime)
        {
            
             
            for (int i = 0; i < laser.Count(); i++)
            {
                laser[i].Update(gameTime);
            }

            for (int j = 0; j < laser.Count(); j++)
            {
                if (laser[j].laserPos.Z <= -50000)
                {
                    laser.RemoveAt(j);
                }
            }
        }

        public void Draw(Matrix Projection, Matrix View)
        {
            for (int i = 0; i < laser.Count(); i++)
            {
                laser[i].Draw(Projection, View);
            }
        }
    }
}