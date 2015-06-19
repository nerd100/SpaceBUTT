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
    class Collision
    {
        public void Update(GameTime gameTime,Player player,Spawn spawn,HUD hud)
        {
            collisionCheckPlayerAsteroid(player.getBoundingSphere(),spawn,hud);

            for (int j = 0; j < player.shoot.laser.Count(); j++)
            {
                collisionCheckLaserAsteroid(player.shoot.laser[j].getBoundingSphere(), spawn, hud);
            }

            for (int j = 0; j < player.shoot.laser.Count(); j++)
            {
                collisionCheckLaserEnemy(player.shoot.laser[j].getBoundingSphere(), spawn, hud);
            }

            collisionCheckEnemyLaserPlayer(player.getBoundingSphere(), spawn, hud);
            

            collisionCheckPlayerEnemy(player.getBoundingSphere(), spawn, hud);
        }

        public bool collisionCheckPlayerAsteroid(BoundingSphere sphere,Spawn spawn, HUD hud)
        {
            for (int i = 0; i < spawn.asteroid.Count(); i++)
                if (spawn.asteroid[i].getBoundingSphere().Intersects(sphere))
                {
                    hud.rectangle.Width -= 100;
                    spawn.asteroid.RemoveAt(i);
                }
            return false;
        }

        public bool collisionCheckLaserAsteroid(BoundingSphere sphere, Spawn spawn, HUD hud)
        {
            for (int i = 0; i < spawn.asteroid.Count(); i++)
                if (spawn.asteroid[i].getBoundingSphere().Intersects(sphere))
                {
                    spawn.asteroid.RemoveAt(i);
                }
            return false;
        }

        public bool collisionCheckLaserEnemy(BoundingSphere sphere, Spawn spawn, HUD hud)
        {
            for (int i = 0; i < spawn.enemies.Count(); i++)
                if (spawn.enemies[i].getBoundingSphere().Intersects(sphere))
                {
                    spawn.enemies.RemoveAt(i);
                }
            return false;
        }

        public bool collisionCheckPlayerEnemy(BoundingSphere sphere, Spawn spawn, HUD hud)
        {
            for (int i = 0; i < spawn.enemies.Count(); i++)
                if (spawn.enemies[i].getBoundingSphere().Intersects(sphere))
                {
                    hud.rectangle.Width -= 10;
                    spawn.enemies.RemoveAt(i);
                }
            return false;
        }

        public bool collisionCheckEnemyLaserPlayer(BoundingSphere sphere, Spawn spawn, HUD hud)
        {
            for (int i = 0; i < spawn.enemies.Count(); i++)
                for (int j = 0; j < spawn.enemies[i].shoot1.enemyLaser.Count();j++ )
                    if (spawn.enemies[i].shoot1.enemyLaser[j].getBoundingSphere().Intersects(sphere))
                    {
                        hud.rectangle.Width -= 10;                       
                        spawn.enemies[i].shoot1.enemyLaser.RemoveAt(j);
                    }
            return false;
        }
    }
}
