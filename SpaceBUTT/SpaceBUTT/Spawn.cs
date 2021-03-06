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
        public List<Boss1> boss1 = new List<Boss1>();
        public List<Boss1Laser> boss1Laser = new List<Boss1Laser>();
        public List<GeschuetzLaser> geschuetzLaser= new List<GeschuetzLaser>();
        public List<BombEx> bombex = new List<BombEx>();
        public List<Geschuetz> geschuetz = new List<Geschuetz>();
        public List<Balken> balken = new List<Balken>();
        bool wall = true;

        int spawnTimer = 0;
        int spawnTime = 50;
        int spawnTimes = 10;

        Random rnd = new Random();
        Random rnd1 = new Random();
        Random rnd2 = new Random();
        Random rnd3 = new Random();

        public void Balken(ContentManager Content)
        {
            int x = rnd.Next(-2000, 2000);
            int y = rnd.Next(-2000, 2000);
            Model balkenW = Content.Load<Model>("Model/Balken1");
            balken.Add(new Balken(balkenW, new Vector3(x, y, -50000)));
        }

        public void Boss1(ContentManager Content)
        {
            Model Boss1Model = Content.Load<Model>("Model/Boss1");
            boss1.Add(new Boss1(Boss1Model, new Vector3(0, 0, -100000)));
        }

        public void Laser(ContentManager Content, Vector3 modelPos)
        {
            Model bullet = Content.Load<Model>("Model/Laser");
            laser.Add(new Laser(bullet, new Vector3(modelPos.X, modelPos.Y, modelPos.Z)));
            //   laser.Add(new Laser(bullet, new Vector3(modelPos.X + 300, modelPos.Y, modelPos.Z)));
            //  laser.Add(new Laser(bullet, new Vector3(modelPos.X - 300, modelPos.Y, modelPos.Z)));
            //  laser.Add(new Laser(bullet, new Vector3(modelPos.X, modelPos.Y + 300, modelPos.Z)));
            //  laser.Add(new Laser(bullet, new Vector3(modelPos.X, modelPos.Y - 300, modelPos.Z)));  

        }

        public void Asteroid(ContentManager Content)
        {
            Model asteroiden = Content.Load<Model>("Model/Asteroid");

            int x = rnd.Next(-10000, 10000);
            int y = rnd.Next(-10000, 10000);
            asteroid.Add(new Asteroid(asteroiden, new Vector3(x, y, -20000)));

        }

        public void BombEx(ContentManager Content, Vector3 modelPos)
        {
            Model bomb = Content.Load<Model>("Model/BombEx");
            bombex.Add(new BombEx(bomb, new Vector3(0, 0, -100)));

        }

        public void Geschuetz(ContentManager Content)
        {
            Model cannon = Content.Load<Model>("Model/Gesch�tz");
            int x = rnd2.Next(1, 3);
            if (x == 1)
            {
                geschuetz.Add(new Geschuetz(cannon, new Vector3(9500, 500, -100000)));
            }
            else
            {
                geschuetz.Add(new Geschuetz(cannon, new Vector3(-9500, 500, -100000)));
            }
        }

        public void EnemyShip(ContentManager Content)
        {
            Model enemy = Content.Load<Model>("Model/EnemyShip");

            int x = rnd.Next(-2000, 2000);
            int y = rnd.Next(-2000, 2000);
            enemies.Add(new EnemyShip(enemy, new Vector3(x, y, -20000)));

        }

        public void EnemyLaser(ContentManager Content, Vector3 EnemyPos, Vector3 PlayerPos)
        {
            Model bullet2 = Content.Load<Model>("Model/Laser");

            enemyLaser.Add(new EnemyLaser(bullet2, EnemyPos, PlayerPos));
        }

        public void GeschuetzLaser(ContentManager Content, Vector3 geschuetzPos, Vector3 PlayerPos)
        {
            Model bullet4 = Content.Load<Model>("Model/Laser");
            geschuetzLaser.Add(new GeschuetzLaser(bullet4, geschuetzPos, PlayerPos));    
        }

        public void Boss1Laser(ContentManager Content, Vector3 Boss1Pos, Vector3 PlayerPos)
        {
            Model bullet3 = Content.Load<Model>("Model/Laser");
            if (Boss1Pos.Z >= -50000)
            {
                if (spawnTimer > spawnTimes)
                {
                    spawnTimer = 0;
                    boss1Laser.Add(new Boss1Laser(bullet3, new Vector3(Boss1Pos.X, Boss1Pos.Y, Boss1Pos.Z), new Vector3(PlayerPos.X, PlayerPos.Y, PlayerPos.Z)));
                    boss1Laser.Add(new Boss1Laser(bullet3, new Vector3(Boss1Pos.X + 3000, Boss1Pos.Y, Boss1Pos.Z), new Vector3(PlayerPos.X, PlayerPos.Y, PlayerPos.Z)));
                    boss1Laser.Add(new Boss1Laser(bullet3, new Vector3(Boss1Pos.X - 3000, Boss1Pos.Y, Boss1Pos.Z), new Vector3(PlayerPos.X, PlayerPos.Y, PlayerPos.Z)));
                    boss1Laser.Add(new Boss1Laser(bullet3, new Vector3(Boss1Pos.X, Boss1Pos.Y + 3000, Boss1Pos.Z), new Vector3(PlayerPos.X, PlayerPos.Y, PlayerPos.Z)));
                    boss1Laser.Add(new Boss1Laser(bullet3, new Vector3(Boss1Pos.X, Boss1Pos.Y - 3000, Boss1Pos.Z), new Vector3(PlayerPos.X, PlayerPos.Y, PlayerPos.Z)));
                }
            }
            else if (spawnTimer > spawnTime)
            {
                int random = rnd1.Next(1, 5);
                spawnTimer = 0;
                if (random == 1)
                {
                    for (int i = 0; i < 3000; i += 300)
                    {
                        for (int j = 0; j < 3000; j += 300)
                        {
                            boss1Laser.Add(new Boss1Laser(bullet3, new Vector3(Boss1Pos.X + i, Boss1Pos.Y + j, Boss1Pos.Z), wall));
                        }
                    }
                    for (int i = -3000; i < 0; i += 300)
                    {
                        for (int j = 0; j < 3000; j += 300)
                        {
                            boss1Laser.Add(new Boss1Laser(bullet3, new Vector3(Boss1Pos.X + i, Boss1Pos.Y + j, Boss1Pos.Z), wall));
                        }
                    }
                    for (int i = -3000; i < 0; i += 300)
                    {
                        for (int j = -3000; j < 0; j += 300)
                        {
                            boss1Laser.Add(new Boss1Laser(bullet3, new Vector3(Boss1Pos.X + i, Boss1Pos.Y + j, Boss1Pos.Z), wall));
                        }
                    }
                }

                if (random == 2)
                {
                    for (int i = 0; i < 3000; i += 300)
                    {
                        for (int j = 0; j < 3000; j += 300)
                        {
                            boss1Laser.Add(new Boss1Laser(bullet3, new Vector3(Boss1Pos.X + i, Boss1Pos.Y + j, Boss1Pos.Z), wall));
                        }
                    }
                    for (int i = -3000; i < 0; i += 300)
                    {
                        for (int j = 0; j < 3000; j += 300)
                        {
                            boss1Laser.Add(new Boss1Laser(bullet3, new Vector3(Boss1Pos.X + i, Boss1Pos.Y + j, Boss1Pos.Z), wall));
                        }
                    }
                    for (int i = 0; i < 3000; i += 300)
                    {
                        for (int j = -3000; j < 0; j += 300)
                        {
                            boss1Laser.Add(new Boss1Laser(bullet3, new Vector3(Boss1Pos.X + i, Boss1Pos.Y + j, Boss1Pos.Z), wall));
                        }
                    }
                }

                if (random == 3)
                {
                    for (int i = -3000; i < 0; i += 300)
                    {
                        for (int j = 0; j < 3000; j += 300)
                        {
                            boss1Laser.Add(new Boss1Laser(bullet3, new Vector3(Boss1Pos.X + i, Boss1Pos.Y + j, Boss1Pos.Z), wall));
                        }
                    }
                    for (int i = -3000; i < 0; i += 300)
                    {
                        for (int j = -3000; j < 0; j += 300)
                        {
                            boss1Laser.Add(new Boss1Laser(bullet3, new Vector3(Boss1Pos.X + i, Boss1Pos.Y + j, Boss1Pos.Z), wall));
                        }
                    }
                    for (int i = 0; i < 3000; i += 300)
                    {
                        for (int j = -3000; j < 0; j += 300)
                        {
                            boss1Laser.Add(new Boss1Laser(bullet3, new Vector3(Boss1Pos.X + i, Boss1Pos.Y + j, Boss1Pos.Z), wall));
                        }
                    }
                }
                if (random == 4)
                {
                    for (int i = 0; i < 3000; i += 300)
                    {
                        for (int j = 0; j < 3000; j += 300)
                        {
                            boss1Laser.Add(new Boss1Laser(bullet3, new Vector3(Boss1Pos.X + i, Boss1Pos.Y + j, Boss1Pos.Z), wall));
                        }
                    }
                    for (int i = -3000; i < 0; i += 300)
                    {
                        for (int j = -3000; j < 0; j += 300)
                        {
                            boss1Laser.Add(new Boss1Laser(bullet3, new Vector3(Boss1Pos.X + i, Boss1Pos.Y + j, Boss1Pos.Z), wall));
                        }
                    }
                    for (int i = 0; i < 3000; i += 300)
                    {
                        for (int j = -3000; j < 0; j += 300)
                        {
                            boss1Laser.Add(new Boss1Laser(bullet3, new Vector3(Boss1Pos.X + i, Boss1Pos.Y + j, Boss1Pos.Z), wall));
                        }
                    }
                }
            }
        }

        public void Update(GameTime gameTime, ContentManager Content, Vector3 PlayerPos)
        {
            spawnTimer++;
            for (int i = 0; i < asteroid.Count(); i++)
            {
                asteroid[i].Update(gameTime);
            }
            for (int i = 0; i < balken.Count(); i++)
            {
                balken[i].Update(gameTime);
            }
            for (int i = 0; i < bombex.Count(); i++)
            {
                bombex[i].Update(gameTime);
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
            for (int i = 0; i < geschuetz.Count(); i++)
            {
                geschuetz[i].Update(gameTime, Content, PlayerPos);

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
            for (int j = 0; j < geschuetz.Count(); j++)
            {
                if (geschuetz[j].geschuetzPos.Z >= 5000)
                {
                    geschuetz.RemoveAt(j);
                }
            }
            for (int j = 0; j < balken.Count(); j++)
            {
                if (balken[j].balkenPos.Z >= 5000)
                {
                    balken.RemoveAt(j);
                }
            }
            for (int j = 0; j < bombex.Count(); j++)
            {
                if (bombex[j].bombexPos.Z <= -50000)
                {
                    bombex.RemoveAt(j);
                }
            }
            for (int j = 0; j < boss1Laser.Count(); j++)
            {
                if (boss1Laser[j].Boss1LaserPos.Z >= 5000)
                {
                    boss1Laser.RemoveAt(j);
                }
            }
            for (int j = 0; j < geschuetzLaser.Count(); j++)
            {
                if (geschuetzLaser[j].GeschuetzLaserPos.Z >= 5000)
                {
                    geschuetzLaser.RemoveAt(j);
                }
            }

            for (int i = 0; i < enemyLaser.Count(); i++)
            {
                enemyLaser[i].Update(gameTime);
            }

            for (int i = 0; i < boss1Laser.Count(); i++)
            {
                boss1Laser[i].Update(gameTime);
            }
            for (int i = 0; i < geschuetzLaser.Count(); i++)
            {
                geschuetzLaser[i].Update(gameTime);
            }

            for (int j = 0; j < enemyLaser.Count(); j++)
            {
                if (enemyLaser[j].EnemyLaserPos.Z >= 5000)
                {
                    enemyLaser.RemoveAt(j);
                }
            }
            for (int j = 0; j < boss1.Count(); j++)
            {
                boss1[j].Update(gameTime, Content, PlayerPos);
            }



        }

        public void Draw(Matrix Projection, Matrix View)
        {
            for (int i = 0; i < asteroid.Count(); i++)
            {
                asteroid[i].Draw(Projection, View);
            }
            for (int i = 0; i < balken.Count(); i++)
            {
                balken[i].Draw(Projection, View);
            }

            for (int i = 0; i < laser.Count(); i++)
            {
                laser[i].Draw(Projection, View);
            }
            for (int i = 0; i < bombex.Count(); i++)
            {
                // bombex[i].Draw(Projection, View);
            }

            for (int i = 0; i < enemies.Count(); i++)
            {
                enemies[i].Draw(Projection, View);
            }
            for (int i = 0; i < geschuetz.Count(); i++)
            {
                geschuetz[i].Draw(Projection, View);
            }
            for (int i = 0; i < enemyLaser.Count(); i++)
            {
                enemyLaser[i].Draw(Projection, View);
            }
            for (int i = 0; i < boss1.Count(); i++)
            {
                boss1[i].Draw(Projection, View);
            }
            for (int i = 0; i < boss1Laser.Count(); i++)
            {
                boss1Laser[i].Draw(Projection, View);
            }
            for (int i = 0; i < geschuetzLaser.Count(); i++)
            {
                geschuetzLaser[i].Draw(Projection, View);
            }

        }
    }
}
