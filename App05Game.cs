using App05MonoGame.Controllers;
using App05MonoGame.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace App05MonoGame
{
    public enum GameStates
    {
        Starting,
        PlayingLevel1,
        PlayingLevel2,
        Ending
    }

    /// <summary>
    /// This game creates a variety of sprites as an example.  
    /// There is no game to play yet. The spaceShip and the 
    /// asteroid can be used for a space shooting game, the player, 
    /// the coin and the enemy could be used for a pacman
    /// style game where the player moves around collecting
    /// random coins and the enemy tries to catch the player.
    /// </summary>
    /// <authors>
    /// Derek Peacock & Andrei Cruceru
    /// </authors>
    public class App05Game : Game
    {
        #region Constants

        public const int Game_Height = 720;
        public const int Game_Width = 1280;

        public const string GameName = "Game Name";
        public const string ModuleName = "BNU CO453 2021";
        public const string AuthorNames = "Derek & Andrei";
        public const string AppName = "App05: C# MonoGame";
        #endregion

        #region Attribute

        // Essesntial XNA objects for Graphics manipularion

        private readonly GraphicsDeviceManager graphicsManager;
        private GraphicsDevice graphicsDevice;
        private SpriteBatch spriteBatch;
        
        // Which state the game is in, starting, playing etc.

        private GameStates gameState;
        private string gameStateTitle;

        // Arial large font and calibri small font

        private SpriteFont arialFont;
        private SpriteFont calibriFont;


        private Texture2D backgroundImage;
        private SoundEffect flameEffect;

        private CoinsController coinsController;

        private PlayerSprite shipSprite;
        private Sprite asteroidSprite;

        private AnimatedPlayer playerSprite;
        private AnimatedSprite enemySprite;

        private Button restartButton;

        // Should be in the main character

        private int score;
        private int energy;

        #endregion

        /// <summary>
        /// Create a graphics manager and the root for
        /// all the game assets or content
        /// </summary>
        public App05Game()
        {
            graphicsManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        /// <summary>
        /// Setup the game window size to HD_Height x HD_Width pixels
        /// Simple fixed playing area with no camera or scrolling
        /// </summary>
        protected override void Initialize()
        {
            gameState = GameStates.Starting;
            gameStateTitle = GameName + ": Start Screen";

            graphicsManager.PreferredBackBufferWidth = Game_Width;
            graphicsManager.PreferredBackBufferHeight = Game_Height;

            graphicsManager.ApplyChanges();

            graphicsDevice = graphicsManager.GraphicsDevice;

            score = 0;
            energy = 100;

            base.Initialize();
        }

        /// <summary>
        /// use Content to load your game images, fonts,
        /// music and sound effects
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            backgroundImage = Content.Load<Texture2D>(
                "backgrounds/green_background720p");

            // Load Music and SoundEffects

            SoundController.LoadContent(Content);
            SoundController.PlaySong("Adventure");
            flameEffect = SoundController.GetSoundEffect("Flame");

            // Load Fonts

            arialFont = Content.Load<SpriteFont>("fonts/arial");
            calibriFont = Content.Load<SpriteFont>("fonts/calibri");

            restartButton = new Button(arialFont,
                Content.Load<Texture2D>("Controls/button-icon-png-200"))
            {
                Position = new Vector2(1100, 600),
                Text = "Quit",
                Scale = 0.6f
            };

            restartButton.click += RestartButton_click;

            // suitable for asteroids type game

            SetupSpaceShip();
            SetupAsteroid();

            // animated sprites suitable for pacman type game

            SetupAnimatedPlayer();
            SetupEnemy();

            coinsController = new CoinsController();

            Texture2D coinSheet = Content.Load<Texture2D>("Actors/coin_copper");
            coinsController.CreateCoin(graphicsDevice, coinSheet);
        }

        private void RestartButton_click(object sender, System.EventArgs e)
        {
            //TODO: do something when the button is clicked!
            
            Exit();
        }

        /// <summary>
        /// This is a single image sprite that rotates
        /// and move at a constant speed in a fixed direction
        /// </summary>
        private void SetupAsteroid()
        {
            Texture2D asteroid = Content.Load<Texture2D>(
               "Actors/asteroid-1");

            asteroidSprite = new Sprite(asteroid, 1200, 500)
            {
                Direction = new Vector2(-1, 0),
                Speed = 100,
                Scale = 0.2f,
                Rotation = MathHelper.ToRadians(3),
                RotationSpeed = 2f,
            };

    }

        /// <summary>
        /// This is a Sprite that can be controlled by a
        /// player using Rotate Left = A, Rotate Right = D, 
        /// Forward = Space
        /// </summary>
        private void SetupSpaceShip()
        {
            Texture2D ship = Content.Load<Texture2D>(
               "Actors/GreenShip");

            shipSprite = new PlayerSprite(ship, 200, 500)
            {
                Direction = new Vector2(1, 0),
                Speed = 200,
                DirectionControl = DirectionControl.Rotational
            };
    }


        /// <summary>
        /// This is a Sprite with four animations for the four
        /// directions, up, down, left and right
        /// </summary>
        private void SetupAnimatedPlayer()
        {
            Texture2D sheet4x3 = Content.Load<Texture2D>("Actors/rsc-sprite-sheet1");

            AnimationController contoller = new AnimationController(graphicsDevice, sheet4x3, 4, 3);

            string[] keys = new string[] { "Down", "Left", "Right", "Up" };
            contoller.CreateAnimationGroup(keys);

            playerSprite = new AnimatedPlayer()
            {
                CanWalk = true,
                Scale = 2.0f,

                Position = new Vector2(200, 200),
                Speed = 200,
                Direction = new Vector2(1, 0),

                Rotation = MathHelper.ToRadians(0),
                RotationSpeed = 0f
            };

            contoller.AppendAnimationsTo(playerSprite);
        }

        /// <summary>
        /// This is an enemy Sprite with four animations for the four
        /// directions, up, down, left and right.  Has no intelligence!
        /// </summary>
        private void SetupEnemy()
        {
            Texture2D sheet4x3 = Content.Load<Texture2D>("Actors/rsc-sprite-sheet3");

            AnimationController manager = new AnimationController(graphicsDevice, sheet4x3, 4, 3);

            string[] keys = new string[] { "Down", "Left", "Right", "Up" };

            manager.CreateAnimationGroup(keys);

            enemySprite = new AnimatedSprite()
            {
                Scale = 2.0f,

                Position = new Vector2(1000, 200),
                Direction = new Vector2(-1, 0),
                Speed = 50,

                Rotation = MathHelper.ToRadians(0),
            };

            manager.AppendAnimationsTo(enemySprite);
            enemySprite.PlayAnimation("Left");
        }


        /// <summary>
        /// Called 60 frames/per second and updates the positions
        /// of all the drawable objects
        /// </summary>
        /// <param name="gameTime">
        /// Can work out the elapsed time since last call if
        /// you want to compensate for different frame rates
        /// </param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || 
                Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();

            restartButton.Update(gameTime);

            // Update Asteroids

            shipSprite.Update(gameTime);
            asteroidSprite.Update(gameTime);

            if (shipSprite.HasCollided(asteroidSprite) && shipSprite.IsAlive)
            {
                flameEffect.Play();

                shipSprite.IsActive = false;
                shipSprite.IsAlive = false;
                shipSprite.IsVisible = false;
            }

            // Update Chase Game

            playerSprite.Update(gameTime);
            enemySprite.Update(gameTime);

            if (playerSprite.HasCollided(enemySprite))
            {
                playerSprite.IsActive = false;
                playerSprite.IsAlive = false;
                enemySprite.IsActive = false;
            }

            coinsController.Update(gameTime);
            coinsController.DetectCollision(playerSprite);

            base.Update(gameTime);
        }

        /// <summary>
        /// Called 60 frames/per second and Draw all the 
        /// sprites and other drawable images here
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LawnGreen);

            spriteBatch.Begin();


            spriteBatch.Draw(backgroundImage, Vector2.Zero, Color.White);

            restartButton.Draw(spriteBatch, gameTime);

            // Draw asteroids game

            shipSprite.Draw(spriteBatch, gameTime);
            asteroidSprite.Draw(spriteBatch, gameTime);

            // Draw Chase game

            playerSprite.Draw(spriteBatch, gameTime);
            coinsController.Draw(spriteBatch, gameTime);
            enemySprite.Draw(spriteBatch, gameTime);

            DrawGameStatus(spriteBatch);
            DrawGameFooter(spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }

        /// <summary>
        /// Display the name of the game and the current score
        /// and status of the player at the top of the screen
        /// </summary>
        public void DrawGameStatus(SpriteBatch spriteBatch)
        {
            int topMargin = 4;
            int sideMargin = 50;
            
            Vector2 topLeft = new Vector2(sideMargin, topMargin);
            string status = $"Score = {score:##0}";

            spriteBatch.DrawString(arialFont, status, topLeft, Color.White);

            Vector2 gameSize = arialFont.MeasureString(GameName);
            Vector2 topCentre = new Vector2((Game_Width/2 - gameSize.X/2), topMargin);
            spriteBatch.DrawString(arialFont, GameName, topCentre, Color.White);

            string healthText = $"Energy = {energy:##0}%";
            Vector2 healthSize = arialFont.MeasureString(healthText);
            Vector2 topRight = new Vector2(Game_Width - (healthSize.X + sideMargin), topMargin);
            spriteBatch.DrawString(arialFont, healthText, topRight, Color.White);

        }

        /// <summary>
        /// Display identifying information such as the the App,
        /// the Module, the authors at the bottom of the screen
        /// </summary>
        public void DrawGameFooter(SpriteBatch spriteBatch)
        {
            int bottomMargin = 30;

            Vector2 namesSize = calibriFont.MeasureString(AuthorNames);
            Vector2 appSize = calibriFont.MeasureString(AppName);

            Vector2 bottomCentre = new Vector2((Game_Width - namesSize.X)/ 2, Game_Height - bottomMargin);
            Vector2 bottomLeft = new Vector2(bottomMargin, Game_Height - bottomMargin);
            Vector2 bottomRight = new Vector2(Game_Width - appSize.X - bottomMargin, Game_Height - bottomMargin);

            spriteBatch.DrawString(calibriFont, AuthorNames, bottomCentre, Color.Yellow);
            spriteBatch.DrawString(calibriFont, ModuleName, bottomLeft, Color.Yellow);
            spriteBatch.DrawString(calibriFont, AppName, bottomRight, Color.Yellow);

        }
    }
}
