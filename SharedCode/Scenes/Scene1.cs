using Comora;
using CrossPlatform.Fruits;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
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
        private bool connected;
        private static NetworkConnection networkConnection;
        double check;
        Enemy[] enemies;
        HashSet<Apple> apples;
        Texture2D appleTex;
        private Song backgroundMusic;
        private Random rnd;
        List<SoundEffect> snd;
        Viewport view;



        float leftBarrier, rightBarrier, bottomBarrier, topBarrier;


        



        public Scene1(Game game) : base(game)
        {
            this.game = game;
            rnd = new Random((int)DateTime.Now.Ticks);

            spriteBatch = (SpriteBatch)game.Services.GetService(typeof(SpriteBatch));

            view = game.GraphicsDevice.Viewport;
          

            

            player = new Player(game);
            joystick = game.Content.Load<Texture2D>("joystick");
            map = game.Content.Load<Texture2D>("seaTexture");
            font = game.Content.Load<SpriteFont>("MyFont");

            int songNumber = rnd.Next(1, 5);
            backgroundMusic = game.Content.Load<Song>("background" + songNumber);


            joystickPos = new Rectangle(-40, game.GraphicsDevice.Viewport.Height * 2 / 3 - 30, (int)(game.GraphicsDevice.Viewport.Width / 4.5 + 80), GraphicsDevice.Viewport.Height / 3 + 60);
            SceneComponents.Add(player);
            EndScene = false;
            
            camera = new Camera(game.GraphicsDevice);

            leftBarrier = view.Width / 2;
            rightBarrier = mapWidth -view.Width/2;
            topBarrier = mapHeight - view.Height / 2;
            bottomBarrier = view.Height / 2;
            camMoving = false;
            connected = false;
            enemies = new Enemy[20];
            apples = new HashSet<Apple>();
            snd = new List<SoundEffect>();

            for(int i = 1; i < 5; i ++)
            {
                SoundEffect tmp = game.Content.Load<SoundEffect>("eat2");
                snd.Add(tmp);
            }

            appleTex = game.Content.Load<Texture2D>("apple");
        }

        public void StartNetwork()
        {
            
            MediaPlayer.Play(backgroundMusic);
            networkConnection = new NetworkConnection(game, "Sharks", Player.name, "192.168.2.111", 15000, enemies, apples);
            networkConnection.Start();
            connected = true;
        }

        protected override void Dispose(bool disposing)
        {
            player.Dispose();
            if(isConnected())
                networkConnection.Stop();
            base.Dispose(disposing);
        }
        public override void Update(GameTime gameTime)
        {
           networkConnection.Eat(player,snd, rnd);


            camera.Update(gameTime);

            check = Math.Sqrt(((camera.Position.X - player.Position.X) * (camera.Position.X - player.Position.X)) + ((camera.Position.Y - player.Position.Y) * (camera.Position.Y - player.Position.Y)));


            if (check > 200 || camMoving)
            {
                
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


            
            networkConnection.SendCoords(player.Position.X, player.Position.Y);
            networkConnection.Update();
            








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
            spriteBatch.Draw(joystick, joystickPos, Color.White);
#endif

            foreach (Apple ap in apples)
            {
                spriteBatch.Draw(appleTex, new Vector2((float)ap.getX(), (float)ap.getY()), Color.White);
            }

            foreach (Enemy en in enemies)
                if (en != null)
                {

                    if (en.getDirection() == 0) { 
                        spriteBatch.Draw(en.PlayerTex, new Vector2(en.Position.X - en.PlayerTex.Width / 2, en.Position.Y - en.PlayerTex.Height / 2), Color.White);
                        spriteBatch.DrawString(font, en.getName(), new Vector2(en.Position.X - 20, en.Position.Y - 50), Color.White);
                }
                else
                    { 
                    spriteBatch.Draw(en.PlayerTex, new Vector2(en.Position.X - en.PlayerTex.Width / 2, en.Position.Y - en.PlayerTex.Height / 2), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.FlipHorizontally, 0);
                    spriteBatch.DrawString(font, en.getName(), new Vector2(en.Position.X - 20, en.Position.Y - 50), Color.White);

        }
        }
            
            spriteBatch.DrawString(font, Player.name, new Vector2(player.Position.X-20, player.Position.Y-50), Color.White);
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

        public bool isConnected()
        {
            return connected;
        }

    }

   
}