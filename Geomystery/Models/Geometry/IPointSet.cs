using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geomystery.Models.Geometry
{
    /// <summary>
    /// 点集接口，被直线Line和圆Circle实现
    /// </summary>
    public interface IPointSet
    {
        /// <summary>
        /// 计算两个点集的交集：
        /// 可能是直线与直线，直线与圆，圆与圆相交
        /// </summary>
        /// <param name="another">another可能是一个Circle或者Line</param>
        /// <returns>交点是一个Point2的数组</returns>
        List<Point2> intersection(IPointSet another);
    }
}
