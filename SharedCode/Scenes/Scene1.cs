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

        public Scene1(Game game) : base(game)
        {
            this.game = game;
            spriteBatch = (SpriteBatch)game.Services.GetService(typeof(SpriteBatch));
            player = new Player(game);
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
            game.GraphicsDevice.Clear(Color.CornflowerBlue);
            base.Draw(gameTime);
        }

        public bool isEnded()
        {
            return EndScene;
        }

    }
}