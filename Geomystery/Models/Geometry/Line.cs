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

    public class Line : IGeometry
    {
        //第一个点
        Point2 p1;

        //第二个点
        Point2 p2;

        //线型（直线，射线，线段）
        LineType type;

        //线上的点
        List<Point2> online;
    }
}
