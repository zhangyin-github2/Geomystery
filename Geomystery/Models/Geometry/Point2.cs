using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Geomystery.Models.Geometry
{
    public class Point2 : IGeometry
    {
        public float X { get; set; }
        public float Y { get; set; }

        //所在坐标系
        public Coordinate coord { get; set; }

        //依赖r

        public List<IGeometry> rely { get; set; }

        //影响
        public List<IGeometry> influence { get; set; }

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
