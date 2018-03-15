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
        private Texture2D map;
        private int mapHeight = 3000;
        private int mapWidth = 5000;
        private Camera2d camera;

        public Scene1(Game game) : base(game)
        {
            this.game = game;
            
            spriteBatch = (SpriteBatch)game.Services.GetService(typeof(SpriteBatch));
            
            player = new Player(game);
            joystick = game.Content.Load<Texture2D>("joystick");
            map = game.Content.Load<Texture2D>("seaTexture");
            camera = new Camera2d(new Viewport(new Rectangle(0, 0, 100, 100)), mapWidth, mapHeight, 1);
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
            camera.Pos = new Vector2(player.Position.X + player.PlayerTex.Width - game.GraphicsDevice.Viewport.Width/2  ,   player.Position.Y - game.GraphicsDevice.Viewport.Height/2 + player.PlayerTex.Height*3);
            base.Update(gameTime);
        }

        private void _initialize() { }
        public override void Draw(GameTime gameTime)
        {
#if ANDROID
            spriteBatch.Draw(joystick, new Rectangle(-40, game.GraphicsDevice.Viewport.Height*2 / 3 - 30 , (int)(game.GraphicsDevice.Viewport.Width / 4.5+80), GraphicsDevice.Viewport.Height/3 +60), Color.White);
#endif
            //  spriteBatch.Draw(map, new Rectangle(0,0, 2000, 2000), Color.White);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.BackToFront,
                    null, null, null, null, null,
                     camera.GetTransformation());
            spriteBatch.Draw(map,
   new Rectangle(0, 0, mapWidth, mapHeight),
   null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1);


            game.GraphicsDevice.Clear(Color.CornflowerBlue);
            base.Draw(gameTime);
            spriteBatch.End();
            spriteBatch.Begin();
        }

        public bool isEnded()
        {
            return EndScene;
        }

    }
}