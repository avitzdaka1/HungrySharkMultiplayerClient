using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace AndroidVersion
{
   public class Player : DrawableGameComponent
    {
        Game game;
        SpriteBatch spriteBatch;
        private Texture2D playerTex;
        private Vector2 position;
        private TouchCollection currentTouchState;
        private float joystickHeight;
        private float joystickWidth;

        public Player(Game game) : base(game)
        {
            this.game = game;
            playerTex = game.Content.Load<Texture2D>("shark");
            spriteBatch = (SpriteBatch)game.Services.GetService(typeof(SpriteBatch));
            joystickHeight = game.GraphicsDevice.Viewport.Height / 3;
            joystickWidth = game.GraphicsDevice.Viewport.Width / 4;
        }

        protected override void Dispose(bool disposing)
        {
            playerTex.Dispose();
            base.Dispose(disposing);
        }

        public override void Update(GameTime gameTime)
        {

#if WINDOWS
            if(Keyboard.GetState().IsKeyDown(Keys.Up))
                {
                    position.Y -= 3;
                }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                position.Y += 3;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                position.X += 3;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                position.X -= 3;
            }

            base.Update(gameTime);
#elif ANDROID
            currentTouchState = TouchPanel.GetState();
            
           foreach(TouchLocation tl in currentTouchState)
            {
                if (tl.Position.X <= game.GraphicsDevice.Viewport.Width / 4.5 && tl.Position.Y >= game.GraphicsDevice.Viewport.Height / 3)
                {
                    float x = 10 * (tl.Position.X*(float)4.5 - game.GraphicsDevice.Viewport.Width/2)/ game.GraphicsDevice.Viewport.Width;
                 
                    float y = 10* (tl.Position.Y - joystickHeight - game.GraphicsDevice.Viewport.Height/2) / game.GraphicsDevice.Viewport.Height;


                    position.X += x;
                    position.Y += y;
                }
            }

#endif
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Draw(playerTex, position, Color.Red);
            base.Draw(gameTime);
        }

    }
}