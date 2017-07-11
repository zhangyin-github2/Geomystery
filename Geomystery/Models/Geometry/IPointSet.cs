using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geomystery.Models.Geometry
{
    public interface IPointSet
    {
        /**
         * 计算两个点集的交集
         * 常见的可能是直线与直线相交
         */
        List<Point2> intersection(IPointSet another);
    }
}
