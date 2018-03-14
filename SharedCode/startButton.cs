using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;


namespace CrossPlatform
{
    class startButton
    {
        private Texture2D texture;
        private Vector2 position;
        private Rectangle rectangle;



        private Color color = new Color(255, 255, 255, 255);

        private Vector2 size;
        bool down;
        public bool isClicked;

        public startButton(Texture2D texture, GraphicsDevice graphics)
        {
            this.texture = texture;

            size = new Vector2(graphics.Viewport.Width / 4, graphics.Viewport.Height / 8);



        }

        public Vector2 getSize()
        {
            return this.size;
        }
#if WINDOWS
        public void Update(MouseState mouse)
        {
            rectangle = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
            Rectangle mouseRectangle = new Rectangle(mouse.X, mouse.Y, 1, 1);

            if(mouseRectangle.Intersects(rectangle))
            {
                if (color.A == 255) down = false;
                if (color.A == 0) down = true;
                if (down) color.A += 3;
                    else color.A -= 3;
                if (mouse.LeftButton == ButtonState.Pressed)
                    isClicked = true;
                
            }
            else if(color.A < 255)
            {
                color.A += 3;
                isClicked = false;
            }
        }

#elif ANDROID
        public void Update()
        {
            TouchCollection touchCollection;
            touchCollection = TouchPanel.GetState();

            foreach (TouchLocation tl in touchCollection)
            {
                if (tl.State == TouchLocationState.Pressed)

                {
                    rectangle = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
                    Rectangle touchPos = new Rectangle((int)tl.Position.X - 1, (int)tl.Position.Y - 1, (int)tl.Position.X + 1, (int)tl.Position.Y + 1);


                    if (touchPos.Intersects(rectangle))
                    {

                        isClicked = true;
                    }


                }


            }

        }





#endif
        public void setPosition( Vector2 position)
        {
            this.position = position;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rectangle, color);
        }


    }
}