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
    /// 用来显示在屏幕上的圆
    /// </summary>
    public class OutputCircle : OutputGeometry
    {
        /// <summary>
        /// 被显示的逻辑坐标系的圆
        /// </summary>
        public Circle circle { get; set; }

        /// <summary>
        /// 圆心坐标
        /// </summary>
        public Vector2 center { get; set; }

        /// <summary>
        /// 半径
        /// </summary>
        public float radius { get; set; }
    }
}
