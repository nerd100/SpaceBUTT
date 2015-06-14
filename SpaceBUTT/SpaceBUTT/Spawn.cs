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
    public class Spawn
    {
        public List<Asteroid> enemies = new List<Asteroid>();
        Random rnd = new Random();
        
        public void LoadContent(ContentManager Content)
        {           
            Model asteroid = Content.Load<Model>("Asteroid");
                                          
            int x = rnd.Next(-2000, 2000);
            int y = rnd.Next(-2000, 2000);
            enemies.Add(new Asteroid(asteroid, new Vector3(x, y, -20000)));
        }
        

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < enemies.Count(); i++)
            {
                enemies[i].Update(gameTime);
            }

            for (int j = 0; j < enemies.Count(); j++) 
            {
                if (enemies[j].asteroidPos.Z >= 5000) 
                {
                    enemies.RemoveAt(j);
                }
            }
        }
       
        public void Draw(Matrix Projection, Matrix View)
        {         
            for (int i = 0; i < enemies.Count(); i++)
            {
                enemies[i].Draw(Projection, View);
            }
        }
    }
}
