using Comora;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;



namespace AndroidVersion
{
public class Scene1 : Scene
    {
        private Camera camera;
        private SpriteBatch spriteBatch;
        private Game game;
        private bool EndScene;
        private Player player;
        private Texture2D joystick;
        private Texture2D map;
        public static int mapHeight = 2000;
        public static int mapWidth = 3000;
        private Rectangle joystickPos;
        private SpriteFont font;
        private Vector2 fontPos = Vector2.Zero;
        private bool camMoving;
        private NetworkConnection networkConnection;

        double check;


        Viewport view;



        float leftBarrier, rightBarrier, bottomBarrier, topBarrier;


        



        public Scene1(Game game) : base(game)
        {
            this.game = game;
            
            spriteBatch = (SpriteBatch)game.Services.GetService(typeof(SpriteBatch));

            view = game.GraphicsDevice.Viewport;

            networkConnection = new NetworkConnection("Sharks", "Maks", "192.168.2.111", 15000);
            networkConnection.Start();

            player = new Player(game);
            joystick = game.Content.Load<Texture2D>("joystick");
            map = game.Content.Load<Texture2D>("seaTexture");
            font = game.Content.Load<SpriteFont>("MyFont");
            
            joystickPos = new Rectangle(-40, game.GraphicsDevice.Viewport.Height * 2 / 3 - 30, (int)(game.GraphicsDevice.Viewport.Width / 4.5 + 80), GraphicsDevice.Viewport.Height / 3 + 60);
            SceneComponents.Add(player);
            EndScene = false;
            
            camera = new Camera(game.GraphicsDevice);

            leftBarrier = view.Width / 2;
            rightBarrier = mapWidth -view.Width/2;
            topBarrier = mapHeight - view.Height / 2;
            bottomBarrier = view.Height / 2;
            camMoving = false;

            
        }

        protected override void Dispose(bool disposing)
        {
            player.Dispose();
            networkConnection.Stop();
            base.Dispose(disposing);
        }
        public override void Update(GameTime gameTime)
        {

            camera.Update(gameTime);

            check = Math.Sqrt(((camera.Position.X - player.Position.X) * (camera.Position.X - player.Position.X)) + ((camera.Position.Y - player.Position.Y) * (camera.Position.Y - player.Position.Y)));


            if (check > 200 || camMoving)
            {
                networkConnection.SendCoords(player.Position.X, player.Position.Y);
                camMoving = true;
                camera.Position = new Vector2(MathHelper.Lerp(camera.Position.X, player.Position.X, 0.05f), MathHelper.Lerp(camera.Position.Y, player.Position.Y, 0.05f));
                if (check < 20)
                    camMoving = false;
            }
                if (camera.Position.X < leftBarrier)
                camera.Position = new Vector2(leftBarrier,camera.Position.Y);
            if(camera.Position.X > rightBarrier)
                camera.Position = new Vector2(rightBarrier, camera.Position.Y);
            if (camera.Position.Y > topBarrier)
                camera.Position = new Vector2(camera.Position.X, topBarrier);
            if (camera.Position.Y < bottomBarrier)
                camera.Position = new Vector2(camera.Position.X, bottomBarrier);






            fontPos = new Vector2(camera.Position.X - game.GraphicsDevice.Viewport.Width/2,camera.Position.Y - game.GraphicsDevice.Viewport.Height/2);
            joystickPos.X = (int)camera.Position.X - view.Width / 2 - 40;
            joystickPos.Y = (int)camera.Position.Y + view.Height/2 - joystick.Height - 60;




               


             
            base.Update(gameTime);
        }

        private void _initialize() { }
        public override void Draw(GameTime gameTime)
        {

            

            spriteBatch.End();
            spriteBatch.Begin(this.camera);
                    
            spriteBatch.Draw(map,
   new Rectangle(0, 0, mapWidth, mapHeight),
   null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1);
#if ANDROID
            spriteBatch.Draw(joystick, joystickPos , Color.White);
#endif




            spriteBatch.DrawString(font, "Barrier: " + check, fontPos, Color.White);
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