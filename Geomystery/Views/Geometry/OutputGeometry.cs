using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace Geomystery.Views.Geometry
{
    //实线或者虚线
    public enum ViewType
    {

        //     实线
        Solid = 0,

        //     虚线
        Dotted = 1,
    }

    //（显示）（抽象）几何实体（父）类
    public abstract class OutputGeometry
    {
        //可见性
        public bool Visible { get; set; }

        //颜色
        public Color color { get; set; }

        //（直、曲）线的粗细
        public float Thickness { get; set; }

        //（直、曲线的）线型——（实线虚线）
        public ViewType Type { get; set; }
    }
}
