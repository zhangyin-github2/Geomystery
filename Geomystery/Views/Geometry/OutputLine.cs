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
    /// 用来显示的屏幕上的线
    /// </summary>
    public class OutputLine : OutputGeometry
    {
        /// <summary>
        /// 被显示的逻辑坐标系中的线
        /// </summary>
        public Line line { get; set; }

        /// <summary>
        /// 开始点
        /// </summary>
        public Vector2 p1 { get; set; }

        /// <summary>
        /// 结束点
        /// </summary>
        public Vector2 p2 { get; set; }
    }
}
