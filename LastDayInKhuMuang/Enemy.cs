using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastDayInKhuMuang
{
    public abstract class Enemy
    {
        protected int hp;
        protected int speed;
        protected int width;
        protected int height;
        protected Vector2 position;
        protected Rectangle enemyRec;
        protected Rectangle range;

        protected AnimatedTexture animated;


        public Enemy(Vector2 playerPos)
        {

        }
    }
}
