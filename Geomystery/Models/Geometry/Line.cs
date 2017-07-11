using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geomystery.Models.Geometry
{
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

    public class Line : Geometry, IPointSet
    {
        //第一个点
        public Point2 p1 { get; set; }

        //第二个点
        public Point2 p2 { get; set; }

        //线型（直线，射线，线段）
        public LineType type { get; set; }

        //线上的点
        public List<Point2> online { get; set; }

        //直线与什么东西相交
        List<Point2> IPointSet.intersection(IPointSet another)
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
