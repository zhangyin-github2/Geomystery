using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geomystery.Models.Geometry
{
    /// <summary>
    /// 多边形
    /// </summary>
    public class Polygon : Geometry
    {
        /// <summary>
        /// 构成多边形的点的序列
        /// </summary>
        public List<Point2> PointList { get; set; }

        /// <summary>
        /// 点序列从头到尾两两相连成线段，变成边界
        /// </summary>
        public List<Line> LineList { get; set; }
    }
}
