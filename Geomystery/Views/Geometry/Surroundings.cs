using Geomystery.Models.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Geomystery.Views.Geometry
{
    /// <summary>
    /// 保存屏幕（视图坐标系）上，鼠标某一点周围有什么元素的数据结构
    /// </summary>
    public class Surroundings
    {
        /// <summary>
        /// 屏幕上的点
        /// </summary>
        Vector2 screenPoint;

        
        /// <summary>
        /// 周围的点
        /// </summary>
        List<Point2> surroundingPoint;

        /// <summary>
        /// 周围的线
        /// </summary>
        List<Line> surroundingLine;

        /// <summary>
        /// 周围的园
        /// </summary>
        List<Circle> surroundingCircle;
    }
}
