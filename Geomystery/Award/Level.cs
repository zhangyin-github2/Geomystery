using Geomystery.Controllers.Geometry;
using Geomystery.Models.Geometry;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geomystery.Award
{
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
        /// 关卡说明
        /// </summary>
        public string Discribe;
        /// <summary>
        /// 是否已解锁
        /// </summary>
        public int unlocked { get; set; }
        /// <summary>
        /// 是否已通过
        /// </summary>
        private bool Passed;
        /// <summary>
        /// 构造函数
        /// </summary>
        public Level(int id=0)
        {
            ID = id;
            name = "";
            cover = "Picture/lock.png";
            unlocked = 0;
            Passed = false;
        }
        public LevelLoader LL;
        /// 关卡通过后，修改关卡状态，修改星星数
        /// </summary>
        /// <param name="NewStars"></param>
        private void LevelPass( )
        {
            if (Passed == false)
            {
                Passed = true;
                Geo_Coin.Geo.GetGeo_Coins(1);
            }
                 
        }  
        public static ObservableCollection<Level> getLevels(int start =1)
        {
            start = 1 + start * 9 - 9;
            ObservableCollection<Level> levels = new ObservableCollection<Level>();
            for(int i=start;i<start+9;i++)
            {
                Level x = new Level(i);
                x.name = AppResources.GetString("L" + x.ID.ToString() + "N") ;
                x.Discribe = AppResources.GetString("L" + x.ID.ToString() + "D") ;
                x.cover = "ms-appx:///Pictures/Levels/" + x.ID.ToString()+".png";
                if (APPDATA.app_data.HAVEDONE >= x.ID - 1) x.unlocked = 1;
                levels.Add(x);
            }
            return levels;
        }
    }
}