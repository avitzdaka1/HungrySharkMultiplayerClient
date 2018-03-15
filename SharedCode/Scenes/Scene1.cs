using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;


namespace AndroidVersion
{
public class Scene1 : Scene
    {
    private SpriteBatch spriteBatch;
    private Game game;
    private bool EndScene;
    private Player player;
    private Texture2D joystick;

        public Scene1(Game game) : base(game)
        {
            this.game = game;
            spriteBatch = (SpriteBatch)game.Services.GetService(typeof(SpriteBatch));
            player = new Player(game);
            joystick = game.Content.Load<Texture2D>("joystick");
            SceneComponents.Add(player);
            EndScene = false;
        }

        protected override void Dispose(bool disposing)
        {
            player.Dispose();
            base.Dispose(disposing);
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        private void _initialize() { }
        public override void Draw(GameTime gameTime)
        {
#if ANDROID
            spriteBatch.Draw(joystick, new Rectangle(0, game.GraphicsDevice.Viewport.Height*2 / 3, (int)(game.GraphicsDevice.Viewport.Width / 4.5), GraphicsDevice.Viewport.Height/3), Color.White);
#endif
            game.GraphicsDevice.Clear(Color.CornflowerBlue);
            base.Draw(gameTime);
        }

        public bool isEnded()
        {
            return EndScene;
        }

    }
}