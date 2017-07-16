using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace Geomystery.Views.Geometry
{
    /// <summary>
    /// 显示线型分为实线Solid或者虚线Dotted
    /// </summary>
    public enum ViewType
    {

        //     实线
        Solid = 0,

        //     虚线
        Dotted = 1,
    }

    /// <summary>
    /// （显示）几何实体（抽象、父）类
    /// </summary>
    public abstract class OutputGeometry
    {
        /// <summary>
        /// 可见性
        /// </summary>
        public bool isVisible { get; set; }

        /// <summary>
        /// 填充颜色
        /// </summary>
        public Color fillColor { get; set; }

        /// <summary>
        /// 被选中时候的边界线颜色
        /// </summary>
        public Color selectedLineColor { get; set; }

        /// <summary>
        /// 被选中时的填充颜色
        /// </summary>
        public Color selectedFillColor { get; set; }

        /// <summary>
        /// 边界线颜色
        /// </summary>
        public Color lineColor { get; set; }

        /// <summary>
        /// （直、曲）线的粗细
        /// </summary>
        public float thickness { get; set; }

        /// <summary>
        /// 边界线（直、曲线的）线型——（实线虚线）
        /// </summary>
        public ViewType borderType { get; set; }
    }
}
