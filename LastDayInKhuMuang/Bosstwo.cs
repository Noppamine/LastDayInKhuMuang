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
        KeyboardState ks;
        private bool usedThunderBolt;
        private bool usedLightningBeam;
        private bool readyRandPattern;

        //Bolt
        private bool beamed;
        private Texture2D damageThunderboltArea;
        private Color[] dataBolt;
        private Texture2D thunderBolt;
        private Vector2 thunderBoltPos;
        private float thunderCooldown;

        //Beam
        private Vector2 lightningBeamPos;
        private Color[] dataBeam;
        private Texture2D damageLightningBeamArea;
        private float beamCooldown;
        private float delayBeam;

        //Rod
        private Texture2D damgeLightningRod;
        private Vector2 lightningRod;
        private Color[] dataRod;
        private Rectangle dangArea;
        private float shockEffect;
        private float rodCooldown;


        private float bossTime;

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
            thunderBolt = game.Content.Load<Texture2D>("Resources/ball");
            damageThunderboltArea = new Texture2D(game.GraphicsDevice, 60, 60);
            damageLightningBeamArea = new Texture2D(game.GraphicsDevice, 1690, 60);
            dataBolt = new Color[60*60];
            dataBeam = new Color[1690*60];
            usedThunderBolt = false;
            usedLightningBeam = false;
            readyRandPattern = true;
            for (int i=0; i < dataBolt.Length; i++)
            {
                dataBolt[i] = Color.LightPink;
            }
            for (int i = 0; i < dataBeam.Length; i++)
            {
                dataBeam[i] = Color.LightPink;
            }
            damageThunderboltArea.SetData(dataBolt);
            damageLightningBeamArea.SetData(dataBeam);
        }
        public void BossUpdate(Vector2 playerPos, GameTime gameTime)
        {
            //AttackPattern();
            AttackPattern(gameTime);
            if (!usedLightningBeam && !usedThunderBolt)
            {
                this.playerPos = playerPos;
            }

            if (!usedLightningBeam)
            {                
                lightningBeamPos = new Vector2(0, playerPos.Y);
            }
            else if (usedLightningBeam && beamed)
            {
                lightningBeamPos = new Vector2(0, this.playerPos.Y);
            }

            if (!usedThunderBolt)
            {                
                thunderBoltPos.X = playerPos.X;
                thunderBoltPos.Y = 0;
            }
            else if (usedThunderBolt)
            {
                thunderBoltPos.X = this.playerPos.X;
                thunderBoltPos.Y += 25;
            }
            ks = Keyboard.GetState();
            ThunderBolt(gameTime);
            LightningBeam(gameTime);
            Console.WriteLine("BossTime : "+bossTime);
        }

        public void BossDraw()
        {
            spriteBatch.Draw(boss2Texture, bossPos, Color.White);
            if (usedThunderBolt && thunderBoltPos.Y <= playerPos.Y)
            {
                spriteBatch.Draw(thunderBolt, thunderBoltPos, Color.White);
                spriteBatch.Draw(damageThunderboltArea, playerPos, Color.White);
            }
            if (usedLightningBeam)
            {
                spriteBatch.Draw(damageLightningBeamArea, lightningBeamPos, Color.White);
            }
        }

        //Start attack
        public void AttackPattern(GameTime gameTime)
        {
            if (readyRandPattern && !usedThunderBolt && !usedLightningBeam)
            {
                bossTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;
                int action;
                if (bossTime > 3 )
                {
                    action = rand.Next(0, 3);
                    switch (action)
                    {
                        case 1:
                            usedThunderBolt = true;
                            ThunderBolt(gameTime);
                            break;
                        case 2:
                            usedLightningBeam = true;
                            LightningBeam(gameTime);
                            break;
                    }
                }                
            }
            else if (!readyRandPattern || usedLightningBeam || usedThunderBolt)
            {
                bossTime = 0;
            }
            //else if (!readyRandPattern)
            //{
            //    bossTime = 0;
            //}            
        }
        public async void ThunderBolt(GameTime time)
        {
            if (usedThunderBolt && thunderBoltPos.Y >= playerPos.Y)
            {
                thunderCooldown += (float)time.ElapsedGameTime.TotalMilliseconds / 1000;
            }
            else if(!usedThunderBolt)
            {
                thunderCooldown = 0;
            }
            //attack
            //if (ks.IsKeyDown(Keys.NumPad1) && !usedThunderBolt)
            //{
            //    usedThunderBolt = true;
            //}
            if (thunderCooldown >= 3)
            {
                usedThunderBolt = false;
                readyRandPattern = true;
            }
        }
        public void LightningBeam(GameTime time)
        {
            //attack
            //if (ks.IsKeyDown(Keys.NumPad2) && !usedLightningBeam)
            //{
            //    usedLightningBeam = true;
            //}

            if (!usedLightningBeam)
            {
                delayBeam = 0;
                beamCooldown = 0;
                beamed = false;
                for (int i = 0; i < dataBeam.Length; i++)
                {
                    dataBeam[i] = Color.LightPink;
                }
                damageLightningBeamArea.SetData(dataBeam);
            }

            if (usedLightningBeam && !beamed)
            {
                delayBeam += (float)time.ElapsedGameTime.TotalMilliseconds / 1000;                
            }
            if (delayBeam >= 1)
            {
                for (int i = 0; i < dataBeam.Length; i++)
                {
                    dataBeam[i] = Color.Red;
                }
                beamed = true;
                damageLightningBeamArea.SetData(dataBeam);
            }

            if (beamed)
            {
                beamCooldown += (float)time.ElapsedGameTime.TotalMilliseconds / 1000;
            }
            if (beamCooldown >= 3)
            {
                usedLightningBeam = false;
                readyRandPattern = true;
            }
        }
        public void LightningRod()
        {
                
        }
    }
}
