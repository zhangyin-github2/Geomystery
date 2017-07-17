using Geomystery.Models.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace Geomystery.Views.Geometry
{
    //显示坐标系
    public class OutputCoordinate
    {
        //被显示的坐标系
        public Coordinate coordinate { get; set; }

        /// <summary>
        /// 单位长度unit length，
        /// 逻辑坐标系的1单位长度相当于多少 DIP,
        /// DIP代表“器件独立像素”。这是可以与物理像素相同，大于或小于的虚拟化单元。
        /// 像素和DIP之间的比例由DPI决定：pixels = dips* dpi / 96
        /// </summary>
        public float unitLength { get; set; }

        /// <summary>
        /// 平移屏幕坐标系（在逻辑坐标系中）的向量
        /// </summary>
        /*
         * 从逻辑坐标系到显示器坐标系的变换向量vector，可以想象：
         * 逻辑坐标系第四象限有一个矩形框，这个矩形框就是要显示在显示器上的部分
         * vector(v1, v2)即是逻辑坐标系原点指向显示器矩形框左上角的向量
         * 假设单位长度UL(unit length) = 10 dip，可以想象矩形框可以覆盖第四象限
         * （如果对于向量vector(v1, v2)，有v1 < 0 || v2 > 0，显示器会显示更大的一部分可能包括一二三象限）
         * 逻辑点p(a1, b1)在显示器上的坐标应该是( (a1 - v1) * UL , (v2 - b1) * UL )
         * 同理，用户在屏幕上点击点(p1, p2)映射到逻辑坐标系当中应该是 ( p1 / UL + v1 , v2 - p2 / UL  )
         */
        public Vector2 vector { get; set; }


        /// <summary>
        /// 被显示的几何实体列表
        /// </summary>
        public List<OutputGeometry> GeometryList { get; set; }

        /// <summary>
        /// （视图，显示，展示）坐标系构造函数
        /// </summary>
        /// <param name="coordinate">coordinate是逻辑坐标系</param>
        public OutputCoordinate(Coordinate coordinate)
        {
            this.coordinate = coordinate;
            GeometryList = new List<OutputGeometry>();
        }

        /// <summary>
        /// 逻辑坐标系到屏幕显示坐标系的转换
        /// </summary>
        /// <param name="p2"></param>
        /// <returns>v2</returns>
        public  Vector2 ToVector2(Point2 p2)
        {
            float x = p2.X - vector.X;
            float y = p2.Y - vector.Y;
            x = x / unitLength;
            y = -y / unitLength;
            Vector2 v2 = new Vector2(x,y);
            return v2;
        }
        /// <summary>
        /// 屏幕显示坐标系到逻辑坐标的转换
        /// </summary>
        /// <param name="v2"></param>
        /// <returns>Point2 p2</returns>
        public Point2 ToPoint2(Vector2 v2)
        { 
            Point2 p2 = new Point2() { X = (v2.X*unitLength+vector.X), Y = -(v2.Y*unitLength+vector.Y) };
            return p2;
        }

        /// <summary>
        /// 屏幕上某一点到某条直线的垂足
        /// </summary>
        /// <param name="lpo">屏幕上的一点</param>
        /// <param name="lv">屏幕上直线方向向量</param>
        /// <param name="outerPoint">直线上或者直线外的一点</param>
        /// <returns></returns>
        public static float DistanceOfPointAndLine(Vector2 lpo, Vector2 lv, Vector2 outerPoint, ref Vector2 result)
        {
            if (lv.Length() <= 0) return -1;
            if(lv.X == 0)
            {
                result.Y = outerPoint.Y;
                result.X = lpo.X;

                return Math.Abs(lpo.X - outerPoint.X);
            }
            else if(lv.Y == 0)
            {
                result.X = outerPoint.X;
                result.Y = lpo.Y;

                return Math.Abs(lpo.Y - outerPoint.Y);
            }
            else
            {
                float A = -(lv.X / lv.Y);
                float B = 1;
                float C = lv.X / lv.Y * lpo.Y - lpo.X;
                float distance = Math.Abs(A * outerPoint.Y + B * outerPoint.X + C) / (float)Math.Sqrt(Math.Pow(A, 2) + Math.Pow(B, 2));
                //result.X = ?;
                //result.Y = ?;
                return distance;
            }
            return -1; ;
        }

        public void AddPoint(Point2 point)
        {
            GeometryList.Add(new OutputPoint()
            {
                borderType = ViewType.Solid,
                fillColor = Color.FromArgb(0, 0, 0, 0),
                isVisible = true,
                lineColor = Color.FromArgb(255, 0, 0, 0),
                point = point,
                selectedFillColor = Color.FromArgb(0, 128, 128, 128),
                selectedLineColor = Color.FromArgb(255, 128, 128, 128),
                thickness = 2,
                viewPoint = ToVector2(point),
            });
        }

        public int AddLine(Line line)
        {

            return 0;
        }

        public int AddCircle(Circle circle)
        {

            return 0;
        }
    }
}
