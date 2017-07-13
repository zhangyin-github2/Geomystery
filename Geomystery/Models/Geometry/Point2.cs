using Geomystery.Views.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Geomystery.Models.Geometry
{
    public class Point2 : Geometry
    {
        public float X { get; set; }

        public float Y { get; set; }

        
        //依赖
        public List<IPointSet> rely { get; set; }

        //影响
        public List<IPointSet> influence { get; set; }

        //绑定输出结果点
        public OutputPoint resultPoint { get; set; }

        public Vector2 ToVector2()
        {
            Vector2 p = new Vector2() { X = this.X, Y = this.Y };
            return p;
        }

        public Point ToPoint()
        {
            Point p = new Point() { X = this.X, Y = this.Y };
            return p;
        }
    }
}
