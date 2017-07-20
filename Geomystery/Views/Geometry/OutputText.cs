using Microsoft.Graphics.Canvas.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace Geomystery.Views.Geometry
{
    /// <summary>
    /// 屏幕上的提示文本，或者是屏幕上元素的“名字标签”
    /// </summary>
    public class OutputText : ICanOutput
    {
        /// <summary>
        /// 文本内容
        /// </summary>
        public string text { get; set; }

        /// <summary>
        /// 在屏幕上的窗体（canva）中，这个文本“写”在哪里
        /// </summary>
        public Vector2 viewPoint { get; set; }

        /// <summary>
        /// 文字颜色
        /// </summary>
        public Color fontColor { get; set; }

        /// <summary>
        /// 文字格式
        /// </summary>
        public CanvasTextFormat format { get; set; }

        /// <summary>
        /// 这个文字是否是某个几何体的标签（名字）
        /// </summary>
        public Models.Geometry.Geometry rely { get; set; }
    }
}
