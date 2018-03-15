using CrossPlatform;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;


namespace AndroidVersion
{
    public class Menu : Scene
    {
        private SpriteBatch spriteBatch;
        private Game game;
        private bool EndScene;
        private startButton btnPlay;
        
        int screenWidth;
        int screenHeight;

        public Menu(Game game) : base(game)
        {
            this.game = game;
            spriteBatch = (SpriteBatch)game.Services.GetService(typeof(SpriteBatch));
            EndScene = false;
            btnPlay = new startButton(game);
            SceneComponents.Add(btnPlay);
            screenWidth = GraphicsDevice.Viewport.Width;
            screenHeight = GraphicsDevice.Viewport.Height;
            btnPlay.setPosition(new Vector2(screenWidth / 4 + screenWidth / 10, screenHeight * 1 / 4));
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
        public override void Update(GameTime gameTime)
        {


            MouseState mouse = Mouse.GetState();

            if (btnPlay.isClicked == true)
                    {
                EndScene = true;
                
                       

                    }
#if WINDOWS
                    btnPlay.Update(mouse);
#elif ANDROID
                    btnPlay.Update();
#endif
              
            

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
          
                
                game.GraphicsDevice.Clear(Color.CornflowerBlue);
                base.Draw(gameTime);
            
            
        }

        public bool isEnded()
        {
            return EndScene;
        }

    }
}