using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace LastDayInKhuMuang
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        //Input
        private KeyboardState ks;
        private KeyboardState oldks;

        //Player
        private int hp = 100;
        private int speed = 3;
        private int boostSpeed = 5;
        private int stamina = 3;
        private int action;
        private Vector2 playerPos = new Vector2(200, 200);
        Player player;

        //Skill
        private Vector2 skillPos = new Vector2(200, 200);
        private Vector2 skillspeed = new Vector2(1, 1);

        //PlayerAnimation
        private AnimatedTexture playerAnimate;
        private const float Rotation = 0;
        private const float Scale = 1.0f;
        private const float Depth = 0.5f;
        private const int Frames = 4;
        private const int FramesPerSec = 12;
        private const int FramesRow = 4;

        //Enemy
        Texture2D ball;

        //Font
        private SpriteFont text;
        private float getElapsed;
        public Game1()
        {
            //Game Setting
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            //Set Screen
            //_graphics.PreferredBackBufferHeight = 600;
            //_graphics.PreferredBackBufferWidth = 800;

            //Set Sprite
            playerAnimate = new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth);
        }

        protected override void Initialize()
        {


            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //Load Player Content
            player = new Player(speed, boostSpeed, hp, stamina, playerPos ,skillPos);
            playerAnimate.Load(Content, "Char01", Frames, FramesRow, FramesPerSec);
            player.SetAction(1);

            //Load Enemy Content
            ball = Content.Load<Texture2D>("ball");

            //Font
            text = Content.Load<SpriteFont>("Test");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //Player            
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            getElapsed = elapsed;
            player.SetElapsed(elapsed);
            player.PlayerMove(ks, _graphics, playerAnimate, gameTime);
            playerPos = player.GetPlayerPos();
            //Skill Direction Update Time
            if (player.GetPlayerSkill())
            {
                skillPos = player.GetSkillPos();
                action = player.GetAction();
            }
            if (action == 2) // left skill
            {
                skillPos.X = skillPos.X - 10;
            }
            if (action == 3) // right skill
            {
                skillPos.X = skillPos.X + 10;
            }
            if (action == 4) // up skill
            {
                skillPos.Y = skillPos.Y - 10;
            }
            if (action == 1) // down skill
            {
                skillPos.Y = skillPos.Y + 10;
            }

            base.Update(gameTime);
        }
        Rectangle ballRectangle = new Rectangle(250, 250, 24, 24);
        Rectangle skillBox = new Rectangle(0,0, 24, 24);
        protected override void Draw(GameTime gameTime)
        {
            if (player.GetPlayerAtack() && player.AttackCollision(ballRectangle))
            {
                GraphicsDevice.Clear(Color.Red);
            }
            else if (player.GetSkillTime() && player.SkillCollision(ballRectangle))
            {
                GraphicsDevice.Clear(Color.Red);
            }
            else
            {
                GraphicsDevice.Clear(Color.CornflowerBlue);
            }



            _spriteBatch.Begin();

            if (!player.GetIdle())
            {
                playerAnimate.DrawFrame(_spriteBatch, playerPos, player.GetAction());
            }
            else if (player.GetIdle())
            {
                playerAnimate.DrawFrame(_spriteBatch, 0, playerPos, player.GetAction());
            }
            //Draw Skill
            if (player.GetSkillTime())
            {
                _spriteBatch.Draw(ball, new Vector2(skillPos.X, skillPos.Y),skillBox, Color.White);
            }

            _spriteBatch.Draw(ball, new Vector2(250, 250), new Rectangle(0, 0, 24, 24), Color.White);

            _spriteBatch.End();

                base.Draw(gameTime);
            }
        }
}   