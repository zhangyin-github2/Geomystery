using Geomystery.Views.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geomystery.Models.Geometry
{
    /// <summary>
    /// 线型有直线Straight射线Ray线段Line，射线和线段有延长线，所以其实还是直线
    /// </summary>
    public enum LineType
    {
        //
        // 摘要:
        //     直线
        Straight = 0,
        //
        // 摘要:
        //     射线
        Ray = 1,
        //
        // 摘要:
        //     线段
        Line = 2
    }

    /// <summary>
    /// 逻辑坐标系中，我们自己定义的“直线”，射线与线段有延长线，所以其实还是直线
    /// </summary>
    public class Line : Geometry, IPointSet
    {
        /// <summary>
        /// 第一个点，如果是射线则为射线的起点
        /// </summary>
        public Point2 p1 { get; set; }

        /// <summary>
        /// 第二个点，如果是射线则为射线的方向
        /// </summary>
        public Point2 p2 { get; set; }

        /// <summary>
        /// 线型（直线Straight，射线Ray，线段Line）
        /// </summary>
        public LineType type { get; set; }

        /// <summary>
        /// 线上的点，这些点依赖于这条线，直线平移或者控制点（p1、p2）旋转删除时会受影响
        /// </summary>
        public List<Point2> online { get; set; }

        /// <summary>
        /// 记录了这条线在屏幕上的投影直线是一个绑定，理论上来说，如果一个模型M有多个实现V，这个变量就会变成List<OutputLine>
        /// </summary>
        public OutputLine resultLine { get; set; }

        /// <summary>
        /// 直线可能与另一条直线相交，交点有0,、1个，也可能与一个圆相交，交点有0、1、2个
        /// </summary>
        /// <param name="another">another可能是一个Circle或者Line</param>
        /// <returns>交点是一个Point2的数组</returns>
        List<Point2> IPointSet.Intersection(IPointSet another)
        {
            List<Point2> pcl = new List<Point2>();
            if(another is Line)
            {

            }
            else if(another is Circle)
            {

            }
            return pcl;
        }
    }
}
