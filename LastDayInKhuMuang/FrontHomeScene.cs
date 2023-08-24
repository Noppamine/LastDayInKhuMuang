
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
//using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastDayInKhuMuang
{
    public class FrontHomeScene : Scenes
    {
        Texture2D frontHome;
        Game1 game;
        public FrontHomeScene(Game1 game, EventHandler SceneEvent) : base(SceneEvent)
        {
            frontHome = game.Content.Load <Texture2D> ("Resources/deaft-map");
            this.game = game;
        }
        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.F1) == true)
            {
                SceneEvent.Invoke(game.mFrontHomeScene, new EventArgs());
                return;
            } 
            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(frontHome, new Vector2(0,0), Color.White);
            base.Draw(spriteBatch);
        }
        public event Action changed;
    }
}
