using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Timers;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;



namespace LastDayInKhuMuang
{
    public class Player
    {        
        //Player valuable
        private int speed;
        private int speedBoost;
        private int stdSpeed;
        private int hp;
        private int action;
        private int stamina = 3;
        private int dashRange;

        private string Direction;
        
        private Vector2 playerPos;
        private Vector2 skillPos;
        private Vector2 skillspeed = new Vector2(10,10);
        private string SkillDirection;

        Rectangle attackBox = new Rectangle();
        Rectangle skillBox = new Rectangle();

        private bool idle = true;
        private bool dashCooldown = false;
        private bool dash = false;
        private bool attack;
        private bool skillpress = false;
        private bool Skill = false;
        private bool skilltime = false;

        private float elapsed;
        private float temp;
        private float skilltimer;

        //Player size
        private int playerWidth = 128;
        private int playerHeight = 128;

        public Player(int speed, int boost,  int hp, int stamina, Vector2 position , Vector2 skillposition)
        {
            this.speed = speed;
            stdSpeed = speed;
            this.hp = hp;
            speedBoost = boost;
            playerPos = position;
            skillPos = skillposition;
            this.stamina = stamina;
            Direction = "Idle";
            dashRange = 80;
        }
        
        public void SetElapsed(float elapsed)
        {
            this.elapsed = elapsed;
        }
        public void SetAction(int action)
        {
            this.action = action;
        }
        public void SetPosition(Vector2 Pos)
        {
            playerPos = Pos;
        }
        //Player move
        public void PlayerMove(KeyboardState ks, GraphicsDeviceManager gp, AnimatedTexture animate, AnimatedTexture attackanimate, AnimatedTexture skillanimate, GameTime gametime)
        {
            ks = Keyboard.GetState();
            animate.UpdateFrame(elapsed);
            attackanimate.UpdateFrame(elapsed);
            PlayerDash(ks);
            if(Skill == true && !skillanimate.IsEnd)
            {
                skillanimate.UpdateFrame(elapsed);
            }
            if (!dash)
            {
                if (ks.IsKeyDown(Keys.A) && !attack && !Skill)
                {
                    playerPos.X -= speed;
                    idle = false;
                    action = 2;
                    Direction = "Left";
                }
                if (ks.IsKeyDown(Keys.D) && !attack && !Skill)
                {
                    playerPos.X += speed;
                    idle = false;
                    action = 1;
                    Direction = "Right";
                }
                if (ks.IsKeyDown(Keys.W) && !attack && !Skill)
                {
                    playerPos.Y -= speed;
                    idle = false;
                    action = 1;
                    Direction = "Up";
                }
                if (ks.IsKeyDown(Keys.S) && !attack && !Skill)
                {
                    playerPos.Y += speed;
                    idle = false;
                    action = 1;
                    Direction = "Down";
                }
                if (ks.IsKeyDown(Keys.W) && ks.IsKeyDown(Keys.A))
                {
                    Direction = "Top-Left";
                }
                if (ks.IsKeyDown(Keys.W) && ks.IsKeyDown(Keys.D))
                {
                    Direction = "Top-Right";
                }
                if (ks.IsKeyDown(Keys.S) && ks.IsKeyDown(Keys.A))
                {
                    Direction = "Down-Left";
                }
                if (ks.IsKeyDown(Keys.S) && ks.IsKeyDown(Keys.D))
                {
                    Direction = "Down-Right";
                }
            }
            if (dashCooldown)
            {
                DashCoolDown(gametime);
            }

            PlayerAttack(ks ,attackanimate);
            if (ks.IsKeyUp(Keys.A) && ks.IsKeyUp(Keys.D) && ks.IsKeyUp(Keys.W) && ks.IsKeyUp(Keys.S))
            {
                idle = true;
            }
            PlayerSkill(ks , gametime , animate , skillanimate);
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
            if (ks.IsKeyDown(Keys.LeftShift) && Direction == "Left" && !dashCooldown && !dash) /*left*/
            {
                playerPos = new Vector2(playerPos.X - dashRange, playerPos.Y);
                dash = true;
                dashCooldown = true;
            }
            if (ks.IsKeyDown(Keys.LeftShift) && Direction == "Top-Left" && !dashCooldown && !dash) //Top-Left
            {
                playerPos = new Vector2(playerPos.X - dashRange, playerPos.Y - dashRange);
                dash = true;
                dashCooldown = true;
            }
            if (ks.IsKeyDown(Keys.LeftShift) && Direction == "Down-Left" && !dashCooldown && !dash) //Down-Left
            {
                playerPos = new Vector2(playerPos.X - dashRange, playerPos.Y + dashRange);
                dash = true;
                dashCooldown = true;
            }
            if (ks.IsKeyDown(Keys.LeftShift) && Direction == "Right" && !dashCooldown && !dash) // right
            {
                playerPos = new Vector2(playerPos.X + dashRange, playerPos.Y);
                dash = true;
                dashCooldown = true;
            }
            if (ks.IsKeyDown(Keys.LeftShift) && Direction == "Top-Right" && !dashCooldown && !dash) //Top-Right
            {
                playerPos = new Vector2(playerPos.X + dashRange, playerPos.Y - dashRange);
                dash = true;
                dashCooldown = true;
            }
            if (ks.IsKeyDown(Keys.LeftShift) && Direction == "Down-Right" && !dashCooldown && !dash) //Down-Right
            {
                playerPos = new Vector2(playerPos.X + dashRange, playerPos.Y + dashRange);
                dash = true;
                dashCooldown = true;
            }
            if (ks.IsKeyDown(Keys.LeftShift) && Direction == "Down" && !dashCooldown && !dash) // down
            {
                playerPos = new Vector2(playerPos.X, playerPos.Y + dashRange);
                dash = true;
                dashCooldown = true;
            }
            if (ks.IsKeyDown(Keys.LeftShift) && Direction == "Up" && !dashCooldown && !dash) //up
            {
                playerPos = new Vector2(playerPos.X, playerPos.Y - dashRange);
                dash = true;
                dashCooldown = true;
            }
            if (ks.IsKeyUp(Keys.LeftShift))
            {
                dash = false;
            }
        }
        public void DashCoolDown(GameTime gametime)
        {
            //temp = 0;
            temp += (float)gametime.ElapsedGameTime.TotalMilliseconds / 1000;
            //Console.WriteLine("CoolDown Check : " + temp);            
            if (temp >= 3)
            {
                temp = 0;
                dashCooldown = false;
            }
            
        }

        public void PlayerAttack(KeyboardState ks , AnimatedTexture attackanimate)
        {
           //KeyboardState oldks;
            
            if (ks.IsKeyDown(Keys.J) && Direction == "Left" && !attack) //Left Attack
            {
                attackBox = new Rectangle((int)playerPos.X - playerWidth, (int)playerPos.Y, playerWidth, playerHeight);
                attack = true;
                idle = true;
            }
            else if (ks.IsKeyDown(Keys.J) && Direction == "Right" && !attack) // Right Attack
            {
                attackBox = new Rectangle((int)playerPos.X + playerWidth, (int)playerPos.Y, playerWidth, playerHeight);
                attack = true;
                idle = true;
            }
            else if (ks.IsKeyDown(Keys.J) && action == 4 && !attack) //Up Attack
            {
                attackBox = new Rectangle((int)playerPos.X - ((playerHeight-playerWidth)/2), (int)playerPos.Y - playerWidth, playerHeight, playerWidth);
                attack = true;
            }
            else if (ks.IsKeyDown(Keys.J) && action == 1 && !attack) //Down Attack
            {
                attackBox = new Rectangle((int)playerPos.X - ((playerHeight - playerWidth) / 2), (int)playerPos.Y + playerHeight, playerHeight, playerWidth);
                attack = true;
            }
            if (ks.IsKeyUp(Keys.J))
            {
                attack = false;
            }
        }
    //Player Skill
    public void PlayerSkill(KeyboardState ks, GameTime gametime , AnimatedTexture animated ,AnimatedTexture skillanimate)
        {
            if (ks.IsKeyDown(Keys.E) && action == 2 && !Skill && stamina == 3) //Left Skill
            {
                SkillDirection = "Left";
                Skill = true;
                skillpress = true;
                skilltime = true;
                stamina = 0;
                skillPos.X = playerPos.X;
                skillPos.Y = playerPos.Y+10;
            }
            else if (ks.IsKeyDown(Keys.E) && action == 1 && !Skill && stamina == 3) // Right Skill
            {
                SkillDirection = "Right";
                Skill = true;
                skillpress = true;
                skilltime = true;
                stamina = 0;
                skillPos.X = playerPos.X+8;
                skillPos.Y = playerPos.Y+10;
            }
            else if (ks.IsKeyDown(Keys.E) && action == 4 && !Skill && stamina == 3) //Up Skill
            {

                Skill = true;
                skillpress = true;
                skilltime = true;
                stamina = 0;
                skillPos.X = playerPos.X+4;
                skillPos.Y = playerPos.Y;
            }
            else if (ks.IsKeyDown(Keys.E) && action == 1 && !Skill && stamina == 3) //Down Skill
            {

                Skill = true;
                skillpress = true;
                skilltime = true;
                stamina = 0;
                skillPos.X = playerPos.X+4;
                skillPos.Y = playerPos.Y;
            }
            if (skilltime)
            {
                SkillTime(gametime , skillanimate);
            }
            if (skillpress)
            {
                SkillPress(gametime , animated, skillanimate);
            }
            if(ks.IsKeyDown (Keys.R)) 
            {
                skillanimate.Reset();
            }
        }
        //Reset Skillposition
        public void SkillPress(GameTime gametime , AnimatedTexture animated ,AnimatedTexture skillanimate)
        {
            skilltimer += (float)gametime.ElapsedGameTime.TotalMilliseconds / 1000;
            if (skilltimer >= 1)
            {
                skilltimer = 0;
                skillpress = false;
                Skill = false;
            }
        }
        //SkillTime
        public void SkillTime(GameTime gametime , AnimatedTexture skillanimate)
        {
            skilltimer += (float)gametime.ElapsedGameTime.TotalMilliseconds / 1000;
            //Console.WriteLine(skilltimer);
            if (skilltimer >= 2)
            {
                skilltimer = 0;
                stamina = 3;
                skilltime = false;
                skillanimate.Reset();
            }

        }

        public bool AttackCollision(Rectangle enemy)
        {
            return attackBox.Intersects(enemy);
        }
        public bool SkillCollision(Rectangle enemy)
        {
            return skillBox.Intersects(enemy);
        }
        public bool GetPlayerAttack()
        {
            return attack;
        }
        public bool GetPlayerSkill()
        {
            return Skill;
        }
        public bool GetSkillTime()
        {
            return skilltime;
        }
        public Vector2 GetPlayerPos()
        {
            return playerPos;
        }
        public Vector2 GetSkillPos()
        {
            return skillPos;
        }
        public bool GetIdle()
        {
            return idle;
        }
        public int GetAction()
        {
            return action;
        }
        public int GetPlayerSpeed()
        {
            return speed;
        }
        public Rectangle GetAttackRec()
        {
            return attackBox;
        }
        public String GetDirection()
        {
            return Direction;
        }
        public String GetSkillDirection()
        {
            return SkillDirection;
        }
    }

    
    
}
