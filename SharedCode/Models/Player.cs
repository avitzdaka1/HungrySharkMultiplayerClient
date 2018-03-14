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

        public Player(Game game) : base(game)
        {
            this.game = game;
            playerTex = game.Content.Load<Texture2D>("shark");
            spriteBatch = (SpriteBatch)game.Services.GetService(typeof(SpriteBatch));
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

            while (TouchPanel.IsGestureAvailable)
            {
                var gesture = TouchPanel.ReadGesture();
                switch (gesture.GestureType)
                {
                    case GestureType.HorizontalDrag:
                        position.X += 3;
                    break;

                    case GestureType.VerticalDrag:
                        position.Y -= 3;
                        break;
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