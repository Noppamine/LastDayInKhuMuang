using Microsoft.VisualBasic.Devices;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastDayInKhuMuang
{
    internal class BossOne
    {
        private int hp;
        private Vector2 bossPos;
        private Vector2 playerPos;

        int randomPat;
        //int[] attackPattern;

        private Random rand;
        public BossOne(int hp, Vector2 position, Vector2 playerPos)
        {
            this.hp = hp;
            this.bossPos = position;
            this.playerPos = playerPos;
            rand = new Random();
            //attackPattern = new int[3] {0, 0, 0};
        }
        
        public void AttackPattern()
        {
            //for (int i=0; i<3; i++)
            //{
            //    attackPattern[i] = RandomAttackPattern();
            //}
            randomPat = rand.Next(2);
            switch (randomPat)
            {
                case 1:
                    SmashDown();
                    break;
                case 2:
                    FistHammer();
                    break;
            }
        }

        //public int RandomAttackPattern()
        //{
        //    bool checkLoop = true;
        //    while (checkLoop)
        //    {
        //        randomPat = rand.Next(3);
        //        for (int i=0; i<3; i++)
        //        {
        //            if (attackPattern[i] == randomPat)
        //            {
        //                checkLoop = true;
        //                break;
        //            }
        //        }
        //    }
        //    return randomPat;
        //}

        public void SmashDown()
        {
            int xPos;
            int yPos;
            xPos = (int)playerPos.X;
            yPos = (int) playerPos.Y;
            Rectangle fistHitBox = new Rectangle(xPos, yPos - 500, 50, 50); //Create hitbox check collision with player and deal damage.

            //if (fistHitBox.Intersects(player)) //Check collision with player (Not sure to do this way. On the other way go to check at Player.cs)
            //{
            //    //Deal damage
            //}
            //if (Keyboard.)
            //{

            //}
        }

        public void FistHammer()
        {

        }

        public void ClearFloor()
        {

        }
    }
}
