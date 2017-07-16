using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geomystery.Models.Geometry
{
    /// <summary>
    /// 多边形类，多边形由一个点数组定义（也是一个点集，需要注意多边形与直线交点问题）
    /// </summary>
    public class Polygon : Geometry
    {
        /// <summary>
        /// 构成多边形的点的序列
        /// </summary>
        public List<Point2> pointList { get; set; }

        /// <summary>
        /// 点序列从头到尾两两相连成线段，变成边界
        /// </summary>
        public List<Line> lineList { get; set; }
    }
}
