using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geomystery.Award
{
    
    /// <summary>
    /// 几何币
    /// </summary>
     class Geo_Coin
    {
        public static Geo_Coin Geo = new Geo_Coin();
        /// <summary>
        /// 金币量
        /// </summary>
        public int Coins;
        /// <summary>
        /// 通过关卡获得几何币
        /// </summary>
        public void GetGeo_Coins(int Flag)
        {
            if (Flag == 1)
            {
                Coins += 200;
            }
            else if (Flag == 2)
            {
                Coins += 50;
            }
            else Coins += 0;
        }

        /// <summary>
        /// 将几何币显示在金币栏中
        /// </summary>
        /// <returns>Coins</returns>
        public int ShowGeo_Coins()
        {
            return Coins;
        }

        /// <summary>
        /// 检查当前金币余量是否足够解锁新关卡
        /// </summary>
        /// <returns>Flag</returns>
        private bool CheckGeo_Coins()
        {
            bool Flag;
            if (Coins>=500)
                Flag = true;
            else Flag = false;
            return Flag;
        }

        /// <summary>
        /// 如果金币余量足够，则返回TRUE，并消耗金币解锁关卡；反之，则返回FALSE。
        /// </summary>
        /// <returns></returns>
        public bool PayGeo_Coins()
        {
            bool Flag;
            if (CheckGeo_Coins() == true)
            {
                Coins -= 500;
                Flag = true;
            }
            else Flag = false;
                return Flag;
        } 
    }
}

