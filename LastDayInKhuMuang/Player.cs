using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastDayInKhuMuang
{
    public class Player
    {
        private int speed;
        private int speedBoost;
        private int stdSpeed;
        private int hp;
        private int action;
        private int stamina;
        
        private Vector2 playerPos;

        private bool idle = true;
        private bool dashCooldown = false;
        private bool swordwave = false;
        private bool ult = false;
        private float elapsed;


        private int playerWidth = 30;
        private int playerHeight = 45;
        public Player(int speed, int boost,  int hp, int stamina, Vector2 position)
        {
            this.speed = speed;
            stdSpeed = speed;
            this.hp = hp;
            speedBoost = boost;
            playerPos = position;
            this.stamina = stamina;
        }
        public void SetElapsed(float elapsed)
        {
            this.elapsed = elapsed;
        }
        public void PlayerMove(KeyboardState ks, GraphicsDeviceManager gp, AnimatedTexture animate)
        {
            ks = Keyboard.GetState();
            animate.UpdateFrame(elapsed);
            if (ks.IsKeyDown(Keys.A))
            {             
                playerPos.X -= speed;
                idle = false;
                action = 2;
            }
            if (ks.IsKeyDown(Keys.D))
            {
                playerPos.X += speed;
                idle = false;
                action = 3;
            }
            if (ks.IsKeyDown(Keys.W))
            {
                playerPos.Y -= speed;
                idle = false;
                action = 4;
            }
            if (ks.IsKeyDown(Keys.S))
            {
                playerPos.Y += speed;
                idle = false;
                action = 1;
            }
            
            if (ks.IsKeyUp(Keys.A) && ks.IsKeyUp(Keys.D) && ks.IsKeyUp(Keys.W) && ks.IsKeyUp(Keys.S))
            {
                idle = true;
            }
            //if (ks.IsKeyUp(Keys.LeftShift))
            //{
            //    speed = stdSpeed;
            //}

            //if (ks.IsKeyDown(Keys.E))
            //{
            //    swordwave = true;
            //}
            //if (ks.IsKeyDown(Keys.Q) && stamina = 3)
            //{
            //    ult = true;
            //}

            //Check Collision With Game Screen
            if (playerPos.X > gp.GraphicsDevice.Viewport.Width - playerWidth) 
            {
                playerPos.X -= speed;
            }
            if (playerPos.X < 0)
            {
                playerPos.X += speed;
            }
            if (playerPos.Y > gp.GraphicsDevice.Viewport.Height - playerHeight)
            {
                playerPos.Y -= speed;
            }
            if (playerPos.Y < 0)
            {
                playerPos.Y += speed;
            }
        }
        
        public Vector2 GetPlayerPos()
        {
            return playerPos;
        }

        public bool GetIdle()
        {
            return idle;
        }
        public int GetAction()
        {
            return action;
        }
    }

    
    
}
