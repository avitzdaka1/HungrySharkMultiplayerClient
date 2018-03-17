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
    public class Enemy : DrawableGameComponent
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
        private string name;
        private int id;

        public Vector2 Position { get => position; set => position = value; }
        public Texture2D PlayerTex { get => playerTex; set => playerTex = value; }

        public Enemy(Game game) : base(game)
        {
            this.game = game;
            playerTex = game.Content.Load<Texture2D>("shark");
            spriteBatch = (SpriteBatch)game.Services.GetService(typeof(SpriteBatch));
            position = new Vector2(-30, -30);
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
                spriteBatch.Draw(playerTex, new Vector2(position.X - playerTex.Width / 2, position.Y - playerTex.Height / 2), Color.Red);
            else
                spriteBatch.Draw(playerTex, new Vector2(position.X - playerTex.Width / 2, position.Y - playerTex.Height / 2), null, Color.Red, 0, Vector2.Zero, 1, SpriteEffects.FlipHorizontally, 0);
            base.Draw(gameTime);
        }

        public string getName()
        {
            return name;
        }

        public int getId()
        {
            return id;
        }

        public void setId(int id)
        {
            this.id = id;
        }

        public void setName(string name)
        {
            this.name = name;
        }


    }
}