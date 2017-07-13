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
        public List<Point2> PointList { get; set; }

        public List<Line> LineList { get; set; }
    }
}
