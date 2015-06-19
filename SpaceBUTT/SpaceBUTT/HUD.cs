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
        public int screenWidth, screenHeight;
        public SpriteFont playerScoreFont, playerTimeFont;
        public Vector2 playerScorePos, playerTimePos, framePos;
        public bool showHud;

        int EnemieCount;

        Texture2D texture;
        Vector2 position;
        public Rectangle rectangle;


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
            playerScoreFont = Content.Load<SpriteFont>("georgia");
            texture = Content.Load <Texture2D>("Healthbar1");
            position = new Vector2(500, 10);
            rectangle = new Rectangle(0, 0, texture.Width, texture.Height);
        }

        //Update
        public void Update(GameTime gameTime, Vector3 playerPos, int EnemieCounter)
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
      
            


        }


        //Draw
        public void Draw(SpriteBatch spriteBatch)
        {

            //if we are showing our HUD (if showHud == true) then display our HUD
            if (showHud)
            {
                frameCounter++;

                int fps =  frameRate;

                spriteBatch.Begin();
                spriteBatch.DrawString(playerScoreFont, "FPS = " + fps, new Vector2(9, 12), Color.White);
                spriteBatch.DrawString(playerScoreFont, "FPS = "+ fps, new Vector2(10, 13), Color.Red);
                spriteBatch.DrawString(playerScoreFont, "PlayerX = " + playerX, playerScorePos, Color.White);
                spriteBatch.DrawString(playerScoreFont, "PlayerY = " + playerY, playerTimePos, Color.White);
                spriteBatch.DrawString(playerScoreFont, "Enemies = " + EnemieCount, new Vector2(10, 80), Color.White);
                spriteBatch.DrawString(playerScoreFont, "BarrelRoll with F or G" , new Vector2(10, 100), Color.White);
                spriteBatch.DrawString(playerScoreFont, "Screenclear with E " , new Vector2(10, 120), Color.White);

                spriteBatch.Draw(texture, position, rectangle,Color.White);
              
              
                //spriteBatch.DrawString(frames, "FPS = " + playerScore, framePos, Color.Yellow);   
                spriteBatch.End();


            }


        }

    }
}