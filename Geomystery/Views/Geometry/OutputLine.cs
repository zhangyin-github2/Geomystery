using Geomystery.Models.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Geomystery.Views.Geometry
{
    //用来显示的线
    public class OutputLine : OutputGeometry
    {
        //被显示的线
        public Line line { get; set; }

        //开始点
        public Vector2 p1 { get; set; }

        //结束点
        public Vector2 p2 { get; set; }
    }
}
