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
        enum Direction { Right, Left };
        private Direction myDirection;

        public Vector2 Position { get => position; set => position = value; }
        public Texture2D PlayerTex { get => playerTex; set => playerTex = value; }

        public Player(Game game) : base(game)
        {
            this.game = game;
            playerTex = game.Content.Load<Texture2D>("shark");
            spriteBatch = (SpriteBatch)game.Services.GetService(typeof(SpriteBatch));
            joystickHeight = game.GraphicsDevice.Viewport.Height / 3;
            joystickWidth = game.GraphicsDevice.Viewport.Width / 4;
            position = new Vector2(game.GraphicsDevice.Viewport.Width / 2 - playerTex.Width/2 , game.GraphicsDevice.Viewport.Height / 2 - playerTex.Height / 2);
            myDirection = Direction.Right;
        }

        protected override void Dispose(bool disposing)
        {
            playerTex.Dispose();
            base.Dispose(disposing);
        }

        public override void Update(GameTime gameTime)
        {
            double startX = position.X;
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


#elif ANDROID
            currentTouchState = TouchPanel.GetState();
            
           foreach(TouchLocation tl in currentTouchState)
            {
                if (tl.Position.X <= game.GraphicsDevice.Viewport.Width / 4.5 && tl.Position.Y >= game.GraphicsDevice.Viewport.Height*2 / 3)
                {
                    float x = 10 * (tl.Position.X*(float)4.5 - game.GraphicsDevice.Viewport.Width/2)/ game.GraphicsDevice.Viewport.Width;
                 
                    float y = 15* (tl.Position.Y - joystickHeight - game.GraphicsDevice.Viewport.Height/2) / game.GraphicsDevice.Viewport.Height;


                    position.X += x;
                    position.Y += y;
                }
            }

#endif
            if (startX != position.X)
            {
                if (startX - position.X < 0)
                    myDirection = Direction.Right;
                else
                    myDirection = Direction.Left;
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (myDirection == Direction.Right)
                spriteBatch.Draw(playerTex, position, Color.Red);
            else
                spriteBatch.Draw(playerTex, position, null, Color.Red, 0, Vector2.Zero, 1, SpriteEffects.FlipHorizontally, 0);
            base.Draw(gameTime);
        }

    }
}