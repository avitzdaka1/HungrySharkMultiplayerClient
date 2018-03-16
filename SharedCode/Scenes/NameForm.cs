using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace AndroidVersion
{
    class NameForm
    {
        private Texture2D guiTexture;
        private Vector2 guiPos;
        private bool isEnabled;

        public NameForm()
        {
            isEnabled = false;
        }

        public void LoadContent(ContentManager content)
        {
            guiTexture = content.Load<Texture2D>("Input");
       
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if(isEnabled)
                spriteBatch.Draw(guiTexture, guiPos, Color.White);
        }

        public void Enable(Game game)
        {
            guiPos = new Vector2(game.GraphicsDevice.Viewport.Width / 2 - guiTexture.Width / 2, game.GraphicsDevice.Viewport.Height / 2 - guiTexture.Height / 2);
            isEnabled = true;
        }


        public void Disable()
        {
            isEnabled = false;
        }

    }

   

}