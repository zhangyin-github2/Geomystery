using Geomystery.Models.Geometry;
using Geomystery.Views.Geometry;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Input;

namespace Geomystery.Controllers.Geometry
{
    /// <summary>
    /// 模型控制器，提供点击pointer_pressed与释放pointer_released接口
    /// </summary>
    public class controllers
    {
        /// <summary>
        /// 受控制的逻辑坐标系
        /// </summary>
        public Coordinate coord { get; set; }

        /// <summary>
        /// 用来显示的坐标系，将会在canvas中显示
        /// </summary>
        public OutputCoordinate outCoord { get; set; }

        /// <summary>
        /// 按下
        /// </summary>
        /// <param name="sender">CanvasAnimatedControl的一个实例canvas1</param>
        /// <param name="e">点击时的信息，包括坐标、滑轮和按键等</param>
        public void PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Point pxy = e.GetCurrentPoint((CanvasAnimatedControl)sender).Position;
        }

        /// <summary>
        /// 弹起
        /// </summary>
        /// <param name="sender">CanvasAnimatedControl的一个实例canvas1</param>
        /// <param name="e">点击时的信息，包括坐标、滑轮和按键等</param>
        public void canvas1_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            Point pxy = e.GetCurrentPoint((CanvasAnimatedControl)sender).Position;
        }
    }
}
