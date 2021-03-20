﻿using App05MonoGame.Helpers;
using App05MonoGame.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace App05MonoGame
{
    public class App05Game : Game
    {
        // Constants

        public const int HD_Height = 720;
        public const int HD_Width = 1280;

        // Variables

        private GraphicsDeviceManager graphicsManager;
        private GraphicsDevice graphicsDevice;
        private SpriteBatch spriteBatch;

        private Texture2D backgroundImage;
        private Sprite playerSprite;
        public App05Game()
        {
            graphicsManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        /// <summary>
        /// Setup the game window size
        /// </summary>
        protected override void Initialize()
        {
            graphicsManager.PreferredBackBufferWidth = HD_Width;
            graphicsManager.PreferredBackBufferHeight = HD_Height;

            graphicsManager.ApplyChanges();

            graphicsDevice = graphicsManager.GraphicsDevice;

            base.Initialize();
        }

        /// <summary>
        /// use this.Content to load your game content here
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            backgroundImage = Content.Load<Texture2D>("backgrounds/green_background720p");
            Texture2D sheet = Content.Load<Texture2D>("Actors/sprite-sheet1");
            
            SpriteSheetHelper helper = new SpriteSheetHelper(
                graphicsDevice, sheet, 4, 3);

            playerSprite = new Sprite(helper.FirstFrame, 100, 500);
            playerSprite.Scale = 2.0f;
        }

        /// <summary>
        /// Called 60 frames/per second and updates the positions
        /// of all the drawable objects
        /// </summary>
        /// <param name="gameTime">
        /// Can work out the elapsed time since last call
        /// </param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || 
                Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();

            // TODO: Add your update logic here

            playerSprite.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// Called 60 frames/per second and Draw all the 
        /// sprites and other drawable images here
        /// </summary>
        /// <param name="gameTime">
        /// Can work out the elapsed time since last call
        /// </param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LawnGreen);

            spriteBatch.Begin();

            spriteBatch.Draw(backgroundImage, Vector2.Zero, Color.White);
            playerSprite.Draw(spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
