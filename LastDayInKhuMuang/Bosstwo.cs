using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Timers;
using SharpDX.Direct2D1.Effects;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
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
        //Animate
        static AnimatedTexture boltAnimate;
        private const float boltRotation = 0;
        private const float boltScale = 1.0f;
        private const float boltDepth = 0.5f;
        private const int boltFrames = 10;
        private const int boltFramesPerSec = 6;
        private const int boltFramesRow = 1;

        //Beam
        private Vector2 lightningBeamPos;
        private Color[] dataBeam;
        private Texture2D damageLightningBeamArea;
        private float beamCooldown;
        private float delayBeam;
        //Animate
        static AnimatedTexture beamAnimate;
        private const float beamRotation = 0;
        private const float beamScale = 1.0f;
        private const float beamDepth = 0.5f;
        private const int beamFrames = 1;
        private const int beamFramesPerSec = 6;
        private const int beamFramesRow = 8;

        //Rod
        private Texture2D damageLightningRod;
        private Vector2 lightningRod;
        private Color[] dataRod;
        private Rectangle dangArea; //not sure
        private bool usedrod;
        private bool readyRod;
        private float delayShock;
        private float shockEffect;
        private float rodCooldown;
        //Animate
        static AnimatedTexture rodAnimate;
        private const float rodRotation = 0;
        private const float rodScale = 1.0f;
        private const float rodDepth = 0.5f;
        private const int rodFrames = 1;
        private const int rodFramesPerSec = 6;
        private const int rodFramesRow = 8;

        private float elapsed;
        private float bossTime;

        private Texture2D boss2Texture;
        SpriteBatch spriteBatch;
        Game1 game;
        private const int bossHeight = 768;
        private const int bossWidth = 768;
        private int health;
        private Vector2 bossPos;
        private Vector2 playerPos;
        private Rectangle playerBox;
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
            bossPos = new Vector2(graphics.GraphicsDevice.Viewport.Width - bossWidth, graphics.GraphicsDevice.Viewport.Height - bossHeight);
        }
        public void SetElapsed(float elapsed)
        {
            this.elapsed = elapsed;
        }
        public static void Initialization()
        {
           // boltAnimate = new AnimatedTexture(Vector2.Zero, boltRotation, boltScale, boltDepth);
            beamAnimate = new AnimatedTexture(Vector2.Zero, beamRotation, beamScale, beamDepth);
            rodAnimate = new AnimatedTexture(Vector2.Zero, rodRotation, rodScale, rodDepth);
        }
        public void BossLoad(Game1 game)
        {
            boss2Texture = game.Content.Load<Texture2D>("Resources/Boss/Boss-Attack-TunderBold-Access");
            thunderBolt = game.Content.Load<Texture2D>("Resources/ball");
            damageThunderboltArea = new Texture2D(game.GraphicsDevice, 60, 60);
            damageLightningBeamArea = new Texture2D(game.GraphicsDevice, 1690, 60);
            damageLightningRod = new Texture2D(game.GraphicsDevice, 768, 768/2);

           // boltAnimate.Load(game.Content, "Resources/Boss/Animation-Tunder_Boss", boltFrames, boltFramesRow, boltFramesPerSec);
            beamAnimate.Load(game.Content, "Resources/Boss/ShootLighting_Boss", beamFrames, beamFramesRow, beamFramesPerSec);
            rodAnimate.Load(game.Content, "ThundeTrap_Boss", rodFrames,rodFramesRow,rodFramesPerSec);

            dataBolt = new Color[60*60];
            dataBeam = new Color[1690*60];
            dataRod = new Color[(768) * (768 / 2)];

            usedThunderBolt = false;
            usedLightningBeam = false;
            usedrod = false;
            readyRandPattern = true;
            readyRod = true;
            for (int i=0; i < dataBolt.Length; i++)
            {
                dataBolt[i] = Color.LightPink;
            }
            for (int i = 0; i < dataBeam.Length; i++)
            {
                dataBeam[i] = Color.LightPink;
            }
            for (int i=0; i < dataRod.Length; i++)
            {
                dataRod[i] = Color.LightPink;
            }
            damageThunderboltArea.SetData(dataBolt);
            damageLightningBeamArea.SetData(dataBeam);
            damageLightningRod.SetData(dataRod);
        }
        public void BossUpdate(Vector2 playerPos, GameTime gameTime, Rectangle playerBox, AnimatedTexture bossAnimate)
        {
            bossAnimate.UpdateFrame(elapsed);
            this.playerBox = playerBox;
            lightningRod = new Vector2(bossPos.X, bossPos.Y + (768/2) /*bossPos.Y + (boss2Texture.Height / 2)*/);
            dangArea = new Rectangle((int)bossPos.X, (int)bossPos.Y + (boss2Texture.Height / 2), boss2Texture.Width, boss2Texture.Height);
            AttackPattern(gameTime, bossAnimate);                        
            if (!usedLightningBeam && !usedThunderBolt)
            {
                this.playerPos = new Vector2(playerBox.X, playerBox.Y);
            }

            if (!usedLightningBeam)
            {                
                lightningBeamPos = new Vector2(0, playerBox.Y);
            }
            else if (usedLightningBeam && beamed)
            {
                lightningBeamPos = new Vector2(0, this.playerPos.Y);
            }

            if (!usedThunderBolt)
            {                
                thunderBoltPos.X = playerBox.X;
                thunderBoltPos.Y = 0;
            }
            else if (usedThunderBolt)
            {
                thunderBoltPos.X = this.playerPos.X;
                thunderBoltPos.Y += 25; //changing
            }
            ks = Keyboard.GetState();
            ThunderBolt(gameTime);
            LightningBeam(gameTime, bossAnimate);
            LightningRod(gameTime,spriteBatch ,rodAnimate);
        }

        public void BossDraw(SpriteBatch spriteBatch, AnimatedTexture boss2Aniamte)
        {
            if (usedrod)
            {
                spriteBatch.Draw(damageLightningRod, lightningRod, Color.White);
                if (delayShock >= 2.5)
                {
                    rodAnimate.DrawFrame(spriteBatch, lightningRod);
                    rodAnimate.UpdateFrame(elapsed);
                }
            }
            if (usedThunderBolt && thunderBoltPos.Y <= playerPos.Y)
            {
                spriteBatch.Draw(thunderBolt, thunderBoltPos, Color.White);
                spriteBatch.Draw(damageThunderboltArea, playerPos, Color.White);
            }
            if (usedLightningBeam && !beamed)
            {
                spriteBatch.Draw(damageLightningBeamArea, lightningBeamPos, Color.White);
                //animate
                beamAnimate.DrawFrame(spriteBatch, lightningBeamPos);
                if (beamAnimate.frame_r >= 5)
                {
                    beamAnimate.DrawFrame(spriteBatch, 1, lightningBeamPos, 8);
                }
            }
            else if (usedLightningBeam && beamed)
            {
                beamAnimate.DrawFrame(spriteBatch, lightningBeamPos);
            }

            //Boss
            if (!usedLightningBeam && !usedThunderBolt )
            {
                spriteBatch.Draw(boss2Texture, bossPos, Color.White);               
            }
            else
            {
                boss2Aniamte.DrawFrame(spriteBatch, bossPos);
            }
            
            //spriteBatch.Draw(boss2Texture, bossPos, Color.White);
        }

        //Start attack
        public void AttackPattern(GameTime gameTime, AnimatedTexture bossAnimate)
        {
            //LightningRod(gameTime);
            if (readyRandPattern && !usedThunderBolt && !usedLightningBeam /*&& !usedrod*/)
            {
                bossTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;
                int action;
                if (bossTime > 2 )
                {
                    action = rand.Next(0, 3);
                    //action = 2;
                    switch (action)
                    {
                        case 1:
                            usedThunderBolt = true;
                            ThunderBolt(gameTime);
                            break;
                        case 2:
                            usedLightningBeam = true;
                            LightningBeam(gameTime, bossAnimate);
                            break;
                    }
                }                
            }
            else if (!readyRandPattern || usedLightningBeam || usedThunderBolt || usedrod)
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
        public void LightningBeam(GameTime time, AnimatedTexture bossAnimate)
        {
            //attack
            //if (ks.IsKeyDown(Keys.NumPad2) && !usedLightningBeam)
            //{
            //    usedLightningBeam = true;
            //}
            //Console.WriteLine(beamAnimate.Frame);
            //Console.WriteLine(usedLightningBeam);
            //Console.WriteLine(beamed);
            Console.WriteLine(beamAnimate.IsEnd);
            Console.WriteLine("Frame row : " + beamAnimate.frame_r);
            if (!usedLightningBeam)
            {
                delayBeam = 0;
                beamCooldown = 0;
                beamed = false;
                bossAnimate.ResetAll();
                bossAnimate.Pause();
                beamAnimate.Pause();
                //animate
                //for (int i = 0; i < dataBeam.Length; i++)
                //{
                //    dataBeam[i] = Color.LightPink;
                //}
                //damageLightningBeamArea.SetData(dataBeam);

            }
            if (usedLightningBeam && !beamed)
            {                
                bossAnimate.Play();
                //delayBeam += (float)time.ElapsedGameTime.TotalMilliseconds / 1000;
                if (bossAnimate.frame_r == 1 && bossAnimate.Frame == 3)
                {
                    bossAnimate.Pause();
                    bossAnimate.frame_r = 1; bossAnimate.Frame = 2;
                    //bossAnimate.Pause();
                    beamAnimate.Play();
                    if (beamAnimate.frame_r == 6 && !beamed)
                    {
                        beamed = true;
                        beamAnimate.frame_r = 6;
                    }
                }
                
            }
            
            //if (delayBeam >= 1)
            //{
            //    //animate
            //    for (int i = 0; i < dataBeam.Length; i++)
            //    {
            //        dataBeam[i] = Color.Red;
            //    }
            //    beamed = true;
            //    damageLightningBeamArea.SetData(dataBeam);
            //}
            if (usedLightningBeam || beamed)
            {
                beamAnimate.UpdateFrame(elapsed);
            }
            if (beamed)
            {
                beamCooldown += (float)time.ElapsedGameTime.TotalMilliseconds / 1000;
                beamAnimate.Pause();
                //beamAnimate.frame_r = 6;
            }
            if (beamCooldown >= 3)
            {
                usedLightningBeam = false;
                beamed = false;
                readyRandPattern = true;
                beamAnimate.frame_r = 0;
            }
        }
        public void LightningRod(GameTime gameTime , SpriteBatch spriteBatch ,AnimatedTexture rodAnimate)
        {           
            if (this.playerBox.Intersects(dangArea) && readyRod)
            {
                usedrod = true;
                readyRod = false;
            }

            if (usedrod)
            {
                delayShock += (float)gameTime.ElapsedGameTime.Milliseconds / 1000;
                if (delayShock >= 2.5)
                {
                    shockEffect += (float)gameTime.ElapsedGameTime.Milliseconds / 1000;
                    for (int i = 0; i < dataRod.Length; i++)
                    {
                        dataRod[i] = Color.White;
                    }
                    damageLightningRod.SetData(dataRod);
                }
                if (shockEffect >= 6)
                {
                    usedrod = false;
                }
            }
            else if (!usedrod)
            {
                delayShock = 0;
                shockEffect = 0;                
                for (int i = 0; i < dataRod.Length; i++)
                {
                    dataRod[i] = Color.LightPink;
                }
                damageLightningRod.SetData(dataRod);
            }

            if (!usedrod && !readyRod)
            {
                rodCooldown += (float)gameTime.ElapsedGameTime.Milliseconds / 1000;
                if (rodCooldown >= 5)
                {
                    readyRod = true;
                    rodCooldown = 0;
                }
            }
            //test skill
            //if (ks.IsKeyDown(Keys.NumPad3))
            //{
            //    usedrod = true;
            //}
            //else if (ks.IsKeyUp(Keys.NumPad3))
            //{
            //    usedrod = false;
            //}                          
        }
    }
}
