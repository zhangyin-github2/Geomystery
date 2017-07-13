using Geomystery.Views.Geometry;
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
        public Point2 center { get; set; }

        //圆上点表示的半径
        public Point2 radius { get; set; }

        //圆上的其他点
        public List<Point2> onCircle;

        //绑定输出结果圆
        public OutputCircle resultCircle { get; set; }

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
