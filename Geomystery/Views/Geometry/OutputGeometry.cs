﻿using System;
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
    /// （显示）（抽象）几何实体（父）类
    /// </summary>
    public abstract class OutputGeometry
    {
        /// <summary>
        /// 可见性
        /// </summary>
        public bool Visible { get; set; }

        /// <summary>
        /// 颜色
        /// </summary>
        public Color color { get; set; }

        /// <summary>
        /// （直、曲）线的粗细
        /// </summary>
        public float Thickness { get; set; }

        /// <summary>
        /// （直、曲线的）线型——（实线虚线）
        /// </summary>
        public ViewType Type { get; set; }
    }
}
