using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.LogicScripts.Client.Net.PB.Enum
{
    public class PlayerOptEnum
    {
        public static int UNKNOW = 0;
        public static int DOWN = 1;
        public static int DOWN_HOLD = 2;
        public static int UP = 3;

        public static bool IsKeyDown(int key)
        {
            return key == DOWN;
        }
        public static bool IsHoldKey(int key)
        {
            return key == DOWN_HOLD;
        }
        public static bool IsKeyUp(int key)
        {
            return key == UP;
        }
    }
}
