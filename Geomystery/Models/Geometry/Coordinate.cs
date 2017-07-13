using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geomystery.Models.Geometry
{
    /// <summary>
    /// 逻辑坐标系，一张无限大的白纸，可以把其中的一个矩形区域（OutputCoordinate）“投射”到屏幕上
    /// </summary>
    public class Coordinate
    {
        /// <summary>
        /// 坐标系中所有的点
        /// </summary>
        public List<Point2> PointList { get; set; }

        /// <summary>
        /// 坐标系中的所有的点集（实现点集IPointSet的类有直线Line和圆Circle）
        /// </summary>
        public List<IPointSet> PointSetList { get; set; }

        /// <summary>
        /// 坐标系中的所有多边形，之所以单独列出就是因为多边形依赖于一个点的序列
        /// </summary>
        public List<Polygon> PolygonList { get; set; }

        /// <summary>
        /// 初始化函数，关于使用构造函数还是额外的初始化函数有待于探讨
        /// </summary>
        public void init()
        {
            PointList = new List<Point2>();
            PointSetList = new List<IPointSet>();
            PolygonList = new List<Polygon>();
        }
    }
}
