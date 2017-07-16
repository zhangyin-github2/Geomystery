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
    }
}
