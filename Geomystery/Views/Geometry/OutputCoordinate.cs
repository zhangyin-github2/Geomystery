using Geomystery.Models.Geometry;
using Microsoft.Graphics.Canvas.UI.Xaml;
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

        ///old只有一个图层，直线和圆会把点盖住
        /// <summary>
        /// 被显示的几何实体列表
        /// </summary>
        //public List<OutputGeometry> geometryList { get; set; }


        /// <summary>
        /// 屏幕上的点的列表
        /// </summary>
        public List<OutputPoint> outputPointList { get; set; }

        /// <summary>
        /// 屏幕上点集的列表
        /// </summary>
        public List<OutputGeometry> outputPointSetList { get; set; }

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
            outputPointList = new List<OutputPoint>();
            outputPointSetList = new List<OutputGeometry>();
            //geometryList = new List<OutputGeometry>();
            unitLength = 10;
        }

        /// <summary>
        /// 逻辑坐标系到屏幕显示坐标系的转换（左上角模式）
        /// </summary>
        /// <param name="p2"></param>
        /// <returns>v2</returns>
        public Vector2 ToVector2Upper_Left_Corner(Point2 p2)
        {
            float x = p2.X - vector.X;
            float y = p2.Y - vector.Y;
            x = x / unitLength;
            y = -y / unitLength;
            Vector2 v2 = new Vector2(x,y);
            return v2;
        }
        /// <summary>
        /// 屏幕显示坐标系到逻辑坐标的转换（左上角模式）
        /// </summary>
        /// <param name="v2"></param>
        /// <returns>Point2 p2</returns>
        public Point2 ToPoint2Upper_Left_Corner(Vector2 v2)
        { 
            Point2 p2 = new Point2() { X = (v2.X*unitLength+vector.X), Y = -(v2.Y*unitLength+vector.Y) };
            return p2;
        }


        /// <summary>
        /// 逻辑坐标系到屏幕显示坐标系的转换（中心模式）
        /// </summary>
        /// <param name="p2"></param>
        /// <returns>屏幕上的点</returns>
        public Vector2 ToVector2(Point2 p2)
        {
            Vector2 vop = new Vector2() { X = p2.X - vector.X, Y = vector.Y - p2.Y };
            //if (WindowHeight <= 0 || WindowWidth <= 0) throw new Exception("中心构造方式需要了解canvas画布的actual宽和高");
            Vector2 half = new Vector2() { X = WindowWidth / 2, Y = WindowHeight / 2 };
            Vector2 result = half + vop * unitLength;
            return result;
        }
        /// <summary>
        /// 屏幕显示坐标系到逻辑坐标的转换（中心模式）
        /// </summary>
        /// <param name="v2"></param>
        /// <returns>逻辑坐标系中的点</returns>
        public Point2 ToPoint2(Vector2 v2)
        {
            if (WindowHeight <= 0 || WindowWidth <= 0) throw new Exception("中心构造方式需要了解canvas画布的actual宽和高");
            Vector2 half = new Vector2() { X = WindowWidth / 2, Y = WindowHeight / 2 };
            Vector2 vop = (v2 - half) / unitLength;
            Point2 p2 = new Point2() { X = (vop.X + vector.X), Y = -vop.Y + vector.Y };
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
            if (lv.Length() <= 0) return float.NaN;
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
                if(distance == 0)                           //点在直线上
                {
                    result.X = outerPoint.X;
                    result.Y = outerPoint.Y;
                }
                else                                        //点在直线外
                {
                    float kk = Vector2.Dot(outerPoint - lpo, lv);                   //v1 .* v2 = |v1| cos θ
                    result = lpo + kk * lv / lv.LengthSquared();
                }
                return distance;
            }
        }

        public static float DistanceOfPointAndCircle(Vector2 center, float radius, Vector2 outerPoint, ref Vector2 result)
        {
            if (radius <= 0) return float.NaN;               //圆的半径不可能为负值
            Vector2 centerOuter = outerPoint - center;
            float lengthToCenter = centerOuter.Length();
            result = center + radius / lengthToCenter * centerOuter;
            return Math.Abs(lengthToCenter - radius);
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
                isVisible = true,
                fillColor = Color.FromArgb(255, 200, 200, 200),
                lineColor = Color.FromArgb(255, 130, 91, 230),
                point = point,
                selectedFillColor = Color.FromArgb(255, 200, 200, 200),
                selectedLineColor = Color.FromArgb(255, 255, 99, 71),
                thickness = 4,
                viewPoint = ToVector2(point),
            };
            point.resultPoint = outputPoint;
            outputPointList.Add(outputPoint);
            return 1;
        }

        /// <summary>
        /// 移除某个点的投影
        /// </summary>
        /// <param name="point">待移除的点（逻辑坐标系中）</param>
        public int RemovePoint(Point2 point)
        {
            foreach(OutputPoint outputPoint in outputPointList)
            {
                if (outputPoint.point == point)
                {
                    outputPointList.Remove(outputPoint);
                    return 1;
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
                if(line.type == LineType.Line)                      //直线
                {
                    Vector2 v1 = ToVector2(line.p1);
                    Vector2 v2 = ToVector2(line.p2);
                    Vector2 v3 = new Vector2();
                    Vector2 v4 = new Vector2(); 

                    if(v1.X == v2.X)                //竖线
                    {
                        v3.X = v1.X;
                        v3.Y = 0;
                        v4.X = v1.X;
                        v4.Y = WindowHeight;
                    }
                    else if( Math.Abs(v2.Y - v1.Y) > Math.Abs(v2.X-v1.X) )
                    {
                        v3.X = v1.X - v1.Y * (v1.X - v2.X) / (v1.Y - v2.Y);
                        v3.Y = 0;
                        v4.X = (WindowHeight - v1.Y) / (v2.Y - v1.Y) * v2.X + (v2.Y - WindowHeight) / (v2.Y - v1.Y) * v1.X;
                        v4.Y = WindowHeight;
                    }
                    else if (Math.Abs(v2.Y - v1.Y) <= Math.Abs(v2.X - v1.X))
                    {
                        v3.X = WindowWidth;
                        v3.Y = (WindowWidth - v1.X) / (v2.X - v1.X) * v2.Y + (v2.X - WindowWidth) / (v2.X - v1.X) * v1.Y;
                        v4.X = 0;
                        v4.Y = v1.Y - v1.X * (v1.Y - v2.Y) / (v1.X - v2.X);
                    }
                    else
                    {
                        throw new Exception();
                    }
                    float length12 = (v1 - v2).Length();

                    OutputLine outputLine = new OutputLine()
                    {
                        borderType = ViewType.Solid,
                        isVisible = true,
                        fillColor = Color.FromArgb(255, 76, 123, 207),
                        lineColor = Color.FromArgb(255, 76, 123, 207),
                        line = line,
                        selectedFillColor = Color.FromArgb(255, 130, 91, 230),
                        selectedLineColor = Color.FromArgb(255, 255, 99, 91),
                        thickness = 8,
                        p1 = v1,
                        p2 = v2,
                    };

                    OutputLine outputLineStraight = new OutputLine()
                    {
                        borderType = ViewType.Solid,
                        isVisible = true,
                        fillColor = Color.FromArgb(128, 86, 106, 143),
                        lineColor = Color.FromArgb(128, 86, 106, 143),
                        line = line,
                        selectedFillColor = Color.FromArgb(128, 86, 106, 143),
                        selectedLineColor = Color.FromArgb(128, 86, 106, 143),
                        thickness = 8,
                        p1 = v3,
                        p2 = v4,
                    };

                    line.resultLine.Add(outputLineStraight);                //先画延长线
                    outputPointSetList.Add(outputLineStraight);

                    line.resultLine.Add(outputLine);                        //覆盖线段
                    outputPointSetList.Add(outputLine);
                    
                    return 1;
                }
            }
            
            return 0;
        }


        /// <summary>
        /// 从视图坐标系（屏幕上）移除直线的投影
        /// </summary>
        /// <param name="line">被移除投影的直线</param>
        /// <returns></returns>
        public int RemoveLine(Line line)
        {
            int n = 0;
            /*
            while(n < outputPointSetList.Count)
            {
                if (outputPointSetList[n] is OutputLine)
                {
                    OutputLine outputLine = outputPointSetList[n] as OutputLine;
                    if (outputLine.line == line)
                    {
                        outputPointSetList.Remove(outputPointSetList[n]);
                        continue;
                    }
                }
                n++;
            }
            */
            for(int i = 0; i < line.resultLine.Count; i++)
            {
                outputPointSetList.Remove(line.resultLine[i]);
            }
            line.resultLine.Clear();
            return 0;
        }

        /// <summary>
        /// 添加逻辑坐标系中圆在视图坐标系的投影
        /// </summary>
        /// <param name="circle"></param>
        /// <returns>操作结果</returns>
        public int AddCircle(Circle circle)
        {
            OutputCircle outputCircle = new OutputCircle()
            {
                borderType = ViewType.Solid,
                isVisible = true,
                fillColor = Color.FromArgb(255, 76, 123, 207),
                lineColor = Color.FromArgb(255, 76, 123, 207),
                circle = circle,
                selectedFillColor = Color.FromArgb(255, 128, 128, 128),
                selectedLineColor = Color.FromArgb(255, 255, 99, 71),
                thickness = 8,
                center = ToVector2(circle.center),
                radius = (ToVector2(circle.center) - ToVector2(circle.radius)).Length(),
            };
            circle.resultCircle = outputCircle;
            outputPointSetList.Add(outputCircle);
            return 0;
        }

        /// <summary>
        /// 从视图坐标系（屏幕上）移除圆的投影
        /// </summary>
        /// <param name="circle">逻辑坐标系中的圆</param>
        /// <returns>操作结果</returns>
        public int RemoveCircle(Circle circle)
        {
            foreach (OutputGeometry outputGeometry in outputPointSetList)
            {
                if (outputGeometry is OutputCircle)
                {
                    OutputCircle outputCircle = outputGeometry as OutputCircle;
                    if (outputCircle.circle == circle)
                    {
                        outputPointSetList.Remove(outputGeometry);
                        return 1;
                    }
                }
            }
            return 0;
        }

        /// <summary>
        /// 获取鼠标在屏幕当前点附近的逻辑坐标点
        /// </summary>
        /// <param name="point">屏幕上的点</param>
        /// <returns>逻辑点列表</returns>
        public Surroundings GetSurroundings(Vector2 point)
        {
            Surroundings result = new Surroundings() { screenPoint = point };
            float currentLength = 0;
            if (outputPointList == null && outputPointSetList == null) return null;
            for (int i = 0; i < outputPointList.Count; i++)
            {
                if (outputPointList[i].isVisible == true)
                {
                    var pCurrent = outputPointList[i];
                    if ((currentLength = (pCurrent.viewPoint - point).Length()) < OutputPoint.scopeLength)            //点击的点在屏幕上某个点的圆圈内
                    {
                        result.surroundingPoint.Add(new GeometryAndTheDistance() { geometry = pCurrent.point, distance = currentLength });
                    }
                }
            }
            for (int i = 0; i < outputPointSetList.Count; i++)
            {
                if (outputPointSetList[i].isVisible == true)
                {
                    if (outputPointSetList[i] is OutputLine)
                    {
                        var lCurrent = outputPointSetList[i] as OutputLine;
                        var perpendicularFoot = new Vector2();
                        if ((currentLength = OutputCoordinate.DistanceOfPointAndLine(lCurrent.p1, lCurrent.p2 - lCurrent.p1, point, ref perpendicularFoot)) < OutputPoint.scopeLength)
                        {
                            int j;
                            for (j = 0; j < result.surroundingLine.Count; j++)
                            {
                                if (result.surroundingLine[j].geometry == lCurrent.line) break;
                            }
                            if (j == result.surroundingLine.Count)                           //防止重复添加
                            {
                                result.surroundingLine.Add(new GeometryAndTheDistance() { geometry = lCurrent.line, distance = currentLength });
                            }
                        }
                    }
                    else if (outputPointSetList[i] is OutputCircle)
                    {
                        var cCurrent = outputPointSetList[i] as OutputCircle;
                        if ((currentLength = Math.Abs(cCurrent.radius - (point - cCurrent.center).Length())) < OutputPoint.scopeLength)
                        {
                            result.surroundingCircle.Add(new GeometryAndTheDistance() { geometry = cCurrent.circle, distance = currentLength });
                        }
                    }
                }
            }
            result.surroundingPoint.Sort();                         //排序
            result.surroundingLine.Sort();
            result.surroundingCircle.Sort();
            return result;
        }

        /// <summary>
        /// 当前屏幕上鼠标点击点附近可用的交点或者依附点
        /// </summary>
        /// <param name="surroundings"></param>
        /// <returns></returns>
        public Surroundings GetCross(Surroundings surroundings)
        {
            Surroundings result = new Surroundings() { screenPoint = surroundings.screenPoint };
            if (surroundings.surroundingLine.Count == 0 && surroundings.surroundingCircle.Count == 0) return result;
            float currentLength;
            if (surroundings.surroundingLine.Count > 0 || surroundings.surroundingCircle.Count > 0 )
            {
                for(int i = 0; i < surroundings.surroundingLine.Count; i++)                     //遍历直线集合
                {
                    Line lCurrent = surroundings.surroundingLine[i].geometry as Line;
                    for(int j = i; j < surroundings.surroundingLine.Count; j++)                 //前一个元素已经和自己相交过，所以后面的元素不需要
                    {
                        Vector2 perpendicularFoot = new Vector2();   //垂足
                        if (i == j)                                  //自己和自己，单依赖
                        {
                            currentLength = DistanceOfPointAndLine(this.ToVector2(lCurrent.p1), this.ToVector2(lCurrent.p2) - this.ToVector2(lCurrent.p1), surroundings.screenPoint, ref perpendicularFoot);
                            Point2 pf = ToPoint2(perpendicularFoot);
                            pf.rely.Add(lCurrent);                      //依赖于一条直线
                            result.surroundingPoint.Add(new GeometryAndTheDistance() { geometry = pf, distance = currentLength });
                        }
                        else                                        //相交依赖
                        {
                            List<Point2> plist = ((IPointSet)lCurrent).Intersection(surroundings.surroundingLine[j].geometry as Line);
                            for(int k = 0; k < plist.Count; k++)
                            {
                                //plist[k].rely.Add(lCurrent);                                                  //添加依赖于两条直线
                                //plist[k].rely.Add(surroundings.surroundingLine[j].geometry as Line);
                                result.surroundingPoint.Add(new GeometryAndTheDistance() { geometry = plist[k], distance = Vector2.Distance(this.ToVector2(plist[k]), surroundings.screenPoint)});
                            }
                        }
                        
                    }
                    for(int j = 0; j < surroundings.surroundingCircle.Count; j++)               //直线与每个圆相交
                    {
                        List<Point2> plist = ((IPointSet)lCurrent).Intersection(surroundings.surroundingCircle[j].geometry as Circle);
                        for (int k = 0; k < plist.Count; k++)
                        {
                            //plist[k].rely.Add(lCurrent);                                                  //添加依赖于1条直线和1个圆
                            //plist[k].rely.Add(surroundings.surroundingCircle[j].geometry as Circle);
                            result.surroundingPoint.Add(new GeometryAndTheDistance() { geometry = plist[k], distance = Vector2.Distance(this.ToVector2(plist[k]), surroundings.screenPoint) });
                        }
                    }
                }
                for(int i = 0; i < surroundings.surroundingCircle.Count; i++)
                {
                    Circle cCurrent = surroundings.surroundingCircle[i].geometry as Circle;
                    for (int j = 0; j < surroundings.surroundingCircle.Count; j++)               //圆集合的相交
                    {
                        Vector2 nearestFoot = new Vector2();
                        if (i == j)                                  //自己和自己，单依赖
                        {
                            currentLength = OutputCoordinate.DistanceOfPointAndCircle(this.ToVector2(cCurrent.center), (this.ToVector2(cCurrent.radius) - this.ToVector2(cCurrent.center)).Length(), surroundings.screenPoint, ref nearestFoot);
                            Point2 nf = ToPoint2(nearestFoot);
                            nf.rely.Add(cCurrent);                                                      //依赖于圆
                            result.surroundingPoint.Add(new GeometryAndTheDistance() { geometry = nf, distance = currentLength });
                        }
                        else                                        //相交依赖
                        {
                            List<Point2> plist = ((IPointSet)cCurrent).Intersection(surroundings.surroundingCircle[j].geometry as Circle);
                            for (int k = 0; k < plist.Count; k++)
                            {
                                //plist[k].rely.Add(cCurrent);                                                  //添加依赖于两条直线
                                //plist[k].rely.Add(surroundings.surroundingCircle[j].geometry as Circle);
                                result.surroundingPoint.Add(new GeometryAndTheDistance() { geometry = plist[k], distance = Vector2.Distance(this.ToVector2(plist[k]), surroundings.screenPoint) });
                            }
                        }
                    }
                }
            }
            result.surroundingPoint.Sort();                     //排序
            return result;
        }

        public void refreshCanvas(CanvasAnimatedControl canvas1)
        {
            WindowHeight = (float)canvas1.ActualHeight;
            WindowWidth = (float)canvas1.ActualWidth;
        }

        /// <summary>
        /// 刷新显示坐标系中所有的元素坐标
        /// </summary>
        public void refreshGeometrys()
        {
            foreach (OutputPoint outputPoint in outputPointList)
            {
                outputPoint.viewPoint = ToVector2(outputPoint.point);
            }
            int i = 0;
            OutputGeometry outputGeometry;
            while (i < outputPointSetList.Count)
            {
                outputGeometry = outputPointSetList[i];
                if (outputGeometry is OutputPoint)
                {
                    throw new Exception("不可能触发的异常");
                }
                else if(outputGeometry is OutputLine)
                {
                    OutputLine outLine = outputGeometry as OutputLine;
                    Line line = outLine.line;
                    if (line.lineRely == LineRely.Normal)                    //两点生成
                    {
                        if (line.type == LineType.Line)                      //线段
                        {
                            if (!(outputPointSetList[i + 1] is OutputLine)) throw new Exception();
                            if ( (outputPointSetList[i + 1] as OutputLine).line != line) throw new Exception();

                            Vector2 v1 = ToVector2(line.p1);
                            Vector2 v2 = ToVector2(line.p2);
                            Vector2 v3 = new Vector2();
                            Vector2 v4 = new Vector2();

                            if (v1.X == v2.X)                //竖线
                            {
                                v3.X = v1.X;
                                v3.Y = 0;
                                v4.X = v1.X;
                                v4.Y = WindowHeight;
                            }
                            else if (Math.Abs(v2.Y - v1.Y) > Math.Abs(v2.X - v1.X))
                            {
                                v3.X = v1.X - v1.Y * (v1.X - v2.X) / (v1.Y - v2.Y);
                                v3.Y = 0;
                                v4.X = (WindowHeight - v1.Y) / (v2.Y - v1.Y) * v2.X + (v2.Y - WindowHeight) / (v2.Y - v1.Y) * v1.X;
                                v4.Y = WindowHeight;
                            }
                            else if (Math.Abs(v2.Y - v1.Y) <= Math.Abs(v2.X - v1.X))
                            {
                                v3.X = WindowWidth;
                                v3.Y = (WindowWidth - v1.X) / (v2.X - v1.X) * v2.Y + (v2.X - WindowWidth) / (v2.X - v1.X) * v1.Y;
                                v4.X = 0;
                                v4.Y = v1.Y - v1.X * (v1.Y - v2.Y) / (v1.X - v2.X);
                            }
                            else
                            {
                                throw new Exception();
                            }

                            OutputLine outputLineNew = new OutputLine()
                            {
                                borderType = ViewType.Solid,
                                isVisible = true,
                                fillColor = Color.FromArgb(255, 76, 123, 207),
                                lineColor = Color.FromArgb(255, 76, 123, 207),
                                line = line,
                                selectedFillColor = Color.FromArgb(255, 130, 91, 230),
                                selectedLineColor = Color.FromArgb(255, 255, 99, 91),
                                thickness = 8,
                                p1 = v1,
                                p2 = v2,
                            };

                            OutputLine outputLineStraightNew = new OutputLine()
                            {
                                borderType = ViewType.Solid,
                                isVisible = true,
                                fillColor = Color.FromArgb(128, 86, 106, 143),
                                lineColor = Color.FromArgb(128, 86, 106, 143),
                                line = line,
                                selectedFillColor = Color.FromArgb(128, 86, 106, 143),
                                selectedLineColor = Color.FromArgb(128, 86, 106, 143),
                                thickness = 8,
                                p1 = v3,
                                p2 = v4,
                            };
                            line.resultLine.Clear();
                            line.resultLine.Add(outputLineStraightNew);                //先画延长线
                            outputPointSetList[i] = outputLineStraightNew;

                            line.resultLine.Add(outputLineNew);                        //覆盖线段
                            outputPointSetList[i + 1] = outputLineNew;
                            i = i + 2;
                        }
                    }
                }
                else if(outputGeometry is OutputCircle)
                {
                    OutputCircle outputCircle = outputGeometry as OutputCircle;
                    outputCircle.center = ToVector2(outputCircle.circle.center);
                    outputCircle.radius = (ToVector2(outputCircle.circle.center) - ToVector2(outputCircle.circle.radius)).Length();
                    i++;
                }
            }
        }
    }
}
