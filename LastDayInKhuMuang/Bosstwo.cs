using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastDayInKhuMuang
{
    public class Bosstwo
    {
        //For Test
        KeyboardState ks = Keyboard.GetState();
        private bool usedThunderBolt;

        private Texture2D thunderBolt;
        private Texture2D boss2Texture;
        SpriteBatch spriteBatch;
        Game1 game;
        private int health;
        private Vector2 bossPos;
        private Vector2 playerPos;
        private Rectangle bossHitbox;
        private Random rand;
        //private Rectangle checkPlayer;
        public Bosstwo(int hp, Vector2 playerPos, SpriteBatch spriteBatch, Game1 game) 
        {
            this.playerPos = playerPos;
            this.spriteBatch = spriteBatch;
            this.game = game;
            health = hp;
            rand = new Random();
        }
        public void SetBossPos(GraphicsDeviceManager graphics)
        {
            bossPos = new Vector2(graphics.GraphicsDevice.Viewport.Width - boss2Texture.Width, graphics.GraphicsDevice.Viewport.Height - boss2Texture.Height);
        }
        public void BossLoad()
        {
            boss2Texture = game.Content.Load<Texture2D>("Resources/Boss/Boss-Attack-TunderBold-Access");
        }
        public void BossUpdate()
        {
            AttackPattern();
        }
        public void BossDraw()
        {
            spriteBatch.Draw(boss2Texture, bossPos, Color.White);
            if (usedThunderBolt)
            {
                spriteBatch.Draw(thunderBolt, bossPos, Color.White);
            }
        }
        public void AttackPattern()
        {
            int action;
            action = rand.Next(2);
            switch (action)
            {
                case 1:
                    ThunderBolt();
                    break;
                case 2:
                    LightningRod();
                    break;
            }
        }
        public void ThunderBolt()
        {
            if (ks.IsKeyDown(Keys.LeftShift) && ks.IsKeyDown(Keys.J) && !usedThunderBolt)
            {
                usedThunderBolt = true;
            }
        }
        public void FloorLightning()
        {

        }
        public void LightningRod()
        {

        }
    }
}
