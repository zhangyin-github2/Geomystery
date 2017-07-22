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
    /// <summary>
    /// 显示坐标系(窗体坐标系，是逻辑坐标系的投影)
    /// </summary>
    public class OutputCoordinate
    {
        /// <summary>
        /// 被显示的坐标系
        /// </summary>
        public Coordinate coordinate { get; set; }

        /// <summary>
        /// 窗口高度
        /// </summary>
        public float WindowHeight { get; set; }

        /// <summary>
        /// 窗口宽度
        /// </summary>
        public float WindowWidth { get; set; }

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
        public List<OutputGeometry> geometryList { get; set; }

        /// <summary>
        /// 被显示的文本列表，这个文本可能是孤立文本，也可能是一个几何体的名字（需要附着在这个几何体的周围）
        /// </summary>
        public List<OutputText> textList { get; set; }

        /// <summary>
        /// （视图，显示，展示）坐标系构造函数
        /// </summary>
        /// <param name="coordinate">coordinate是逻辑坐标系</param>
        public OutputCoordinate(Coordinate coordinate)
        {
            this.coordinate = coordinate;
            geometryList = new List<OutputGeometry>();
            unitLength = 100;
        }

        /// <summary>
        /// 逻辑坐标系到屏幕显示坐标系的转换
        /// </summary>
        /// <param name="p2"></param>
        /// <returns>v2</returns>
        public Vector2 ToVector2(Point2 p2)
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
        }

        /// <summary>
        /// 增加某个点的投影
        /// </summary>
        /// <param name="point">被投影的点（逻辑坐标系中）</param>
        public int AddPoint(Point2 point)
        {
            OutputPoint outputPoint = new OutputPoint()
            {
                borderType = ViewType.Solid,
                fillColor = Color.FromArgb(0, 0, 0, 0),
                isVisible = true,
                lineColor = Color.FromArgb(255, 0, 0, 0),
                point = point,
                selectedFillColor = Color.FromArgb(255, 128, 128, 128),
                selectedLineColor = Color.FromArgb(255, 128, 128, 128),
                thickness = 2,
                viewPoint = ToVector2(point),
            };
            point.resultPoint = outputPoint;
            geometryList.Add(outputPoint);
            return 1;
        }

        /// <summary>
        /// 移除某个点的投影
        /// </summary>
        /// <param name="point">待移除的点（逻辑坐标系中）</param>
        public int RemovePoint(Point2 point)
        {
            foreach(OutputGeometry outputGeometry in geometryList)
            {
                if(outputGeometry is OutputPoint)
                {
                    OutputPoint outputPoint = outputGeometry as OutputPoint;
                    if(outputPoint.point == point)
                    {
                        geometryList.Remove(outputGeometry);
                        return 1;
                    }
                }
            }
            return 0;
        }

        /// <summary>
        /// 增加某一条直线的投影
        /// </summary>
        /// <param name="line">被投影的直线</param>
        /// <returns></returns>
        public int AddLine(Line line)
        {
            if(line.lineRely == LineRely.Normal)                    //两点生成
            {
                if(line.type == LineType.Straight)                      //直线
                {
                    Vector2 v1 = ToVector2(line.p1);
                    Vector2 v2 = ToVector2(line.p2);
                    Vector2 v3 = new Vector2();
                    Vector2 v4 = new Vector2();

                    float length12 = (v1 - v2).Length();


                    OutputLine outputLine = new OutputLine()
                    {
                        borderType = ViewType.Solid,
                        fillColor = Color.FromArgb(0, 0, 0, 0),
                        isVisible = true,
                        lineColor = Color.FromArgb(255, 0, 0, 0),
                        line = line,
                        selectedFillColor = Color.FromArgb(255, 128, 128, 128),
                        selectedLineColor = Color.FromArgb(255, 128, 128, 128),
                        thickness = 2,
                        p1 = v1,
                        p2 = v2,
                    };

                    line.resultLine.Add(outputLine);
                    geometryList.Add(outputLine);
                    return 1;
                }
            }
            
            
            return 0;
        }

        /// <summary>
        /// 移除某一条直线的投影
        /// </summary>
        /// <param name="line">被移除投影的直线</param>
        /// <returns></returns>
        public int RemoveLine(Line line)
        {

            return 0;
        }

        public int AddCircle(Circle circle)
        {
            OutputCircle outputCircle = new OutputCircle()
            {
                borderType = ViewType.Solid,
                fillColor = Color.FromArgb(0, 0, 0, 0),
                isVisible = true,
                lineColor = Color.FromArgb(255, 0, 0, 0),
                circle = circle,
                selectedFillColor = Color.FromArgb(255, 128, 128, 128),
                selectedLineColor = Color.FromArgb(255, 128, 128, 128),
                thickness = 2,
                center = ToVector2(circle.center),
                radius = (ToVector2(circle.center) - ToVector2(circle.radius)).Length(),
            };
            circle.resultCircle = outputCircle;
            geometryList.Add(outputCircle);
            return 0;
        }

        public int RemoveCircle(Circle circle)
        {

            return 0;
        }
    }
}
