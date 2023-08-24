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
        Player player;
        //private readonly List<IEntity> entities = new List<IEntity>();
        //public readonly CollisionComponent collisionComponent;

        //Skill
        private Vector2 skillPos = new Vector2(200, 200);
        private Vector2 skillspeed = new Vector2(1, 1);
        Rectangle skillBox = new Rectangle();

        //PlayerAnimation
        private AnimatedTexture playerAnimate;
        private const float Rotation = 0;
        private const float Scale = 1.0f;
        private const float Depth = 0.5f;
        private const int Frames = 4;
        private const int FramesPerSec = 12;
        private const int FramesRow = 4;

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
            _graphics.IsFullScreen = true;

            //Set Screen
            //_graphics.PreferredBackBufferWidth = MapWidth;
            //_graphics.PreferredBackBufferHeight = MapHeight;
            //_graphics.PreferredBackBufferHeight = MapHeight / 2;
            //_graphics.PreferredBackBufferWidth = MapWidth / 2;

            //Set Player Collision
            //collisionComponent = new CollisionComponent(new RectangleF(200, 200, MapWidth, MapHeight));

            //Set Sprite
            playerAnimate = new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth);            
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferHeight = MapHeight;
            _graphics.PreferredBackBufferWidth = MapWidth;
            _graphics.ApplyChanges();
            GraphicsDevice.BlendState = BlendState.AlphaBlend;

            ViewportAdapter viewportadpter = new BoxingViewportAdapter(Window, GraphicsDevice, MapWidth / zoom, MapHeight / zoom);
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
            playerAnimate.Load(Content, "Resources/Char01", Frames, FramesRow, FramesPerSec);
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
            skillBox = new Rectangle((int)skillPos.X, (int)skillPos.Y, 24, 24);

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
                boss2.BossUpdate();
                //player.SetPosition(new Vector2(200,200));
                ViewportAdapter viewportadpter = new BoxingViewportAdapter(Window, GraphicsDevice, MapWidth, MapHeight);
                camera = new OrthographicCamera(viewportadpter);
                camera.LookAt(bgPos + new Vector2(MapWidth/2, MapHeight/2));
            }
            

            //Scenes Update
            changeScenes.UpdateScenes(ks);

            
            base.Update(gameTime);
        }
        Rectangle ballRectangle = new Rectangle(250, 250, 24, 24);


        protected override void Draw(GameTime gameTime)
        {
            if (player.GetPlayerAtack() && player.AttackCollision(ballRectangle))
            {
                GraphicsDevice.Clear(Color.Red);
            }
            else if (player.GetSkillTime() && skillBox.Intersects(ballRectangle))
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
                playerAnimate.DrawFrame(_spriteBatch, 0, playerPos, player.GetAction());
            }


            //Draw Skill
            if (player.GetSkillTime())
            {
                if (skillPos.X > 0 && skillPos.X < _graphics.GraphicsDevice.Viewport.Width || skillPos.Y > 0 && skillPos.Y < _graphics.GraphicsDevice.Viewport.Width)
                {
                    _spriteBatch.Draw(ball, new Vector2(skillPos.X, skillPos.Y), skillBox, Color.White);
                    _spriteBatch.Draw(ball, new Vector2(skillPos.X, skillPos.Y), new Rectangle(0, 0, 24, 24), Color.White);
                }
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
   