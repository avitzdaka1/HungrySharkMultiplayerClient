using Microsoft.Xna.Framework;
using System.Collections.Generic;


namespace AndroidVersion
{
    public class Scene : DrawableGameComponent
    {
        public List<GameComponent> SceneComponents { get; set; }

        public Scene(Game game) :base(game)
        {
            SceneComponents = new List<GameComponent>();
            Visible = false;
            Enabled = false;
        }

        public void Show()
        {
            Enabled = true;
            Visible = true;
        }
        public void Hide()
        {
            Enabled = false;
            Visible = false;
        }
        public void Pause()
        {
            Enabled = false;
            Visible = true;
        }

        public override void Update(GameTime gameTime)
        {
            foreach(GameComponent Component in SceneComponents)
            {
                if (Component.Enabled)
                    Component.Update(gameTime);
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach(GameComponent Component in SceneComponents)
            {
                if(Component is DrawableGameComponent)
                {
                    if ((Component as DrawableGameComponent).Visible)
                        (Component as DrawableGameComponent).Draw(gameTime);
                }
            }
            base.Draw(gameTime);
        }
    }
}