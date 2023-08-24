using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace LastDayInKhuMuang
{
    public class SimpleChangeScenes
    {
        int scenes = 1;
        public Game1 game;
        public SpriteBatch spriteBatch;
        //Scenes
        private Vector2 pos = new Vector2(0,0);
        public Texture2D homeScene;
        public Texture2D bossTwoScene;
        public SimpleChangeScenes(Game1 game, SpriteBatch spriteBatch)
        {
            this.game = game;
            this.spriteBatch = spriteBatch; 
        }
        public void UpdateScenes(KeyboardState ks)
        {
            ks = Keyboard.GetState();
            if (ks.IsKeyDown(Keys.F1))
            {
                scenes = 1;
            }
            if (ks.IsKeyDown(Keys.F2))
            {
                scenes = 2;
            }
            if (ks.IsKeyDown(Keys.F3))
            {
                scenes = 3;
            }
            Console.WriteLine(scenes);
        }
        public void LoadScenes()
        {
            homeScene = game.Content.Load<Texture2D>("Resources/Scenes/draft-map");
            bossTwoScene = game.Content.Load<Texture2D>("Resources/Scenes/Bg1");
        }
        public void DrawScenes()
        {
            if (scenes == 1)
            {
                spriteBatch.Draw(homeScene, pos, null, Color.White, 0f, Vector2.Zero, new Vector2(2.5f, 2.5f), 0, 0);
            }
            else if (scenes == 2)
            {
                spriteBatch.Draw(bossTwoScene, pos, null, Color.White, 0f, Vector2.Zero, new Vector2(2.5f, 2.5f), 0, 0);
            }
            else if (scenes == 3)
            {
                spriteBatch.Draw(homeScene, pos, null, Color.White, 0f, Vector2.Zero, new Vector2(2.5f, 2.5f), 0, 0);
            }
        }
        public int GetScenes()
        {
            return scenes;
        }
    }
}
