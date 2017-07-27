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
    /// <summary>
    /// 逻辑坐标系中，我们自己定义的“点”
    /// </summary>
    public class Point2 : Geometry
    {
        /// <summary>
        /// 坐标X轴分量
        /// </summary>
        public float X { get; set; }

        /// <summary>
        /// 坐标Y轴分量
        /// </summary>
        public float Y { get; set; }

        /// <summary>
        /// 记录了这个点在屏幕上的投影点，是一个用来绑定的属性，理论上来说，如果一个模型M有多个实现V，这个变量就会变成List<OutputCircle>
        /// </summary>
        public OutputPoint resultPoint { get; set; }

        /// <summary>
        /// 逆时针的第1个，或者是第二个，这个标记表示了圆与某个点集相交的时候，交点的顺序
        /// </summary>
        public bool markOfTwoIntersectPointOnCircle { get; set; }

        /// <summary>
        /// 简单的转换成Vector2函数
        /// </summary>
        /// <returns>Vector2类的对象</returns>
        public Vector2 ToVector2()
        {
            Vector2 p = new Vector2() { X = this.X, Y = this.Y };
            return p;
        }

        /// <summary>
        /// 简单的转换成Point函数
        /// </summary>
        /// <returns>Point类的对象</returns>
        public Point ToPoint()
        {
            Point p = new Point() { X = this.X, Y = this.Y };
            return p;
        }

        public float distanceOf(Point2 anoyherP)
        {
            return distance(this, anoyherP);
        }

        /// <summary>
        /// 两点之间距离公式
        /// </summary>
        /// <param name="p1">点1</param>
        /// <param name="p2">点2</param>
        /// <returns>两点距离</returns>
        public static float distance(Point2 p1, Point2 p2)
        {
            return (float)Math.Sqrt(Math.Pow(p1.X - p2.X, 2.0) + Math.Pow(p1.Y - p2.Y, 2.0));
        }
    }
}
