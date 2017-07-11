using Geomystery.Models.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Geomystery.Views.Geometry
{
    //用来显示的圆
    public class OutputCircle : OutputGeometry
    {
        //被显示的圆
        public Circle circle { get; set; }

        //圆心坐标
        public Vector2 center { get; set; }

        //半径
        public float radius { get; set; }
    }
}
