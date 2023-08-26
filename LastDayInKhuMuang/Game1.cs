using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using MonoGame.Extended.ViewportAdapters;
using MonoGame.Extended;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Collisions;
using System.Collections.Generic;

namespace LastDayInKhuMuang
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        //SetMapsize
        const int MapWidth = 1920;
        const int MapHeight = 1080;

        //Scenes
        public FrontHomeScene mFrontHomeScene;
        public Scenes mCurrentScene;

        //Scense
        private Texture2D homeScenes;
        private SimpleChangeScenes changeScenes;

        ////Camera
        public static OrthographicCamera camera;
        public static Vector2 cameraPos;
        public static Vector2 bgPos;
        private int zoom = 3;

        //Input
        private KeyboardState ks;
        private MouseState ms;
        //private KeyboardState oldks;

        //Player
        private int hp = 100;
        private int speed = 3;
        private int boostSpeed = 5;
        private int stamina = 3;
        private int action;
        private Vector2 playerPos = new Vector2(200, 200);
        Rectangle playerBox = new Rectangle();
        Player player;
        //private readonly List<IEntity> entities = new List<IEntity>();
        //public readonly CollisionComponent collisionComponent;

        //Skill
        private Vector2 skillPos = new Vector2(200, 200);
        private Vector2 skillspeed = new Vector2(1, 1);
        Rectangle skillBox = new Rectangle();
        private string SkillDirection;

        //PlayerAnimation
        private AnimatedTexture playerAnimate;
        private const float Rotation = 0;
        private const float Scale = 1.0f;
        private const float Depth = 0.5f;
        private const int Frames = 10;
        private const int FramesPerSec = 12;
        private const int FramesRow = 8;

        //AttackAnimation
        private AnimatedTexture AttackAnimate;
        private const float AttackRotation = 0;
        private const float AttackScale = 1.0f;
        private const float AttackDepth = 0.5f;
        private const int AttackFrames = 10;
        private const int AttackFramesPerSec = 12;
        private const int AttackFramesRow = 2;

        //SkillAnimation
        private AnimatedTexture SkillAnimate;
        private const float SkillRotation = 0;
        private const float SkillScale = 1.0f;
        private const float SkillDepth = 0.5f;
        private const int SkillFrames = 4;
        private const int SkillFramesPerSec = 6;
        private const int SkillFramesRow = 4;

        //Enemy
        private Texture2D ball;
        private Bosstwo boss2;
        private const int bossHp = 100;
        

        //Font
        private SpriteFont text;
        private float getElapsed;

        public Game1()
        {
            //Game Setting
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.IsFullScreen = false;

            //Set Screen
            //_graphics.PreferredBackBufferWidth = MapWidth;
            //_graphics.PreferredBackBufferHeight = MapHeight;
            //_graphics.PreferredBackBufferHeight = MapHeight / 2;
            //_graphics.PreferredBackBufferWidth = MapWidth / 2;

            //Set Player Collision
            //collisionComponent = new CollisionComponent(new RectangleF(200, 200, MapWidth, MapHeight));

            //Set Sprite
            playerAnimate = new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth);
            AttackAnimate = new AnimatedTexture(Vector2.Zero, AttackRotation,AttackScale,AttackDepth);
            SkillAnimate = new AnimatedTexture(Vector2.Zero,SkillRotation,SkillScale,SkillDepth);
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferHeight = MapHeight ;
            _graphics.PreferredBackBufferWidth = MapWidth;
            _graphics.ApplyChanges();
            GraphicsDevice.BlendState = BlendState.AlphaBlend;

            ViewportAdapter viewportadpter = new BoxingViewportAdapter(Window, GraphicsDevice, MapWidth/zoom, MapHeight/zoom);
            camera = new OrthographicCamera(viewportadpter);
            bgPos = new Vector2(0, 0);
            
            //bgPos = new Vector2(0, 0);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //Load Player Content
            player = new Player(speed, boostSpeed, hp, stamina, playerPos, skillPos);
            playerAnimate.Load(Content, "Player_all_set", Frames, FramesRow, FramesPerSec);
            AttackAnimate.Load(Content, "Effect_Attack", AttackFrames, AttackFramesRow, AttackFramesPerSec);
            SkillAnimate.Load(Content,"set-Skill-E",SkillFrames,SkillFramesRow,SkillFramesPerSec);
            player.SetAction(1);
            //Setup player
            //entities.Add(new PlayerEntity(this, new RectangleF(new Point2(32, 470), new Size2(56, 56))));
            //foreach (IEntity entity in entities)
            //{
            //    collisionComponent.Insert(entity);
            //}

            //Load Enemy Content
            ball = Content.Load<Texture2D>("Resources/ball");
            boss2 = new Bosstwo(bossHp, player.GetPlayerPos(), _spriteBatch, this);
            boss2.BossLoad();
            boss2.SetBossPos(_graphics);

            //Load Map Content
            //homeScenes = Content.Load<Texture2D>("Resources/draft-map");
            //mFrontHomeScene = new FrontHomeScene(this, new EventHandler());
            //mCurrentScene = mFrontHomeScene;
            changeScenes = new SimpleChangeScenes(this, _spriteBatch);
            changeScenes.LoadScenes();

            //Font
            //text = Content.Load<SpriteFont>("Resources/Test");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            ms = Mouse.GetState();

            //Player            
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            getElapsed = elapsed;
            player.SetElapsed(elapsed);
            //foreach (IEntity entity in entities)
            //{
            //    entity.Update(gameTime);
            //}
            //collisionComponent.Update(gameTime);
            player.PlayerMove(ks, _graphics, playerAnimate,AttackAnimate, SkillAnimate,gameTime);
            playerPos = player.GetPlayerPos();
            //player Hitbox
            playerBox = new Rectangle((int)playerPos.X + 20,(int)playerPos.Y + 96,88,32);
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
            if (action == 1) // right skill
            {
                skillPos.X = skillPos.X + 10;
            }
            if (action == 5) // up skill
            {
                skillPos.Y = skillPos.Y - 10;
            }
            if (action == 6) // down skill
            {
                skillPos.Y = skillPos.Y + 10;
            }
            skillBox = new Rectangle((int)skillPos.X, (int)skillPos.Y, 128, 128);

            //Camera
            if (changeScenes.GetScenes() == 1)
            {
                ViewportAdapter viewportadpter = new BoxingViewportAdapter(Window, GraphicsDevice, MapWidth / zoom, MapHeight / zoom);
                camera = new OrthographicCamera(viewportadpter);               
                camera.LookAt(cameraPos);
                UpdateCamera();
            }
            else if (changeScenes.GetScenes() == 2)
            {
                boss2.BossUpdate(player.GetPlayerPos(), gameTime);
                //player.SetPosition(new Vector2(200,200));
                ViewportAdapter viewportadpter = new BoxingViewportAdapter(Window, GraphicsDevice, MapWidth, MapHeight);
                camera = new OrthographicCamera(viewportadpter);
                camera.LookAt(bgPos + new Vector2(MapWidth / 2, MapHeight / 2));
                //camera.LookAt(bgPos + new Vector2(MapWidth / 2, MapHeight / 2));
                UpdateCamera();
            }
            

            //Scenes Update
            changeScenes.UpdateScenes(ks);

            base.Update(gameTime);
        }
        Rectangle ballRectangle = new Rectangle(250, 250, 24, 24);


        protected override void Draw(GameTime gameTime)
        {
            if (player.GetPlayerAttack() && player.AttackCollision(ballRectangle))
            {
                GraphicsDevice.Clear(Color.Red);
            }
            else if (player.GetSkillTime() && skillBox.Intersects(ballRectangle))
            {
                GraphicsDevice.Clear(Color.Red);
            }
            else if (playerBox.Intersects(ballRectangle))
            {
                GraphicsDevice.Clear(Color.Red);
            }
            else
            {
                GraphicsDevice.Clear(Color.CornflowerBlue);
            }

            //Draw begin here
            var transformMatrix = camera.GetViewMatrix();
            _spriteBatch.Begin(transformMatrix: transformMatrix);
            changeScenes.DrawScenes();
            //_spriteBatch.Draw(homeScenes, new Vector2(0, 0), null, Color.White, 0f, Vector2.Zero, new Vector2(2.5f, 2.5f), 0, 0);
            //foreach (IEntity entity in entities)
            //{
            //    entity.Draw(_spriteBatch);
            //}

            //Draw Player            
            if (!player.GetIdle())
            {
                playerAnimate.DrawFrame(_spriteBatch, playerPos, player.GetAction());
            }
            else if (player.GetIdle())
            {
                if (player.GetPlayerAttack() && player.GetDirection() == "Right") // right attack animate
                {
                    playerAnimate.DrawFrame(_spriteBatch, playerPos, 3);
                }
                else if (player.GetPlayerAttack() && player.GetDirection() == "Left") // left attack animate
                {
                    playerAnimate.DrawFrame(_spriteBatch, playerPos, 4);
                }
                else if (player.GetPlayerSkill() && player.GetSkillDirection() == "Left") // left skill attack animate
                {
                    playerAnimate.DrawFrame(_spriteBatch, playerPos, 4);
                }
                else if (player.GetPlayerSkill() && player.GetSkillDirection() == "Right") // right skill attack animate
                {
                    playerAnimate.DrawFrame(_spriteBatch, playerPos, 3);
                }
                else if (player.GetPlayerSkill() && player.GetSkillDirection() == "Up") // up skill attack animate
                {
                    playerAnimate.DrawFrame(_spriteBatch, playerPos, 3);
                }
                else if (player.GetPlayerSkill() && player.GetSkillDirection() == "Down") // down skill attack animate
                {
                    playerAnimate.DrawFrame(_spriteBatch, playerPos, 3);
                }
                else if (player.GetDirection() == "Left" && ks.IsKeyUp(Keys.D))
                {
                    playerAnimate.DrawFrame(_spriteBatch, playerPos, 8 );
                }
                else if (player.GetDirection() == "Right" && ks.IsKeyUp(Keys.A))
                {
                    playerAnimate.DrawFrame(_spriteBatch, playerPos, 7);
                }
                else
                {
                    playerAnimate.DrawFrame(_spriteBatch, playerPos, 7);
                }
            }


            //Draw Skill
            if (player.GetSkillTime())
            {
                if (skillPos.X > 0 && skillPos.X < _graphics.GraphicsDevice.Viewport.Width && skillPos.Y > 0 && skillPos.Y < _graphics.GraphicsDevice.Viewport.Width)
                {
                    //_spriteBatch.Draw(ball, new Vector2(skillPos.X, skillPos.Y), skillBox, Color.White);
                    //_spriteBatch.Draw(ball, new Vector2(skillPos.X, skillPos.Y), new Rectangle(0, 0, 24, 24), Color.White);

                    if (player.GetSkillDirection() == "Right") // right skill
                    {
                        SkillAnimate.DrawFrame(_spriteBatch, new Vector2(skillPos.X, skillPos.Y), 1);
                        if(SkillAnimate.IsEnd)
                        {
                            SkillAnimate.DrawFrame(_spriteBatch, 4, new Vector2(skillPos.X, skillPos.Y), 1);
                            SkillAnimate.Pause(4,1);
                        }
                        else
                        {
                            SkillAnimate.DrawFrame(_spriteBatch, new Vector2(skillPos.X, skillPos.Y), 1);
                        }
                    }
                    if (player.GetSkillDirection() == "Left" ) // left skill
                    {
                        SkillAnimate.DrawFrame(_spriteBatch, new Vector2(skillPos.X, skillPos.Y), 2);
                    }
                    if (player.GetSkillDirection() == "Up") // up skill
                    {
                        SkillAnimate.DrawFrame(_spriteBatch, new Vector2(skillPos.X, skillPos.Y), 3);
                    }
                    if (player.GetSkillDirection() == "Down") // down skill
                    {
                        SkillAnimate.DrawFrame(_spriteBatch, new Vector2(skillPos.X, skillPos.Y), 4);
                    }
                }
            }

            //Draw AttackAnimate
            if (player.GetPlayerAttack()&& player.GetDirection() == "Right") // right attack
            {
                playerAnimate.DrawFrame(_spriteBatch, playerPos,3);
                AttackAnimate.DrawFrame(_spriteBatch,playerPos,1);
            }
            if (player.GetPlayerAttack() && player.GetDirection() == "Left") // right attack
            {
                playerAnimate.DrawFrame(_spriteBatch, playerPos, 4);
                AttackAnimate.DrawFrame(_spriteBatch, playerPos, 2);
            }

            //Draw Scenes
            if (changeScenes.GetScenes() == 1)
            {                
                _spriteBatch.Draw(ball, new Vector2(250, 250), new Rectangle(0, 0, 24, 24), Color.White);
            }
            else if (changeScenes.GetScenes() == 2)
            {
                boss2.BossDraw();
            }
            

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        public void UpdateCamera()
        {
            cameraPos = player.GetPlayerPos() + new Vector2(15, 22);
        }
        public int GetMapWidth()
        {
            return MapWidth;
        }
        public int GetMapHeight()
        {
            return MapHeight;
        }
    }
}
   