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
        public List<Asteroid> asteroid = new List<Asteroid>();
        public List<EnemyShip> enemies = new List<EnemyShip>();
        public List<Laser> laser = new List<Laser>();
        public List<EnemyLaser> enemyLaser = new List<EnemyLaser>();
        Random rnd = new Random();


        public void LoadContent(ContentManager Content, Vector3 modelPos)
        {
            Model bullet = Content.Load<Model>("Laser");

            laser.Add(new Laser(bullet, modelPos));
        }


        public void LoadContent1(ContentManager Content)
        {           
            Model asteroiden = Content.Load<Model>("Asteroid");
                                          
            int x = rnd.Next(-2000, 2000);
            int y = rnd.Next(-2000, 2000);
            asteroid.Add(new Asteroid(asteroiden, new Vector3(x, y, -20000)));

        }

        public void LoadContent2(ContentManager Content)
        {
            Model enemy = Content.Load<Model>("EnemyShip");

            int x = rnd.Next(-2000, 2000);
            int y = rnd.Next(-2000, 2000);
            enemies.Add(new EnemyShip(enemy, new Vector3(x, y, -20000)));

        }

        public void LoadContent3(ContentManager Content, Vector3 EnemyPos, Vector3 PlayerPos)
        {
            Model bullet2 = Content.Load<Model>("Laser");

            enemyLaser.Add(new EnemyLaser(bullet2, EnemyPos, PlayerPos));
        }


        public void Update(GameTime gameTime, ContentManager Content, Vector3 PlayerPos)
        {
            
            for (int i = 0; i < asteroid.Count(); i++)
            {
                asteroid[i].Update(gameTime);
            }

            for (int j = 0; j < asteroid.Count(); j++) 
            {
                if (asteroid[j].asteroidPos.Z >= 5000) 
                {
                    asteroid.RemoveAt(j);
                }
            }

            for (int i = 0; i < enemies.Count(); i++)
            {
                enemies[i].Update(gameTime, Content, PlayerPos);
               
            }

            for (int j = 0; j < enemies.Count(); j++)
            {
                if (enemies[j].EnemyPos.Z >= 5000)
                {
                    enemies.RemoveAt(j);
                }
            }
            //Update Laser
            for (int i = 0; i < laser.Count(); i++)
            {
                laser[i].Update(gameTime);
            }
            //delete Laser
            for (int j = 0; j < laser.Count(); j++)
            {
                if (laser[j].laserPos.Z <= -50000)
                {
                    laser.RemoveAt(j);
                }
            }

            for (int i = 0; i < enemyLaser.Count(); i++)
            {
                enemyLaser[i].Update(gameTime);
            }

            for (int j = 0; j < enemyLaser.Count(); j++)
            {
                if (enemyLaser[j].EnemyLaserPos.Z >= 5000)
                {
                    enemyLaser.RemoveAt(j);
                }
            }


        }
       
        public void Draw(Matrix Projection, Matrix View)
        {
            for (int i = 0; i < asteroid.Count(); i++)
            {
                asteroid[i].Draw(Projection, View);
            }

            for (int i = 0; i < laser.Count(); i++)
            {
                laser[i].Draw(Projection, View);
            }

            for (int i = 0; i < enemies.Count(); i++)
            {
                enemies[i].Draw(Projection, View);
            }
            for (int i = 0; i < enemyLaser.Count(); i++)
            {
                enemyLaser[i].Draw(Projection, View);
            }

        }
    }
}
