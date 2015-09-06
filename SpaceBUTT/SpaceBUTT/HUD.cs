using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;



namespace SpaceBUTT
{
    public class HUD
    {
        enum GameState
        {
            StartMenu,
            Loading,
            Playing,
            Paused
        }
        public int screenWidth, screenHeight;
        public SpriteFont playerScoreFont, playerTimeFont;
        public Vector2 playerScorePos, playerTimePos, framePos;
        public bool showHud;

        int EnemieCount;

        bool spawnBoss = false;

        Texture2D healthbar;
        Vector2 position;
        Vector2 positionBoss;
        public Rectangle rectangle;
        public Rectangle rectangleBoss;


        int frameRate = 0;
        int frameCounter = 0;
        TimeSpan elapsedTime;

        float playerX = 0;
        float playerY = 0;

        //Constructor
        public HUD()
        {

            showHud = true;
            screenHeight = 0;
            screenWidth = 100;
            playerScoreFont = null;
            playerTimeFont = null;
            playerScorePos = new Vector2(10, 30);
            playerTimePos = new Vector2(10, 50);



        }

        //Load Content
        public void LoadContent(ContentManager Content)
        {
            playerScoreFont = Content.Load<SpriteFont>("UI/georgia");
            healthbar = Content.Load<Texture2D>("UI/Healthbar4");
            position = new Vector2(250, 10);
            positionBoss = new Vector2(50, 550);
            rectangle = new Rectangle(0, 0, 300, healthbar.Height);
            rectangleBoss = new Rectangle(0, 0, 0, healthbar.Height);
        }

        //Update
        public void Update(GameTime gameTime, Vector3 playerPos, int EnemieCounter, bool spawnBoss)
        {
            //Get keyboard state
            KeyboardState keyState = Keyboard.GetState();

            EnemieCount = EnemieCounter;
            playerX = playerPos.X;
            playerY = playerPos.Y;

            elapsedTime += gameTime.ElapsedGameTime;

            if (elapsedTime > TimeSpan.FromSeconds(1))
            {
                elapsedTime -= TimeSpan.FromSeconds(1);
                frameRate = frameCounter;
                frameCounter = 0;
            }

            this.spawnBoss = spawnBoss;


        }


        //Draw
        public void Draw(SpriteBatch spriteBatch, int killedEnemies)
        {

            //if we are showing our HUD (if showHud == true) then display our HUD
            if (showHud)
            {
                frameCounter++;

                int fps = frameRate;

                //  spriteBatch.Begin();

                spriteBatch.DrawString(playerScoreFont, "FPS = " + fps, new Vector2(9, 12), Color.White);
                spriteBatch.DrawString(playerScoreFont, "FPS = " + fps, new Vector2(10, 13), Color.Red);
               // spriteBatch.DrawString(playerScoreFont, "PlayerX = " + playerX, playerScorePos, Color.White);
               // spriteBatch.DrawString(playerScoreFont, "PlayerY = " + playerY, playerTimePos, Color.White);
               // spriteBatch.DrawString(playerScoreFont, "Enemies = " + EnemieCount, new Vector2(10, 80), Color.White);
              //  spriteBatch.DrawString(playerScoreFont, "BarrelRoll with F or G", new Vector2(10, 100), Color.White);
               // spriteBatch.DrawString(playerScoreFont, "Screenclear with E", new Vector2(10, 120), Color.White);
               // spriteBatch.DrawString(playerScoreFont, "Press 1-3 for difficulty", new Vector2(10, 140), Color.White);
                spriteBatch.DrawString(playerScoreFont, "Killed  Enemies :" + killedEnemies, new Vector2(10, 30), Color.White);
                spriteBatch.Draw(healthbar, position, rectangle, Color.White);
                if (spawnBoss == true && rectangleBoss.Width < 700)
                {
                    rectangleBoss.Width += 2;
                }
                spriteBatch.Draw(healthbar, positionBoss, rectangleBoss, Color.Red);
                //spriteBatch.DrawString(frames, "FPS = " + playerScore, framePos, Color.Yellow);   
                //  spriteBatch.End();


            }


        }
        public void Draw2(SpriteBatch spriteBatch, int zaehler)
        {
            if (zaehler == 0)
            {
                spriteBatch.DrawString(playerScoreFont, "WASD to Move", new Vector2(350, 250), Color.White);
                spriteBatch.DrawString(playerScoreFont, "Controller: Left Stick to Move", new Vector2(340, 270), Color.White);
            }
            if (zaehler == 1)
            {
                spriteBatch.DrawString(playerScoreFont, "Press Space to Fire", new Vector2(350, 250), Color.White);
                spriteBatch.DrawString(playerScoreFont, "Controller: Press A to Fire", new Vector2(340, 270), Color.White);
            }
            if (zaehler == 2)
            {
                spriteBatch.DrawString(playerScoreFont, "Press Q or E for Barrelroll", new Vector2(330, 250), Color.White);
                spriteBatch.DrawString(playerScoreFont, "Controller: Left or Right Shoulder for Barrelroll", new Vector2(320, 270), Color.White);
            }
            if (zaehler == 3)
            {
                spriteBatch.DrawString(playerScoreFont, "Press F to clear", new Vector2(350, 250), Color.White);
                spriteBatch.DrawString(playerScoreFont, "Controller: Press X to clear", new Vector2(340, 270), Color.White);
            }
        }


    }
}