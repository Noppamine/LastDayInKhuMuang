using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastDayInKhuMuang
{
    public class MeleeEnemy : Enemy
    {
        private Vector2 playerPos;

        //For test
        private Texture2D enemy;
        private Color[] data;

        public MeleeEnemy(Vector2 playerPos, int hp, int speed) : base(playerPos)
        {
            this.playerPos = playerPos; 
            this.speed = speed;
            this.hp = hp;
            width = 128;
            height = 150;
            range = new Rectangle((int)position.X, (int)position.Y, 400, 400);
            position = new Vector2(500, 500);

        }

        public void Load(Game1 game)
        {
            enemy = new Texture2D(game.GraphicsDevice, width, height);
            data = new Color[128*150];

            

            for (int i=0; i<data.Length; i++)
            {
                data[i] = Color.Red;
            }
        }

        public void Update(float elapsed)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
