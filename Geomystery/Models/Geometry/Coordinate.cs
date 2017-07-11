using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geomystery.Models.Geometry
{
    public class Coordinate
    {
        //坐标系中的点
        public List<Point2> PointList { get; set; }

        //坐标系中的所有点集
        public List<IPointSet> PointSetList { get; set; }

        //坐标系中的所有多边形
        public List<Polygon> PolygonList { get; set; }
    }
}
