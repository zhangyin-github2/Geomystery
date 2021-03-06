﻿using Geomystery.Models.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Geomystery.Views.Geometry
{
    /// <summary>
    /// 用来显示的屏幕上的点
    /// </summary>
    public class OutputPoint : OutputGeometry
    {
        /// <summary>
        /// 逻辑坐标系中的点
        /// </summary>
        public Point2 point { get; set; }

        /// <summary>
        /// 屏幕上的窗体（canva）中的点
        /// </summary>
        public Vector2 viewPoint { get; set; }

        /// <summary>
        /// 这个点的范围（在屏幕上一个圆表示一个点）
        /// </summary>
        public static float scopeLength { get; set; }
    }
}
