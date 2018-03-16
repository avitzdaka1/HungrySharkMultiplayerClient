using CrossPlatform;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Input.Touch;

namespace AndroidVersion
{
    public class Menu : Scene
    {
        enum keys { A, B, C, D, E, F, G };

        private SpriteBatch spriteBatch;
        private Game game;
        private bool EndScene;
        private startButton btnPlay;
        private NameForm inputName;
        int screenWidth;
        int screenHeight;
        private bool inputMode;
        private StringBuilder name;
        private SpriteFont font;
        private Keys last_Key;
        private keys currentKey;

        public Menu(Game game) : base(game)
        {
            this.game = game;
            spriteBatch = (SpriteBatch)game.Services.GetService(typeof(SpriteBatch));
            EndScene = false;
            btnPlay = new startButton(game);
            SceneComponents.Add(btnPlay);
            screenWidth = GraphicsDevice.Viewport.Width;
            screenHeight = GraphicsDevice.Viewport.Height;
            btnPlay.setPosition(new Vector2(screenWidth / 4 + screenWidth / 10, screenHeight * 1 / 4));
            inputName = new NameForm();
            inputName.LoadContent(Game.Content);
            inputMode = false;
            name = new StringBuilder(20);
            font = game.Content.Load<SpriteFont>("MyFont");
            last_Key = 0;
            currentKey = 0;
            







        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
        public override void Update(GameTime gameTime)
        {


            MouseState mouse = Mouse.GetState();

            if (btnPlay.isClicked == true)
            {
                inputName.Enable(game);
                inputMode = true;
                btnPlay.Visible = false;
            }
                if(inputMode)
                {
#if ANDROID
                var gesture = default(GestureSample);
                TouchPanel.EnableMouseGestures = true;
                while (TouchPanel.IsGestureAvailable)
                {
                    gesture = TouchPanel.ReadGesture();

                    if (gesture.GestureType == GestureType.DragComplete)
                    {
                        if (gesture.Delta.Y < 0 || gesture.Delta.X < 0)
                            currentKey++;
                        if (gesture.Delta.Y > 0 || gesture.Delta.X > 0)
                            currentKey--;
                    }
                }
#endif








                Keys[] pressed_key = Keyboard.GetState().GetPressedKeys();
               
               
                   if(pressed_key.Length > 0)
                {
                    if(last_Key != pressed_key[0])
                    {
                        if (pressed_key[0] == Keys.Enter)
                            inputMode = false;
                        else
                        {
                            name.Append(pressed_key[0]);
                            last_Key = pressed_key[0];
                        }
                    }
                }
                    



                }
                
                



                //EndScene = true;
                
                       

                    
#if WINDOWS
                    btnPlay.Update(mouse);
            
#elif ANDROID
                    btnPlay.Update();
#endif
              
            

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {

                inputName.Draw(spriteBatch);
            if (inputMode)
                spriteBatch.DrawString(font, name + currentKey.ToString(), new Vector2(game.GraphicsDevice.Viewport.Width / 2, game.GraphicsDevice.Viewport.Height / 2),Color.Red);
              //  game.GraphicsDevice.Clear(Color.CornflowerBlue);
                base.Draw(gameTime);
            
            
        }

        public bool isEnded()
        {
            return EndScene;
        }

    }
}