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
using System.Threading;

namespace SpaceBUTT
{
  
    public class Menu
       {
        enum GameState
        {
            StartMenu,
            Loading,
            Playing,
            Paused
        }

        SpriteBatch spriteBatch;
        public bool goPlay = false;
        private Texture2D startButton;
        private Texture2D exitButton;
        private Texture2D pauseButton;
        private Texture2D resumeButton;
        private Texture2D loadingScreen; 
        private Vector2 startButtonPosition;
        private Vector2 exitButtonPosition;
        private Vector2 resumeButtonPosition;
        private GameState gameState;
        private bool isLoading = false;

        MouseState mouseState;
        MouseState previousMouseState;

       
       
       public void Initialize(bool IsMouseVisible,int width)
        {
            //enable the mousepointer
           // IsMouseVisible = true;

            ////set the position of the buttons
            startButtonPosition = new Vector2((width / 2) - 50, 200);
            exitButtonPosition = new Vector2((width / 2) - 50, 250);

            //set the gamestate to start menu
            gameState = GameState.StartMenu;

            //get the mouse state
            mouseState = Mouse.GetState();
            previousMouseState = mouseState;

            
          
        }

       
       public void LoadContent(SpriteBatch spriteBatch, ContentManager Content)
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            this.spriteBatch = spriteBatch;

            //load the buttonimages into the content pipeline
            startButton = Content.Load<Texture2D>(@"start");
            exitButton = Content.Load<Texture2D>(@"exit");

            //load the loading screen
            loadingScreen = Content.Load<Texture2D>(@"loading");
            pauseButton = Content.Load<Texture2D>(@"pause");
            resumeButton = Content.Load<Texture2D>(@"resume");
            resumeButtonPosition = new Vector2((5 / 2) - (resumeButton.Width / 2),
                                               (5 / 2) - (resumeButton.Height / 2));
        }



       public void Update(GameTime gameTime, ContentManager Content, int width)
        {
            
            if (gameState == GameState.Loading && !isLoading) //isLoading bool is to prevent the LoadGame method from being called 60 times a seconds
            {         
                isLoading = true;
            }

            //move the orb if the game is in progress
            if (gameState == GameState.Playing)
            {
              
                //prevent out of bounds  
            }

            //wait for mouseclick
            mouseState = Mouse.GetState();
            if (previousMouseState.LeftButton == ButtonState.Pressed && 
                mouseState.LeftButton == ButtonState.Released)
            {
                MouseClicked(mouseState.X, mouseState.Y);
            }

            previousMouseState = mouseState;

            if (gameState == GameState.Playing && isLoading)
            {     
                LoadGame(Content,width);
                isLoading = false;              
            }
        }

       public bool checkPlay() {
           if (goPlay == true)
               return true;
           else return false;
       }
      
        public void Draw(GameTime gameTime,int width,int height)
        {

            spriteBatch.Begin();

            //draw the start menu
            if (gameState == GameState.StartMenu)
            {  
                spriteBatch.Draw(startButton, startButtonPosition, Color.White);
                spriteBatch.Draw(exitButton, exitButtonPosition, Color.White);
            }

            //show the loading screen when needed
            if (gameState == GameState.Loading)
            {              
                spriteBatch.Draw(loadingScreen, new Vector2((width / 2) - 
                    (loadingScreen.Width / 2), (height / 2) - (loadingScreen.Height / 2)), Color.YellowGreen);

            }

            //draw the the game when playing
            if (gameState == GameState.Playing)
            { 
                //pause button
                spriteBatch.Draw(pauseButton, new Vector2(0, 0), Color.White);
            }

            //draw the pause screen
            if (gameState == GameState.Paused)
            {
                spriteBatch.Draw(resumeButton, resumeButtonPosition, Color.White);
            }

            spriteBatch.End();

        }

        void MouseClicked(int x, int y)
        {
            //creates a rectangle of 10x10 around the place where the mouse was clicked
            Rectangle mouseClickRect = new Rectangle(x, y, 10, 10);

            //check the startmenu
            if (gameState == GameState.StartMenu)
            {
                Rectangle startButtonRect = new Rectangle((int)startButtonPosition.X, (int)startButtonPosition.Y, 100, 20);
                Rectangle exitButtonRect = new Rectangle((int)exitButtonPosition.X, (int)exitButtonPosition.Y, 100, 20);

                if (mouseClickRect.Intersects(startButtonRect)) //player clicked start button
                {
                    gameState = GameState.Loading;
                    isLoading = false;
                    
                    goPlay = true;
                }
                else if (mouseClickRect.Intersects(exitButtonRect)) //player clicked exit button
                {
                    
                }
            }

            //check the pausebutton
            if (gameState == GameState.Playing)
            {
                Rectangle pauseButtonRect = new Rectangle(0, 0, 70, 70);

                if (mouseClickRect.Intersects(pauseButtonRect))
                {
                    gameState = GameState.Paused;
                }
            }

            //check the resumebutton
            if (gameState == GameState.Paused)
            {
                Rectangle resumeButtonRect = new Rectangle((int)resumeButtonPosition.X, (int)resumeButtonPosition.Y, 100, 20);

                if (mouseClickRect.Intersects(resumeButtonRect))
                {
                    gameState = GameState.Playing;
                }
            }
        }

        void LoadGame(ContentManager Content,int width)
        {
            //load the game images into the content pipeline
            pauseButton = Content.Load<Texture2D>(@"pause");
            resumeButton = Content.Load<Texture2D>(@"resume");
            resumeButtonPosition = new Vector2((width / 2) - (resumeButton.Width / 2),
                                               (width / 2) - (resumeButton.Height / 2));

            //set the position of the orb in the middle of the gamewindow

            //since this will go to fast for this demo's purpose, wait for 3 seconds
            

            //start playing
            gameState = GameState.Playing;
            isLoading = false;
        }
    }
}