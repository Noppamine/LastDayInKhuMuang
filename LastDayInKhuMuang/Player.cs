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

        Rectangle attackBox = new Rectangle();

        private bool idle = true;
        private bool dashCooldown = false;
        private bool dash = false;
        private bool attack;

        private float elapsed;
        private float temp;


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
        public void SetAction(int action)
        {
            this.action = action;
        }
        //Player move
        public void PlayerMove(KeyboardState ks, GraphicsDeviceManager gp, AnimatedTexture animate, GameTime gametime)
        {
            ks = Keyboard.GetState();
            animate.UpdateFrame(elapsed);
            PlayerDash(ks);
            if (!dash)
            {
                if (ks.IsKeyDown(Keys.A) && !attack)
                {
                    playerPos.X -= speed;
                    idle = false;
                    action = 2;
                }
                if (ks.IsKeyDown(Keys.D) && !attack)
                {
                    playerPos.X += speed;
                    idle = false;
                    action = 3;
                }
                if (ks.IsKeyDown(Keys.W) && !attack)
                {
                    playerPos.Y -= speed;
                    idle = false;
                    action = 4;
                }
                if (ks.IsKeyDown(Keys.S) && !attack)
                {
                    playerPos.Y += speed;
                    idle = false;
                    action = 1;
                }
            }
            if (dashCooldown)
            {
                CoolDown(gametime);
            }
            
            PlayerAttack(ks);
            if (ks.IsKeyUp(Keys.A) && ks.IsKeyUp(Keys.D) && ks.IsKeyUp(Keys.W) && ks.IsKeyUp(Keys.S))
            {
                idle = true;
            }
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

        //Player Dash ability
        public void PlayerDash(KeyboardState ks)
        {
            if (ks.IsKeyDown(Keys.LeftShift) && action == 2 && !dashCooldown && !dash)
            {
                playerPos = new Vector2(playerPos.X - 60, playerPos.Y);
                dash = true;
                dashCooldown = true;
            }
            if (ks.IsKeyUp(Keys.LeftShift))
            {
                dash = false;
            }
        }
        public void CoolDown(GameTime gametime)
        {
            //temp = 0;
            temp += (float)gametime.ElapsedGameTime.TotalMilliseconds / 1000;
            Console.WriteLine(temp);
            if (temp >= 3)
            {
                temp = 0;
                dashCooldown = false;
            }
            
        }

        public void PlayerAttack(KeyboardState ks)
        {
           //KeyboardState oldKs;
            
            if (ks.IsKeyDown(Keys.J) && action == 2 && !attack) //Left Attack
            {
                attackBox = new Rectangle((int)playerPos.X - playerWidth, (int)playerPos.Y, playerWidth, playerHeight);
                attack = true;
            }
            else if (ks.IsKeyDown(Keys.J) && action == 3 && !attack) // Right
            {
                attackBox = new Rectangle((int)playerPos.X + playerWidth, (int)playerPos.Y, playerWidth, playerHeight);
                attack = true;
            }
            else if (ks.IsKeyDown(Keys.J) && action == 4 && !attack) //Up
            {
                attackBox = new Rectangle((int)playerPos.X - ((playerHeight-playerWidth)/2), (int)playerPos.Y - playerWidth, playerHeight, playerWidth);
                attack = true;
            }
            else if (ks.IsKeyDown(Keys.J) && action == 1 && !attack) //Down
            {
                attackBox = new Rectangle((int)playerPos.X - ((playerHeight - playerWidth) / 2), (int)playerPos.Y + playerHeight, playerHeight, playerWidth);
                attack = true;
            }
            if (ks.IsKeyUp(Keys.J))
            {
                attack = false;
            }
        }
        public bool AttackCollision(Rectangle enemy)
        {
            return attackBox.Intersects(enemy);
        }
        public bool GetPlayerAtack()
        {
            return attack;
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
