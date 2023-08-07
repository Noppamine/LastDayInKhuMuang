using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LastDayInKhuMuang
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        //Input
        private KeyboardState ks;

        //Player
        private int hp = 100;
        private int speed = 4;
        private int boostSpeed = 5;
        private Vector2 playerPos = new Vector2(200, 200);
        Player player;

        //PlayerAnimation
        private AnimatedTexture playerAnimate;
        private const float Rotation = 0;
        private const float Scale = 1.0f;
        private const float Depth = 0.5f;
        private const int Frames = 4;
        private const int FramesPerSec = 12;
        private const int FramesRow = 4;

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
            _graphics.PreferredBackBufferHeight = 600;
            _graphics.PreferredBackBufferWidth = 800;

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
            player = new Player(speed, boostSpeed, hp, playerPos);
            playerAnimate.Load(Content, "Char01", Frames, FramesRow, FramesPerSec);

            //Font
            text = Content.Load<SpriteFont>("Test");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            player.PlayerMove(ks, _graphics, playerAnimate);
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            getElapsed = elapsed;
            player.SetElapsed(elapsed);
            playerPos = player.GetPlayerPos();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);


            _spriteBatch.Begin();
            if (!player.GetIdle())
            {
                playerAnimate.DrawFrame(_spriteBatch, playerPos, player.GetAction());
            }
            else
            {
                playerAnimate.DrawFrame(_spriteBatch, 0, playerPos, player.GetAction());
            }
            //_spriteBatch.DrawString(text, "" + getElapsed, new Vector2(400,0), Color.Black);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}