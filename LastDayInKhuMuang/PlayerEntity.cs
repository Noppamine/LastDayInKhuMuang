using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastDayInKhuMuang
{
    public class PlayerEntity : IEntity
    {
        private readonly Game1 game;

        public int velocity = 4;
        Vector2 move;
        public IShapeF Bounds { get; }
        private KeyboardState currentKey;
        private KeyboardState oldKey;
        public PlayerEntity(Game1 game, IShapeF circleF)
        {
            this.game = game;
            Bounds = circleF;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawRectangle((RectangleF)Bounds, Color.Red, 3f);
        }
        public void OnCollision(CollisionEventArgs collisionInfo)
        {
            //throw new NotImplementedException();
        }

        public void Update(GameTime gameTime)
        {
            currentKey = Keyboard.GetState();
            if (currentKey.IsKeyDown(Keys.D) && Bounds.Position.X < game.GetMapWidth() - ((RectangleF)Bounds).Width)
            {
                move = new Vector2(velocity, 0) * gameTime.GetElapsedSeconds() * 50;
                Bounds.Position += move;
            }
            else if (currentKey.IsKeyDown(Keys.A) && Bounds.Position.X > 0)
            {
                move = new Vector2(-velocity, 0) * gameTime.GetElapsedSeconds() * 50;
                Bounds.Position += move;
            }
        }
    }
}
