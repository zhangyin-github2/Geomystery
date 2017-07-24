using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geomystery.Award
{
    enum Stars { NoStars=0, OneStar = 1, TwoStar = 2, ThreeStar = 3 };
    /// <summary>
    /// 设置一些章节相关信息与函数
    /// </summary>
    public class Level
    {
        /// <summary>
        /// 关卡编号
        /// </summary>
        public int ID;
        /// <summary>
        /// 关卡名称
        /// </summary>
        public string name;
        /// <summary>
        /// 关卡封面
        /// </summary>
        public string cover;
        /// <summary>
        /// 是否已解锁
        /// </summary>
        public int unlocked { get; set; }
        /// <summary>
        /// 是否已通过
        /// </summary>
        private bool Passed;
        /// <summary>
        /// 获得星星数
        /// </summary>
        private Stars stars;
        /// <summary>
        /// 构造函数
        /// </summary>
        public Level()
        {
            ID = 0;
            name = "";
            cover = "Picture/lock.png";
            unlocked = 0;
            Passed = false;
            stars = Stars.NoStars;
        }
        /// <summary>
        /// 关卡初始化载入
        /// </summary>
        public void LevelStart()
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
        public static ObservableCollection<Level> getLevels()
        {
            ObservableCollection<Level> cp1 = new ObservableCollection<Level>();
            cp1.Add(new Level() { ID = 1, name = "Draw Line", unlocked = 1 });
            cp1.Add(new Level() { ID = 2, name = "Draw Circle" });
            cp1.Add(new Level() { ID = 3, name = "Draw Point" });
            cp1.Add(new Level() { ID = 4, name = "Angle of 60°" });
            cp1.Add(new Level() { ID = 5, name = "Perpendicular Bisector" });
            cp1.Add(new Level() { ID = 6, name = "Mid Point" });
            cp1.Add(new Level() { ID = 7, name = "Ciricl in Square" });
            cp1.Add(new Level() { ID = 8, name = "Rhombus in Rectangle" });
            cp1.Add(new Level() { ID = 9, name = "Circle Center" });
            foreach(var x in cp1)
            {
                x.cover = "ms-appx:///Pictures/Levels/" + x.ID.ToString()+".png";
            }
            return cp1;
        }
    }
}
