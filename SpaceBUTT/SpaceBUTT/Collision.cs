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
    class Collision : Microsoft.Xna.Framework.Game
    {
        public int killedEnemies;
      //  Boss1 boss1 = new Boss1();

        public void Update(GameTime gameTime, Player player, Spawn spawn, HUD hud, int killedEnemies, Boss1 boss1)
        {
            this.killedEnemies = killedEnemies;
            
            collisionCheckPlayerAsteroid(player.getBoundingSphere(), spawn, hud,player);
            collisionCheckPlayerBalken(player.getBoundingSphere(), spawn, hud, player);

            for (int j = 0; j < player.shoot.laser.Count(); j++)
            {
                collisionCheckLaserAsteroid(player.shoot.laser[j].getBoundingSphere(), spawn, hud);
            }

            for (int j = 0; j < player.shoot.laser.Count(); j++)
            {
                collisionCheckLaserEnemy(player.shoot.laser[j].getBoundingSphere(), spawn, hud);
            }

            for (int j = 0; j < player.shoot.laser.Count(); j++)
            {
                collisionCheckLaserBoss1(player.shoot.laser[j].getBoundingSphere(), spawn, hud,boss1);
            }
            collisionCheckBossLaserPlayer(player.getBoundingSphere(), spawn, hud, player);

            collisionCheckEnemyLaserPlayer(player.getBoundingSphere(), spawn, hud, player);

            collisionCheckPlayerEnemy(player.getBoundingSphere(), spawn, hud, player);
            for (int j = 0; j < player.bomb.bombex.Count(); j++)
            {
                collisionCheckBombenemies(player.bomb.bombex[j].getBoundingSphere(),spawn);
            }
            for (int j = 0; j < player.bomb.bombex.Count(); j++)
            {
                collisionCheckBombAsteroids(player.bomb.bombex[j].getBoundingSphere(), spawn);
            }

            collisionCheckGeschuetzLaserPlayer(player.getBoundingSphere(), spawn, hud, player);
        }


        public bool collisionCheckBombenemies(BoundingSphere sphere, Spawn spawn) 
        {
            for (int i = 0; i < spawn.enemies.Count(); i++)
                if (spawn.enemies[i].getBoundingSphere().Intersects(sphere))
                {
                    spawn.enemies.RemoveAt(i);
                }
            
            return true;
        }
        public bool collisionCheckBombAsteroids(BoundingSphere sphere, Spawn spawn)
        {
            for (int i = 0; i < spawn.asteroid.Count(); i++)
                if (spawn.asteroid[i].getBoundingSphere().Intersects(sphere))
                {
                    spawn.asteroid.RemoveAt(i);
                }

            return true;
        }

        public bool collisionCheckBossLaserPlayer(BoundingSphere sphere,Spawn spawn, HUD hud, Player player){
            for (int i = 0; i < spawn.boss1.Count(); i++)
                for (int j = 0; j < spawn.boss1[i].shoot3.boss1Laser.Count(); j++)
                    if (spawn.boss1[i].shoot3.boss1Laser[j].getBoundingSphere().Intersects(sphere))
                    {
                        player.PlayerHealth -= 5;
                        hud.rectangle.Width = (int)(300 * (player.PlayerHealth / 100));
                        spawn.boss1[i].shoot3.boss1Laser.RemoveAt(j);
                    }
            
            return true;
        }

        public bool collisionCheckPlayerAsteroid(BoundingSphere sphere, Spawn spawn, HUD hud,Player player)
        {
           
            for (int i = 0; i < spawn.asteroid.Count(); i++)
                if (spawn.asteroid[i].getBoundingSphere().Intersects(sphere))
                {
                    player.PlayerHealth -= 20;
                    hud.rectangle.Width = (int)(300*(player.PlayerHealth/100));
                    spawn.asteroid.RemoveAt(i);
                }
            return true;
        }
        public bool collisionCheckPlayerBalken(BoundingSphere sphere, Spawn spawn, HUD hud, Player player)
        {

            for (int i = 0; i < spawn.balken.Count(); i++)
                if (spawn.balken[i].getBoundingSphere().Intersects(sphere))
                {
                    player.PlayerHealth -= 20;
                    hud.rectangle.Width = (int)(300 * (player.PlayerHealth / 100));
                    spawn.balken.RemoveAt(i);
                }
            return true;
        }
        

        public bool collisionCheckLaserBoss1(BoundingSphere sphere, Spawn spawn, HUD hud,Boss1 boss1)
        {
            for (int i = 0; i < spawn.boss1.Count(); i++)
                if (spawn.boss1[i].getBoundingSphere().Intersects(sphere))
                {
                    if (boss1.BossLife <= 0.0f)
                    {
                        spawn.boss1.RemoveAt(i);
                    }
                    else
                    {
                        boss1.BossLife = boss1.BossLife - 0.1f;
                        hud.rectangleBoss.Width = (int)(700*((boss1.BossLife/100)));
                    }
                        
                }
            return true;
        }

        public bool collisionCheckLaserAsteroid(BoundingSphere sphere, Spawn spawn, HUD hud)
        {
            for (int i = 0; i < spawn.asteroid.Count(); i++)
                if (spawn.asteroid[i].getBoundingSphere().Intersects(sphere))
                {
                    spawn.asteroid.RemoveAt(i);
                }
            return true;
        }

        public bool collisionCheckLaserEnemy(BoundingSphere sphere, Spawn spawn, HUD hud)
        {
            for (int i = 0; i < spawn.enemies.Count(); i++)
                if (spawn.enemies[i].getBoundingSphere().Intersects(sphere))
                {  
                        spawn.enemies.RemoveAt(i);
                        killedEnemies++;
                                
                }
            return true;
        }

        public bool collisionCheckPlayerEnemy(BoundingSphere sphere, Spawn spawn, HUD hud, Player player)
        {
            for (int i = 0; i < spawn.enemies.Count(); i++)
                if (spawn.enemies[i].getBoundingSphere().Intersects(sphere))
                {
                    player.PlayerHealth -= 10;
                    hud.rectangle.Width = (int)(300 * (player.PlayerHealth / 100));
                    spawn.enemies.RemoveAt(i);
                }
            return true;
        }

        public bool collisionCheckEnemyLaserPlayer(BoundingSphere sphere, Spawn spawn, HUD hud,Player player)
        {
            for (int i = 0; i < spawn.enemies.Count(); i++)
                for (int j = 0; j < spawn.enemies[i].shoot1.enemyLaser.Count();j++ )
                    if (spawn.enemies[i].shoot1.enemyLaser[j].getBoundingSphere().Intersects(sphere))
                    {
                        player.PlayerHealth -= 5;
                        hud.rectangle.Width = (int)(300 * (player.PlayerHealth / 100));                       
                        spawn.enemies[i].shoot1.enemyLaser.RemoveAt(j);
                    }
            return true;
        }
        public bool collisionCheckGeschuetzLaserPlayer(BoundingSphere sphere, Spawn spawn, HUD hud, Player player)
        {
            for (int i = 0; i < spawn.geschuetz.Count(); i++)
                for (int j = 0; j < spawn.geschuetz[i].shoot2.geschuetzLaser.Count(); j++)
                    if (spawn.geschuetz[i].shoot2.geschuetzLaser[j].getBoundingSphere().Intersects(sphere))
                    {
                        player.PlayerHealth -= 5;
                        hud.rectangle.Width = (int)(300 * (player.PlayerHealth / 100));
                        spawn.geschuetz[i].shoot2.geschuetzLaser.RemoveAt(j);
                    }
            return true;
        }

        public int IncrementkilledEnemie() {
            
            return killedEnemies;
        }
    }
}
