using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geomystery.Models.Geometry
{
    public class Circle : Geometry, IPointSet
    {
        //圆心
        public Point2 center;

        //半径
        public Point2 radius;

        public List<Point2> onCircle;

        List<Point2> IPointSet.intersection(IPointSet another)
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
