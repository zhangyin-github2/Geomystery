using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geomystery.Models.Geometry
{
    public class Circle : IGeometry
    {
        //圆心
        Point2 center;

        //半径
        Point2 radius;

        List<Point2> onCircle;
    }
}
