using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geomystery.Award
{
    enum Stars { OneStar = 1, TwoStar = 2, ThreeStar = 3 };
    /// <summary>
    /// 设置一些章节相关信息与函数
    /// </summary>
    class Level
    {
        /// <summary>
        /// 关卡编号
        /// </summary>
        public int Level_ID;
        /// <summary>
        /// 是否已通过
        /// </summary>
       private bool Passed;
        /// <summary>
        /// 获得星星数
        /// </summary>
        private Stars stars;
         void LevelStart()
        { }
        /// <summary>
        /// 关卡通过后，修改关卡状态，修改星星数
        /// </summary>
        /// <param name="NewStars"></param>
        private void LevelPass(Stars NewStars)
        {
            if (Passed == false)
            {
                Passed = true;
                Geo_Coin.Geo.GetGeo_Coins(1);
            }
            if (NewStars > stars)
            {
                for (int i = 0; i < (NewStars - stars); i++)
                {
                    Geo_Coin.Geo.GetGeo_Coins(2);
                }
                stars = NewStars;
            }     
        }  
    }
}
