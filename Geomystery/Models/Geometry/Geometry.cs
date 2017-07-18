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
        /// 几何体的ID号，
        /// 屏幕上首先有一个点，后来构造的点与刚才的点重合，很可能两个点的属性完全相同，此时需要一个ID来区分不同的（长得差不多的）元素，
        /// 鼠标点击只能选择存在的点，但是用户的构造操作可能会创建新的与旧的几何元素重合的几何元素，不能把二者视为同一个元素。
        /// </summary>
        public int id { get; set; }

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
