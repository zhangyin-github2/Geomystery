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
    //
    public class controllers
    {
        //逻辑坐标系
        public Coordinate coord { get; set; }

        //用来显示的坐标系
        public OutputCoordinate outCoord { get; set; }

        //按下
        public void PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Point pxy = e.GetCurrentPoint((CanvasAnimatedControl)sender).Position;
        }

        //弹起
        public void canvas1_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            Point pxy = e.GetCurrentPoint((CanvasAnimatedControl)sender).Position;
        }
    }
}
