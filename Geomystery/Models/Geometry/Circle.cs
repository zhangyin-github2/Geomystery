using Geomystery.Views.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geomystery.Models.Geometry
{
    /// <summary>
    /// 逻辑坐标系中，我们自己定义的“圆”
    /// </summary>
    public class Circle : Geometry, IPointSet
    {
        /// <summary>
        /// 圆心所在点
        /// </summary>
        public Point2 center { get; set; }

        /// <summary>
        /// 用圆上的点表示的半径
        /// </summary>
        public Point2 radius { get; set; }

        /// <summary>
        /// 圆上的其他点，这些点依赖于这个圆
        /// </summary>
        public List<Point2> onCircle;

        /// <summary>
        /// 记录了这个圆在屏幕上的投影圆，是一个绑定，理论上来说，如果一个模型M有多个实现V，这个变量就会变成List<OutputCircle>
        /// </summary>
        public OutputCircle resultCircle { get; set; }

        /// <summary>
        /// 圆可能会与直线相交，也可能会与另一个圆相交，交点有0,1,2个
        /// </summary>
        /// <param name="another">another可能是一个Circle或者Line</param>
        /// <returns>交点是一个Point2的数组</returns>
        List<Point2> IPointSet.Intersection(IPointSet another)
        {
            List<Point2> pcl = new List<Point2>();
            if (another is Line)
            {

            }
            else if (another is Circle)
            {

            }
            return pcl;
        }
    }
}
