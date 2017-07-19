using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geomystery.Models.Geometry
{
    //“几何实体”，是点线圆的抽象基类
    public abstract class Geometry
    {
        /// <summary>
        /// 实体所在坐标系，实体与坐标系绑定
        /// </summary>
        public Coordinate coord { get; set; }

        /// <summary>
        /// 这个元素是否被选中
        /// </summary>
        public bool isSelected { get; set; }


        /// <summary>
        /// 依赖于什么点集(Line，Circle)，比如点在直线上，点在圆上，就是点对直线或者圆的依赖，交点会依赖相交的直线或者圆
        /// </summary>
        public List<Geometry> rely { get; set; }

        /// <summary>
        /// 哪些点集受此点影响，此点一定是受影响直线或圆的定义点(p1, p2, center, radius)
        /// </summary>
        public List<Geometry> influence { get; set; }
    }
}
