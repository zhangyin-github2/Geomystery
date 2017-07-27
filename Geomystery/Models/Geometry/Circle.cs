using Geomystery.Views.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Geomystery.Models.Geometry
{
    /// <summary>
    /// 逻辑坐标系中，我们自己定义的“圆”
    /// </summary>
    public class Circle : Geometry, IPointSet
    {
        /// <summary>
        /// 圆心所在点
        /// </summary>
        public Point2 center { get; set; }

        /// <summary>
        /// 用圆上的点表示的半径
        /// </summary>
        public Point2 radius { get; set; }

        /// <summary>
        /// 圆上的其他点，这些点依赖于这个圆
        /// </summary>
        public List<Point2> onCircle;

        /// <summary>
        /// 记录了这个圆在屏幕上的投影圆，是一个绑定，理论上来说，如果一个模型M有多个实现V，这个变量就会变成List<OutputCircle>
        /// </summary>
        public OutputCircle resultCircle { get; set; }

        /// <summary>
        /// 计算圆的半径
        /// </summary>
        /// <returns>半径</returns>
        public float GetRadius()
        {
            return (float)Math.Sqrt(Math.Pow(center.X - radius.X, 2) + Math.Pow(center.Y - radius.Y, 2));
        }

        /// <summary>
        /// 圆可能会与直线相交，也可能会与另一个圆相交，交点有0,1,2个
        /// </summary>
        /// <param name="another">another可能是一个Circle或者Line</param>
        /// <returns>交点是一个Point2的数组</returns>
        List<Point2> IPointSet.Intersection(IPointSet another)
        {
            List<Point2> pcl = new List<Point2>();          //point cross list
            if (another is Line)
            {
                pcl = another.Intersection(this);               //直接调用直线与圆交点
            }
            else if (another is Circle)
            {
                Circle c2 = another as Circle;
                float distanceOfTwoCircle = this.center.distanceOf(c2.center);              //圆心距
                float radius1 = this.GetRadius();                           //圆1半径
                float radius2 = c2.GetRadius();                             //圆2半径

                if(distanceOfTwoCircle >= Math.Abs(radius1 - radius2) && distanceOfTwoCircle <= radius1 + radius2)          //存在交点(内切、外切、相交)
                {
                    //TODO:两个圆相交
                    int method = 1;
                    if(method==1)                           //平面几何计算解析几何
                    {
                        //余弦定理 cosA = (b^2 + c^2 - a^2)/(2bc)
                        double cosA = (Math.Pow(radius1, 2.0) + Math.Pow(distanceOfTwoCircle, 2.0) - Math.Pow(radius2, 2.0)) / 2 / radius1 / distanceOfTwoCircle;
                        double sinA = Math.Sqrt(1 - Math.Pow(cosA, 2.0));
                        double lengthOfCenterAndBisector = radius1 * cosA;                  //圆心到垂足长度
                        double halfChord = radius1 * sinA;                                  //弦长的一半
                        Vector2 vectorR1ToR2 = new Vector2() { X = c2.center.X - this.center.X, Y = c2.center.Y - this.center.Y };
                        Vector2 ve = Vector2.Normalize(vectorR1ToR2);                   //圆心法向量
                        Vector2 vb = ve * (float)lengthOfCenterAndBisector;         //偏移
                        Vector2 vep = new Vector2() { X = ve.Y, Y = -ve.X };            //弦直线的法向量
                        Vector2 vepb1 = vep * (float)halfChord;                     //一个点
                        Vector2 vepb2 = -vep * (float)halfChord;                    //另一个点
                        Vector2 bbbb = new Vector2() { X = this.center.X + vb.X, Y = this.center.Y + vb.Y };
                        Point2 p1 = new Point2() { X = bbbb.X + vepb1.X, Y = bbbb.Y + vepb1.Y };
                        Point2 p2 = new Point2() { X = bbbb.X + vepb2.X, Y = bbbb.Y + vepb2.Y };
                        bool counterclockwise = true;              //逆时针
                        if ((p1.X - c2.center.X) * (p2.Y - c2.center.Y) - (p1.Y - c2.center.Y) * (p2.X - c2.center.X) < 0)           //顺时针
                        {
                            counterclockwise = false;
                        }
                        p1.rely.Add(this);
                        p1.rely.Add((Geometry)another);
                        p1.markOfTwoIntersectPointOnCircle = counterclockwise;
                        p2.rely.Add(this);
                        p2.rely.Add((Geometry)another);
                        p2.markOfTwoIntersectPointOnCircle = !counterclockwise;
                        pcl.Add(p1);
                        pcl.Add(p2);
                    }
                    else if(method == 2)                    //解析几何方法
                    {
                        //TODO:联立两圆标准方程，得到公共弦方程，联立公共弦方程与其中一个圆，得到交点
                    }
                    else if(method == 3)                    //另一种解析几何
                    {
                        //TODO:一个圆用标准方程(x - x0)^2 + (y - y0)^2 = r^2
                        //另一个圆用参数方程x = r * cos θ; y = r * sin θ;
                        //把参数方程代入标准方程，整理得
                        //a * cosθ + b * sinθ = c
                        //其中
                        //a = 2 * r1 * (x1 - x2)
                        //b = 2 * r1 * (y1 - y2)
                        //c = r2 ^ 2 - r1 ^ 2 - (x1 - x2) ^ 2 - (y1 - y2) ^ 2
                        //通过一些方法解出合理的θ，代入参数方程，即可得到交点
                    }
                }

            }
            return pcl;
        }
    }
}
