using Geomystery.Models.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Geomystery.Views.Geometry
{
    //用来显示的点
    public class OutputPoint : OutputGeometry
    {
        //逻辑坐标系中的点
        public Point2 point { get; set; }

        //窗体（canva）中的点
        public Vector2 ViewPoint { get; set; }
    }
}
