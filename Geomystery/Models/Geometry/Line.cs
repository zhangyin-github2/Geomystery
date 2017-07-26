using Geomystery.Models.FMatrix;
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
    /// 线型有直线Straight射线Ray线段Line，射线和线段有延长线，所以其实还是直线
    /// </summary>
    public enum LineType
    {
        //
        // 摘要:
        //     直线
        Straight = 0,
        //
        // 摘要:
        //     射线
        Ray = 1,
        //
        // 摘要:
        //     线段
        Line = 2
    }

    /// <summary>
    /// 直线的依赖（直线的生成方式）
    /// </summary>
    public enum LineRely
    {
        //
        // 摘要:
        //     常规方式，p1,p2两点确定一条直线
        Normal = 0,
        //
        // 摘要:
        //     垂线，依赖列表的一条线加上线上或者线外点p1，过p1作依赖点的垂线
        Perpendicular = 1,
        //
        // 摘要:
        //     垂直平分线，p1,p2两点连线的中垂线
        PerpendicularBisector = 2,
        //
        // 摘要:
        //     垂直平分线，p1,p2两点连线的中垂线
        AngleBisector = 3,
    }

    /// <summary>
    /// 逻辑坐标系中，我们自己定义的“直线”，射线与线段有延长线，所以其实还是直线
    /// </summary>
    public class Line : Geometry, IPointSet
    {
        /// <summary>
        /// 第一个点，如果是射线则为射线的起点
        /// </summary>
        public Point2 p1 { get; set; }

        /// <summary>
        /// 第二个点，如果是射线则为射线的方向
        /// </summary>
        public Point2 p2 { get; set; }

        /// <summary>
        /// 确定角平分线专用：可能会在使用p1、p2、p3三点确定角平分线的时候用到
        /// </summary>
        public Point2 p3 { get; set; }

        /// <summary>
        /// 点斜式（p1 , vector）生成直线和射线的时候
        /// </summary>
        public Vector2 lineVector { get; set; }

        /// <summary>
        /// 线型（直线Straight，射线Ray，线段Line）
        /// </summary>
        public LineType type { get; set; }

        /// <summary>
        /// 直线的依赖关系，也是线的生成方式
        /// </summary>
        public LineRely lineRely { get; set; }


        /// <summary>
        /// 线上的点，这些点依赖于这条线，直线平移或者控制点（p1、p2）旋转删除时会受影响
        /// </summary>
        public List<Point2> online { get; set; }

        /// <summary>
        /// 记录了这条线在屏幕上的投影直线是一个绑定，理论上来说，如果一个模型M有多个实现V，这个变量就会变成List<OutputLine>
        /// </summary>
        public List<OutputLine> resultLine { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public Line()
        {
            resultLine = new List<OutputLine>();
        }

        /// <summary>
        /// 获得点斜式定义的直线方向向量
        /// </summary>
        /// <returns>直线方向向量</returns>
        public Vector2 GetVector()
        {
            Vector2 vector = new Vector2();
            if(lineRely == LineRely.Normal)                                 //过p1 p2两点生成
            {
                vector.X = p2.X - p1.X;
                vector.Y = p2.Y - p1.Y;
                //lineVector = vector;
            }
            else if(lineRely == LineRely.AngleBisector)                     //p2为角的顶点，p1 p3为角的边上的点，角平分线生成
            {
                Vector2 v1 = new Vector2() { X = p1.X - p2.X, Y = p1.Y - p2.Y };
                Vector2 v2 = new Vector2() { X = p3.X - p2.X, Y = p3.Y - p2.Y };
                vector = Vector2.Normalize(v1) + Vector2.Normalize(v2);
            }
            else if(lineRely == LineRely.PerpendicularBisector)             //线段p1 p2的垂直平分线
            {
                vector = new Vector2() { X = p2.Y - p1.Y, Y = p1.X - p2.X };
            }
            else if(lineRely == LineRely.Perpendicular)                     //线段p1 p2的垂线（重构后垂线增加 点 线 构造方式）
            {
                vector = new Vector2() { X = p2.Y - p1.Y, Y = p1.X - p2.X };
            }

            lineVector = vector;
            return vector;
        }

        /// <summary>
        /// 获得点斜式定义的点
        /// </summary>
        /// <returns>点斜式定义的点</returns>
        public Point2 GetCenterPoint()
        {
            Point2 point = null;
            if(lineRely == LineRely.Normal)                                 //过p1 p2两点生成
            {
                point = p1;
            }
            else if (lineRely == LineRely.AngleBisector)                     //p2为角的顶点，p1 p3为角的边上的点，角平分线生成
            {
                point = p2;
            }
            else if (lineRely == LineRely.PerpendicularBisector)             //线段p1 p2的垂直平分线
            {
                point = new Point2() { X = (p1.X + p2.X) / 2, Y = (p1.Y + p2.Y) / 2 };              //中点
            }
            else if (lineRely == LineRely.Perpendicular)                     //线段p1 p2的垂线（重构后垂线增加 点 线 构造方式）
            {
                point = p2;
            }
            return point;
        }

        /// <summary>
        /// 直线可能与另一条直线相交，交点有0,、1个，也可能与一个圆相交，交点有0、1、2个
        /// </summary>
        /// <param name="another">another可能是一个Circle或者Line</param>
        /// <returns>交点是一个Point2的数组(List)</returns>
        List<Point2> IPointSet.Intersection(IPointSet another)
        {
            List<Point2> pcl = new List<Point2>();
            if(another is Line)
            {
                Line l2 = another as Line;                      //转换
                Vector2 v1 = this.GetVector();
                Vector2 v2 = l2.GetVector();
                Point2 p1 = this.GetCenterPoint();
                Point2 p2 = l2.GetCenterPoint();
                FMatrix<double> matrix = new FMatrix<double>(2, 3, 0);
                //if(Vector2.Zero)                              //理论上来说两个直线方向向量都不会为0
                Vector2 multiVector = v1 * v2;                      //向量积、叉积
                if(multiVector.Length() > 1e-7)                     //几乎平行
                {
                    
                    matrix[0][0] = v1.X;
                    matrix[0][1] = -v2.X;
                    matrix[0][2] = p2.X - p1.X;

                    matrix[1][0] = v1.Y;
                    matrix[1][1] = -v2.Y;
                    matrix[1][2] = p2.Y - p1.Y;
                }
                FMatrix<double> simpleMatrix = FMatrix<double>.RowSimplestFormOf(matrix);
                pcl.Add(new Point2() { X = p1.X + (float)matrix[0][2] * v1.X, Y = p1.Y + (float)matrix[1][2] * v1.Y });     //存在一个交点
            }
            else if(another is Circle)
            {
                Circle c2 = another as Circle;
                float radius = c2.GetRadius();
                Vector2 lv = this.GetVector();
                Vector2 result = new Vector2();
                
                float distance = OutputCoordinate.DistanceOfPointAndLine(this.GetCenterPoint().ToVector2(), this.GetVector(), c2.center.ToVector2(),ref result);

                float halfChord = (float)Math.Sqrt(Math.Pow(radius, 2.0) - Math.Pow(distance, 2.0));            //弦长的一半
                if (radius >= distance)                  //直线过圆
                {
                    Vector2 lve = Vector2.Normalize(lv);
                    Vector2 vd1 = lve * halfChord;
                    Vector2 vd2 = Vector2.Negate(lve) * halfChord;
                    pcl.Add(new Point2() { X = result.X + vd1.X, Y = result.Y + vd1.Y });
                    pcl.Add(new Point2() { X = result.X + vd2.X, Y = result.Y + vd2.Y });
                }
            }
            return pcl;
        }
    }
}
