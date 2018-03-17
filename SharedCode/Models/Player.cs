using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CrossPlatform.Fruits;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace AndroidVersion
{
   public class Player : DrawableGameComponent
    {
        private Random rnd;
        Game game;
        SpriteBatch spriteBatch;
        private Texture2D playerTex;
        private Vector2 position;
        private TouchCollection currentTouchState;
        private float joystickHeight;
        private float joystickWidth;
        enum Direction { Right, Left };
        private Direction myDirection;
        public static int id;
        public static string name = "";
        private Color myColor;
        private float speedFactor;
       

        public Vector2 Position { get => position; set => position = value; }
        public Texture2D PlayerTex { get => playerTex; set => playerTex = value; }

        public Player(Game game) : base(game)
        {
            rnd = new Random();
            this.game = game;
            playerTex = game.Content.Load<Texture2D>("shark");
            spriteBatch = (SpriteBatch)game.Services.GetService(typeof(SpriteBatch));
            joystickHeight = game.GraphicsDevice.Viewport.Height / 3;
            joystickWidth = game.GraphicsDevice.Viewport.Width / 4;
            position = new Vector2(rnd.Next(0,3000),rnd.Next(0,2000));
            myDirection = Direction.Right;
            myColor = Color.DarkGray;
            speedFactor = 3;
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
            if(Keyboard.GetState().IsKeyDown(Keys.Up) && position.Y > 0 + playerTex.Height/2)
                {
                    position.Y -= speedFactor;
                }
            if (Keyboard.GetState().IsKeyDown(Keys.Down) && position.Y < Scene1.mapHeight -playerTex.Width/2)
            {
                position.Y += speedFactor;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right) && position.X < Scene1.mapWidth - playerTex.Width/2)
            {
                position.X += speedFactor;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Left) && position.X > 0 + playerTex.Width/2)
            {
                position.X -= speedFactor;
            }


#elif ANDROID
            currentTouchState = TouchPanel.GetState();
            
           foreach(TouchLocation tl in currentTouchState)
            {
                if (tl.Position.X <= game.GraphicsDevice.Viewport.Width / 4.5 && tl.Position.Y >= game.GraphicsDevice.Viewport.Height*2 / 3)
                {
                    float x = 3*speedFactor * (tl.Position.X*(float)4.5 - game.GraphicsDevice.Viewport.Width/2)/ game.GraphicsDevice.Viewport.Width;
                 
                    float y = 5*speedFactor* (tl.Position.Y - joystickHeight - game.GraphicsDevice.Viewport.Height/2) / game.GraphicsDevice.Viewport.Height;


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
                spriteBatch.Draw(playerTex, new Vector2(position.X - playerTex.Width/2, position.Y - playerTex.Height/2), myColor);
            else
                spriteBatch.Draw(playerTex, new Vector2(position.X - playerTex.Width / 2, position.Y - playerTex.Height / 2), null, myColor, 0, Vector2.Zero, 1, SpriteEffects.FlipHorizontally, 0);
            base.Draw(gameTime);
        }

        public void setSpeedFactor(float speedFactor)
        {
            this.speedFactor = speedFactor;
        }

       

    }
}