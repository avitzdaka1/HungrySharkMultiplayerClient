using CrossPlatform;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AndroidVersion
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class MyCrossPlatformGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D mainScreen;
        int screenWidth;
        int screenHeight;

        enum GameState
        {
            MainMenu,
            SinglePlayer,
            MultiPlayer
        }

        GameState CurrentGameState = GameState.MainMenu;

        startButton btnPlay;

        public MyCrossPlatformGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

           // graphics.IsFullScreen = true;
          //  graphics.PreferredBackBufferWidth = 800;
           // graphics.PreferredBackBufferHeight = 480;
          //  graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            IsMouseVisible = true;
            base.Initialize();
            screenWidth = GraphicsDevice.Viewport.Width;
            screenHeight = GraphicsDevice.Viewport.Height;
           
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            mainScreen = Content.Load<Texture2D>("MainMenu");
            btnPlay = new startButton(Content.Load<Texture2D>("play"),graphics.GraphicsDevice);
            btnPlay.setPosition(new Vector2(200, 200));
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
             if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                Exit();
            MouseState mouse = Mouse.GetState();
            // TODO: Add your update logic here
            switch(CurrentGameState)
            {
                case GameState.MainMenu:
                    if (btnPlay.isClicked == true) CurrentGameState = GameState.SinglePlayer;
#if WINDOWS
                    btnPlay.Update(mouse);
#elif ANDROID
                     btnPlay.Update();
#endif
                    break;
                case GameState.SinglePlayer:

                    break;
                case GameState.MultiPlayer:

                    break;
            }


            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>



        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            switch (CurrentGameState)
            {
                case GameState.MainMenu:
                    spriteBatch.Draw(mainScreen, new Rectangle(0, 0, screenWidth, screenHeight), Color.White);
                    btnPlay.Draw(spriteBatch);
                    break;
                case GameState.SinglePlayer:

                    break;
                case GameState.MultiPlayer:

                    break;
            }


            
                base.Draw(gameTime);
            spriteBatch.End(); 
        }
    }
}
