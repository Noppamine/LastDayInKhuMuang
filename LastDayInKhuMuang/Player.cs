using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Timers;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;



namespace LastDayInKhuMuang
{
    public class Player
    {
        //PlayerAnimation
        static AnimatedTexture playerAnimate;
        private const float Rotation = 0;
        private const float Scale = 1.0f;
        private const float Depth = 0.5f;
        private const int Frames = 10;
        private const int FramesPerSec = 12;
        private const int FramesRow = 8;

        //AttackAnimation
        static AnimatedTexture AttackAnimate;
        private const float AttackRotation = 0;
        private const float AttackScale = 1.0f;
        private const float AttackDepth = 0.5f;
        private const int AttackFrames = 10;
        private const int AttackFramesPerSec = 12;
        private const int AttackFramesRow = 2;

        //SkillAnimation
        static AnimatedTexture SkillAnimate;
        private const float SkillRotation = 0;
        private const float SkillScale = 1.0f;
        private const float SkillDepth = 0.5f;
        private const int SkillFrames = 4;
        private const int SkillFramesPerSec = 6;
        private const int SkillFramesRow = 4;

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
        
        public static void Initialize()
        {
            playerAnimate = new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth);
            AttackAnimate = new AnimatedTexture(Vector2.Zero, AttackRotation, AttackScale, AttackDepth);
            SkillAnimate = new AnimatedTexture(Vector2.Zero, SkillRotation, SkillScale, SkillDepth);
        }
        public void Load(Game1 game)
        {
            playerAnimate.Load(game.Content, "Resources/Player/Player_all_set", Frames, FramesRow, FramesPerSec);
            AttackAnimate.Load(game.Content, "Resources/Player/Effect_Attack", AttackFrames, AttackFramesRow, AttackFramesPerSec);
            SkillAnimate.Load(game.Content, "Resources/Player/set-Skill-E", SkillFrames, SkillFramesRow, SkillFramesPerSec);
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
        public void PlayerMove(KeyboardState ks, GraphicsDeviceManager gp, GameTime gametime, Vector2 skillPos)
        {
            ks = Keyboard.GetState();
            playerAnimate.UpdateFrame(elapsed);
            AttackAnimate.UpdateFrame(elapsed);
            PlayerDash(ks);
            this.skillPos = skillPos;
            if(Skill == true && !SkillAnimate.IsEnd)
            {
                SkillAnimate.UpdateFrame(elapsed);
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
                    action = 5;
                    Direction = "Up";
                }
                if (ks.IsKeyDown(Keys.S) && !attack && !Skill)
                {
                    playerPos.Y += speed;
                    idle = false;
                    action = 6;
                    Direction = "Down";
                }
                if (ks.IsKeyDown(Keys.D) &&(ks.IsKeyDown(Keys.A)) &&!attack && !Skill)
                {
                    idle = true;
                    if(ks.IsKeyDown(Keys.W))
                    {
                        playerPos.Y += speed;
                    }
                    if (ks.IsKeyDown(Keys.S))
                    {
                        playerPos.Y -= speed;
                    }
                }
                if (ks.IsKeyDown(Keys.W) && (ks.IsKeyDown(Keys.S)) && !attack && !Skill)
                {
                    idle = true;
                    if (ks.IsKeyDown(Keys.A))
                    {
                        playerPos.X += speed;
                    }
                    if (ks.IsKeyDown(Keys.D))
                    {
                        playerPos.X -= speed;
                    }
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

            PlayerAttack(ks ,AttackAnimate);
            if (ks.IsKeyUp(Keys.A) && ks.IsKeyUp(Keys.D) && ks.IsKeyUp(Keys.W) && ks.IsKeyUp(Keys.S))
            {
                idle = true;
            }

            PlayerSkill(ks , gametime , playerAnimate , AttackAnimate, SkillAnimate);
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

        //player attack
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
            else if (ks.IsKeyDown(Keys.J) && Direction == "Up" && !attack) //Up Attack
            {
                attackBox = new Rectangle((int)playerPos.X - ((playerHeight-playerWidth)/2), (int)playerPos.Y - playerWidth, playerHeight, playerWidth);
                attack = true;
                idle = true;
            }
            else if (ks.IsKeyDown(Keys.J) && Direction == "Down" && !attack) //Down Attack
            {
                attackBox = new Rectangle((int)playerPos.X - ((playerHeight - playerWidth) / 2), (int)playerPos.Y + playerHeight, playerHeight, playerWidth);
                attack = true;
                idle = true;
            }
            if (ks.IsKeyUp(Keys.J))
            {
                attack = false;
                attackanimate.Reset();
            }
        }
        //Player Skill
        public void PlayerSkill(KeyboardState ks, GameTime gametime , AnimatedTexture animated ,AnimatedTexture attackanimate,AnimatedTexture skillanimate)
        {
            if (ks.IsKeyDown(Keys.E) && action == 2 && !Skill && stamina == 3) //Left Skill
            {
                SkillDirection = "Left";
                Skill = true;
                skillpress = true;
                skilltime = true;
                idle = true;
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
                idle = true;
                stamina = 0;
                skillPos.X = playerPos.X+8;
                skillPos.Y = playerPos.Y+10;
            }
            else if (ks.IsKeyDown(Keys.E) && action == 5 && !Skill && stamina == 3) //Up Skill
            {
                SkillDirection = "Up";
                Skill = true;
                skillpress = true;
                skilltime = true;
                idle = true;
                stamina = 0;
                skillPos.X = playerPos.X+4;
                skillPos.Y = playerPos.Y;
            }
            else if (ks.IsKeyDown(Keys.E) && action == 6 && !Skill && stamina == 3) //Down Skill
            {
                SkillDirection = "Down";
                Skill = true;
                skillpress = true;
                skilltime = true;
                idle = true;
                stamina = 0;
                skillPos.X = playerPos.X+4;
                skillPos.Y = playerPos.Y;
            }
            if (skilltime)
            {
                SkillTime(gametime ,animated,attackanimate, skillanimate);
            }
            if (skillpress)
            {
                SkillPress(gametime , animated, skillanimate);
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
        public void SkillTime(GameTime gametime , AnimatedTexture animated, AnimatedTexture attackanimate, AnimatedTexture skillanimate)
        {
            skilltimer += (float)gametime.ElapsedGameTime.TotalMilliseconds / 1000;
            //Console.WriteLine(skilltimer);
            if (skilltimer >= 2)
            {
                skilltimer = 0;
                stamina = 3;
                skilltime = false;
                skillanimate.Reset();
                attackanimate.Reset();
            }

        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            //Draw Player 
            if (!idle)
            {
                playerAnimate.DrawFrame(spriteBatch, playerPos, action);
            }
            else if (idle)
            {
                if (attack && Direction == "Right") // right attack animate
                {
                    playerAnimate.DrawFrame(spriteBatch, playerPos, 3);
                }
                else if (attack && Direction == "Left") // left attack animate
                {
                    playerAnimate.DrawFrame(spriteBatch, playerPos, 4);
                }
                else if (attack && Direction == "Up") // up attack animate
                {
                    playerAnimate.DrawFrame(spriteBatch, playerPos, 3);
                }
                else if (attack && Direction == "Down") // down attack animate
                {
                    playerAnimate.DrawFrame(spriteBatch, playerPos, 3);
                }
                else if (Skill && Direction == "Left") // left skill attack animate
                {
                    playerAnimate.DrawFrame(spriteBatch, playerPos, 4);
                }
                else if (Skill && Direction == "Right") // right skill attack animate
                {
                    playerAnimate.DrawFrame(spriteBatch, playerPos, 3);
                }
                else if (Skill && Direction == "Up") // up skill attack animate
                {
                    playerAnimate.DrawFrame(spriteBatch, playerPos, 3);
                }
                else if (Skill && Direction == "Down") // down skill attack animate
                {
                    playerAnimate.DrawFrame(spriteBatch, playerPos, 3);
                }
                else if (Direction == "Left" && Keyboard.GetState().IsKeyUp(Keys.D))
                {
                    playerAnimate.DrawFrame(spriteBatch, playerPos, 8);
                }
                else if (Direction == "Right" && Keyboard.GetState().IsKeyUp(Keys.A))
                {
                    playerAnimate.DrawFrame(spriteBatch, playerPos, 7);
                }
                else
                {
                    playerAnimate.DrawFrame(spriteBatch, playerPos, 7);
                }
            }

            //Draw Skill
            if (skilltime)
            {
                if (skillPos.X > 0 && skillPos.X < graphics.GraphicsDevice.Viewport.Width && skillPos.Y > 0 && skillPos.Y < graphics.GraphicsDevice.Viewport.Width)
                {
                    //_spriteBatch.Draw(ball, new Vector2(skillPos.X, skillPos.Y), skillBox, Color.White);
                    //_spriteBatch.Draw(ball, new Vector2(skillPos.X, skillPos.Y), new Rectangle(0, 0, 24, 24), Color.White);

                    if (SkillDirection == "Right") // right skill
                    {
                        SkillAnimate.DrawFrame(spriteBatch, new Vector2(skillPos.X, skillPos.Y), 1);
                        if (SkillAnimate.IsEnd)
                        {
                            SkillAnimate.DrawFrame(spriteBatch, 4, new Vector2(skillPos.X, skillPos.Y), 1);
                            SkillAnimate.Pause(4, 1);
                        }
                        else
                        {
                            SkillAnimate.DrawFrame(spriteBatch, new Vector2(skillPos.X, skillPos.Y), 1);
                        }
                    }

                    if (SkillDirection == "Left") // left skill
                    {
                        SkillAnimate.DrawFrame(spriteBatch, new Vector2(skillPos.X, skillPos.Y), 2);
                    }
                    if (SkillDirection == "Up") // up skill
                    {
                        SkillAnimate.DrawFrame(spriteBatch, new Vector2(skillPos.X, skillPos.Y), 3);
                    }
                    if (SkillDirection == "Down") // down skill
                    {
                        SkillAnimate.DrawFrame(spriteBatch, new Vector2(skillPos.X, skillPos.Y), 4);
                    }
                }
            }

            //Draw AttackAnimate
            if (attack && Direction == "Right") // right attack
            {
                playerAnimate.DrawFrame(spriteBatch, playerPos, 3);
                AttackAnimate.DrawFrame(spriteBatch, playerPos, 1);
            }
            if (attack && Direction == "Left") // left attack
            {
                playerAnimate.DrawFrame(spriteBatch, playerPos, 4);
                AttackAnimate.DrawFrame(spriteBatch, playerPos, 2);
            }
            if (attack && Direction == "Up") // up attack
            {
                playerAnimate.DrawFrame(spriteBatch, playerPos, 3);
                AttackAnimate.DrawFrame(spriteBatch, playerPos, 1);
            }
            if (attack && Direction == "Down") // down attack
            {
                playerAnimate.DrawFrame(spriteBatch, playerPos, 3);
                AttackAnimate.DrawFrame(spriteBatch, playerPos, 1);
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
