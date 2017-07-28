using Geomystery.Models.Geometry;
using Geomystery.Views.Geometry;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Input;

namespace Geomystery.Controllers.Geometry
{
    /// <summary>
    /// 模型控制器，提供点击pointer_pressed与释放pointer_released接口
    /// </summary>
    public class Controllers
    {
        /// <summary>
        /// 正在执行的记录器
        /// </summary>
        public DFA runningDFA { get; set; }

        /// <summary>
        /// 历史记录
        /// </summary>
        public List<DFA> historyDfaList { get; set; }

        /// <summary>
        /// 重做历史记录，记录用户撤销了多少
        /// </summary>
        public List<DFA> redoDfaList { get; set; }

        /// <summary>
        /// 是否经过初始化
        /// </summary>
        public bool isIniialized { get; set; }

        /// <summary>
        /// 受控制的逻辑坐标系
        /// </summary>
        public Coordinate coordinate { get; set; }

        /// <summary>
        /// 用来显示的坐标系列表，将会分别在在canvas中显示
        /// </summary>
        public List<OutputCoordinate> outputCoordinates { get; set; }

        /// <summary>
        /// 指针按下点
        /// </summary>
        public Point pressedPoint { get; set; }

        /// <summary>
        /// 指针弹起点
        /// </summary>
        public Point releasedPoint { get; set; }

        /// <summary>
        /// 空构造函数
        /// </summary>
        public Controllers()
        {
            isIniialized = false;
            OutputPoint.scopeLength = 6;               //10 dip
        }

        /// <summary>
        /// 输出坐标系个数
        /// </summary>
        /// <param name="outputNumber">输出坐标系个数</param>
        public Controllers(int outputNumber)
        {
            isIniialized = false;
            OutputPoint.scopeLength = 6;               //10 dip
            Initialized(outputNumber);
        }

        /// <summary>
        /// 初始化坐标系
        /// </summary>
        /// <param name="outputNumber">有多少个显示器（canvas）</param>
        /// <returns>1经过初始化 0初始化失败</returns>
        public int Initialized(int outputNumber)
        {
            if (isIniialized) return 1;
            if(outputNumber>0)
            {
                coordinate = new Coordinate();
                outputCoordinates = new List<OutputCoordinate>();
                for(int i = 0; i < outputNumber; i++)
                {
                    outputCoordinates.Add(new OutputCoordinate(coordinate));
                }
                coordinate.outputCoordinates = outputCoordinates;

                historyDfaList = new List<DFA>();
                redoDfaList = new List<DFA>();
                isIniialized = true;
                return 1;
            }
            return 0;
        }

        /// <summary>
        /// 关卡的预初始化
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        public int PreInitialized(Controllers controller)
        {
            this.coordinate = controller.coordinate;
            if (outputCoordinates != null)
            {
                outputCoordinates.Clear();
            }
            outputCoordinates = new List<OutputCoordinate>();
            outputCoordinates.Add(controller.outputCoordinates[0]);
            return 1;
        }

        /// <summary>
        /// 指针被按下
        /// </summary>
        /// <param name="userTool">当前选择的工具</param>
        /// <param name="sender">CanvasAnimatedControl的一个实例canvas1</param>
        /// <param name="e">点击时的信息，包括坐标、滑轮和按键等</param>
        public void PointerPressed(UserTool userTool, object sender, PointerRoutedEventArgs e)
        {
            pressedPoint = e.GetCurrentPoint((CanvasAnimatedControl)sender).Position;
            Vector2 vector2 = new Vector2();
            vector2.X = (float)pressedPoint.X;
            vector2.Y = (float)pressedPoint.Y;

            //var surroundinsGeometryList = GetSurroundings(vector2);
            Surroundings surroundings = outputCoordinates[0].GetSurroundings(vector2);               //周围的点直线圆
            Surroundings scross = outputCoordinates[0].GetCross(surroundings);                       //周围所有可能的交点

            if (runningDFA == null)                                             //可以使用新工具
            {
                runningDFA = new DFA(userTool, 0, coordinate);                      //创建当前工具的记录器
            }
            else if (runningDFA.userTool.toolName != userTool.toolName || runningDFA.state == -2)             //用户更换工具
            {
                runningDFA.Undo();                          //撤销上一个工具
                runningDFA = null;
                runningDFA = new DFA(userTool, 0, coordinate);
            }
            else if (runningDFA.state == 2)
            {
                historyDfaList.Add(runningDFA);
                redoDfaList.Clear();
                runningDFA = null;
            }

            if (userTool.toolName == "选择工具")
            {
                if (surroundings.surroundingPoint.Count > 0)
                {
                    coordinate.ToSelectGeometry(surroundings.surroundingPoint[0].geometry);
                    Debug.WriteLine(surroundings.surroundingPoint[0].geometry.ToString());
                }
                else if(surroundings.surroundingLine.Count > 0)
                {
                    coordinate.ToSelectGeometry(surroundings.surroundingLine[0].geometry);
                    Debug.WriteLine(surroundings.surroundingLine[0].geometry.ToString());
                }
                else if(surroundings.surroundingCircle.Count > 0)
                {
                    coordinate.ToSelectGeometry(surroundings.surroundingCircle[0].geometry);
                    Debug.WriteLine(surroundings.surroundingCircle[0].geometry.ToString());
                }
                else
                { 
                    coordinate.ClearSelectedGeometry();
                }
            }
            else if(userTool.toolName == "交点工具")
            {
                if (surroundings.surroundingLine.Count > 0)
                {
                    if(surroundings.surroundingCircle.Count > 0)                    //选定一个
                    {
                        if(surroundings.surroundingLine[0].distance <= surroundings.surroundingCircle[0].distance)
                        {
                            coordinate.ToSelectGeometry(surroundings.surroundingLine[0].geometry);
                            runningDFA.UserSelectGeomerty(surroundings.surroundingLine[0].geometry, false);
                        }
                        else
                        {
                            coordinate.ToSelectGeometry(surroundings.surroundingCircle[0].geometry);
                            runningDFA.UserSelectGeomerty(surroundings.surroundingCircle[0].geometry, false);
                        }
                    }
                    else
                    {
                        coordinate.ToSelectGeometry(surroundings.surroundingLine[0].geometry);
                        runningDFA.UserSelectGeomerty(surroundings.surroundingLine[0].geometry, false);
                    }
                }
                else if(surroundings.surroundingCircle.Count > 0)
                {
                    coordinate.ToSelectGeometry(surroundings.surroundingCircle[0].geometry);
                    runningDFA.UserSelectGeomerty(surroundings.surroundingCircle[0].geometry, false);
                }

                if(runningDFA.state == 2)
                {
                    List<Point2> plist = ((IPointSet)(runningDFA.needList[0].selectStack[0].selectedGeometry)).Intersection((IPointSet)runningDFA.needList[0].selectStack[1].selectedGeometry);
                    for (int i = 0; i < plist.Count; i++)
                    {
                        runningDFA.result.Add(plist[i]);
                        coordinate.AddPoint(plist[i]);
                        coordinate.ToSelectGeometry(plist[i]);
                    }
                    historyDfaList.Add(runningDFA);
                    redoDfaList.Clear();
                    runningDFA = null;
                }
            }
            else if (userTool.toolName == "点工具")
            {
                if(surroundings.surroundingPoint.Count > 0)
                {
                    runningDFA.UserSelectGeomerty(surroundings.surroundingPoint[0].geometry, false);
                    coordinate.ToSelectGeometry(surroundings.surroundingPoint[0].geometry);
                }
                else
                {
                    Point2 newPoint = outputCoordinates[0].ToPoint2(vector2);
                    runningDFA.UserSelectGeomerty(newPoint, true);
                    //runningDFA.result.Add()
                    coordinate.AddPoint(newPoint);
                    coordinate.ToSelectGeometry(newPoint);
                }
                if (runningDFA.state == 2)
                {
                    historyDfaList.Add(runningDFA);
                    redoDfaList.Clear();
                    runningDFA = null;
                }
            }
            else if(userTool.toolName == "直线工具")
            {
                if (surroundings.surroundingPoint.Count > 0)
                {
                    coordinate.ToSelectGeometry(surroundings.surroundingPoint[0].geometry);
                    runningDFA.UserSelectGeomerty(surroundings.surroundingPoint[0].geometry, false);
                }
                else
                {
                    Point2 newPoint = outputCoordinates[0].ToPoint2(vector2);
                    coordinate.AddPoint(newPoint);
                    coordinate.ToSelectGeometry(newPoint);
                    runningDFA.UserSelectGeomerty(newPoint, true);
                }

                if (runningDFA.state == 2)
                {
                    Line line = new Line();

                    line.p1 = runningDFA.needList[0].selectStack[0].selectedGeometry as Point2;
                    line.p2 = runningDFA.needList[0].selectStack[1].selectedGeometry as Point2;
                    line.type = LineType.Straight;
                    line.lineRely = LineRely.Normal;

                    coordinate.AddLine(line);
                    coordinate.ToSelectGeometry(line);
                    runningDFA.result.Add(line);
                    historyDfaList.Add(runningDFA);
                    redoDfaList.Clear();
                    runningDFA = null;
                }
            }
            else if (userTool.toolName == "圆工具")
            {
                if (surroundings.surroundingPoint.Count > 0)
                {
                    coordinate.ToSelectGeometry(surroundings.surroundingPoint[0].geometry);
                    runningDFA.UserSelectGeomerty(surroundings.surroundingPoint[0].geometry, false);
                }
                else
                {
                    Point2 newPoint = outputCoordinates[0].ToPoint2(vector2);
                    coordinate.AddPoint(newPoint);
                    coordinate.ToSelectGeometry(newPoint);
                    runningDFA.UserSelectGeomerty(newPoint, true);
                }

                if (runningDFA.state == 2)
                {
                    Circle circle = new Circle();

                    circle.center = runningDFA.needList[0].selectStack[0].selectedGeometry as Point2;
                    circle.radius = runningDFA.needList[0].selectStack[1].selectedGeometry as Point2;

                    coordinate.AddCircle(circle);
                    coordinate.ToSelectGeometry(circle);
                    runningDFA.result.Add(circle);
                    historyDfaList.Add(runningDFA);
                    redoDfaList.Clear();
                    runningDFA = null;
                }

            }

        }


        /// <summary>
        /// 指针弹起
        /// </summary>
        /// <param name="userTool">当前选择的工具</param>
        /// <param name="sender">CanvasAnimatedControl的一个实例canvas1</param>
        /// <param name="e">点击时的信息，包括坐标、滑轮和按键等</param>
        public void PointerReleased(UserTool userTool, object sender, PointerRoutedEventArgs e)
        {
            releasedPoint = e.GetCurrentPoint((CanvasAnimatedControl)sender).Position;
        }

        /// <summary>
        /// 指针移动
        /// </summary>
        /// <param name="userTool">当前选择的工具</param>
        /// <param name="sender">CanvasAnimatedControl的一个实例canvas1</param>
        /// <param name="e">点击时的信息，包括坐标、滑轮和按键等</param>
        public void PointerMoved(UserTool userTool, object sender, PointerRoutedEventArgs e)
        {
            Point currentPoint = e.GetCurrentPoint((CanvasAnimatedControl)sender).Position;

        }

        /// <summary>
        /// 是否可以撤销
        /// </summary>
        /// <returns>可否撤销</returns>
        public bool CanUndo()
        {
            if (runningDFA == null && historyDfaList.Count == 0) return false;
            return true;
        }
        
        /// <summary>
        /// 是否可以重做
        /// </summary>
        /// <returns>可否重做</returns>
        public bool CanRedo()
        {
            if (redoDfaList.Count == 0) return false;
            return true;
        }

        /// <summary>
        /// 撤销
        /// </summary>
        /// <returns>撤销结果</returns>
        public bool Undo()
        {
            if(CanUndo())
            {
                if(runningDFA != null)
                {
                    runningDFA.Undo();
                    runningDFA = null;
                }
                else if(historyDfaList.Count > 0)
                {
                    DFA dfa = historyDfaList[historyDfaList.Count - 1];
                    redoDfaList.Add(dfa);
                    dfa.Undo();
                    historyDfaList.Remove(dfa);
                }
                else
                {
                    throw new Exception("逻辑错误");
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// 重做
        /// </summary>
        /// <returns>重做结果</returns>
        public bool Redo()
        {
            if(CanRedo())
            {
                if (runningDFA != null)
                {
                    runningDFA.Undo();
                    runningDFA = null;
                }
                if (redoDfaList.Count > 0)
                {
                    DFA dfa = redoDfaList[redoDfaList.Count - 1];
                    historyDfaList.Add(dfa);
                    dfa.Redo();
                    redoDfaList.Remove(dfa);
                }
                else
                {
                    throw new Exception("逻辑错误");
                }
                return true;
            }
            return false;
        }

        public int AddFromString(string args)
        {
            if (args == null || args == "") return 0;                           //无参数
            char[] splitter = new Char[] { ',', };
            string[] infomations = args.Split(splitter);                        //参数列表
            if (infomations.Count() < 3) return 0;
            string nowstr = null;
            int id = -1;                                //元素的id
            if (!int.TryParse(infomations[1], out id)) return 0;                //id不正常
            if (infomations[0] == "0" || infomations[0].ToLower() == "p" || infomations[0].ToLower() == "point")
            {
                nowstr = infomations[2];
                if (nowstr == "0" || nowstr.ToLower() == "n" || nowstr.ToLower() == "normal")
                {
                    float pX, pY;
                    if (!float.TryParse(infomations[3], out pX)) return 0;                //id不正常
                    if (!float.TryParse(infomations[4], out pY)) return 0;                //id不正常
                    Point2 point = new Point2()
                    {
                        X = pX,
                        Y = pY,
                        coord = this.coordinate,
                        id = id,
                    };
                    coordinate.AddPoint(point);
                    point.id = id;
                    return 1;
                }
                else if (nowstr == "1" || nowstr.ToLower() == "r" || nowstr.ToLower() == "rely")
                {
                    float pX, pY;
                    if (!float.TryParse(infomations[3], out pX)) return 0;                //id不正常
                    if (!float.TryParse(infomations[4], out pY)) return 0;                //id不正常
                    int rid;        //依赖点集id
                    if (!int.TryParse(infomations[1], out rid)) return 0;                //id不正常

                    Point2 point = new Point2()
                    {
                        X = pX,
                        Y = pY,
                        coord = this.coordinate,
                        id = id,
                    };
                    //if (p2.rely == null) p2.rely = new List<Models.Geometry.Geometry>();
                    Models.Geometry.Geometry relyGeometry = coordinate.GetGeometryById(rid);
                    if (relyGeometry == null) return 0;                                 //不存在的依赖元素
                    Vector2 vresult = new Vector2();
                    if (relyGeometry is Line)
                    {
                        Line lCurrent = relyGeometry as Line;
                        OutputCoordinate.DistanceOfPointAndLine(lCurrent.p1.ToVector2(), new Vector2() { X = lCurrent.p2.X - lCurrent.p1.X, Y = lCurrent.p2.Y - lCurrent.p1.Y}, point.ToVector2(), ref vresult);
                    }
                    else if(relyGeometry is Circle)
                    {
                        Circle cCurrent = relyGeometry as Circle;
                        OutputCoordinate.DistanceOfPointAndCircle(cCurrent.center.ToVector2(), Point2.Distance(cCurrent.radius, cCurrent.center), point.ToVector2(), ref vresult);
                    }
                    else
                    {
                        return 0;                                                       //点只能附着在点集上
                    }
                    point.rely.Add(relyGeometry);
                    Point2 pointActual = new Point2() { X = vresult.X, Y = vresult.Y, id = id, };
                    coordinate.AddPoint(pointActual);
                    pointActual.id = id;
                    return 1;
                }
                else if (nowstr == "2" || nowstr.ToLower() == "i" || nowstr.ToLower() == "intersect")
                {
                    int iid1, iid2;
                    if (!int.TryParse(infomations[3], out iid1)) return 0;                //id不正常
                    if (!int.TryParse(infomations[4], out iid2)) return 0;                //id不正常
                    bool cwiskmark;        //如果点集其中一个是圆，该点在第一个点集中是否为逆时针第一个
                    if (!bool.TryParse(infomations[1], out cwiskmark)) cwiskmark = true;
                    if (iid1 == iid2) return 0;
                    Models.Geometry.Geometry iSet1 = coordinate.GetGeometryById(iid1);
                    if (iSet1 == null) return 0;
                    Models.Geometry.Geometry iSet2 = coordinate.GetGeometryById(iid2);
                    if (iSet2 == null) return 0;
                    if (iSet1 is IPointSet && iSet2 is IPointSet)                       //两个必须都是点集
                    {
                        List<Point2> plist = (iSet1 as IPointSet).Intersection(iSet2 as IPointSet);
                        if (plist.Count == 1)
                        {
                            coordinate.AddPoint(plist[0]);
                        }
                        for(int i = 0; i < plist.Count; i++)
                        {
                            if(plist[i].markOfTwoIntersectPointOnCircle == cwiskmark)           //选择两个点之中你要的交点
                            {
                                
                                coordinate.AddPoint(plist[i]);
                                plist[i].id = id;
                                return 1;
                                //break;
                            }
                        }
                    }
                    return 0;                       //两个必须都是点集
                }
                else
                {
                    return 0;                   //创建失败
                }
            }
            else if(infomations[0] == "1" || infomations[0].ToUpper() == "L" || infomations[0].ToLower() == "line")         //构造线
            {
                nowstr = infomations[2];
                if (nowstr == "0" || nowstr.ToLower() == "n" || nowstr.ToLower() == "normal")               //两点构造一条直线
                {
                    int pid1, pid2;
                    if (!int.TryParse(infomations[3], out pid1)) return 0;                //id不正常
                    if (!int.TryParse(infomations[4], out pid2)) return 0;                //id不正常
                    if (pid1 == pid2) return 0;
                    Models.Geometry.Geometry p1 = coordinate.GetGeometryById(pid1);
                    if (p1 == null) return 0;
                    Models.Geometry.Geometry p2 = coordinate.GetGeometryById(pid2);
                    if (p2 == null) return 0;
                    if (p1 is Point2 && p2 is Point2)                       //两个必须都是点集
                    {
                        Line line = new Line()
                        {
                            p1 = p1 as Point2,
                            p2 = p2 as Point2,
                            id = id,
                        };
                        coordinate.AddLine(line);
                        line.id = id;
                    }
                }
            }
            else if(infomations[0] == "2" || infomations[0].ToLower() == "c" || infomations[0].ToLower() == "circle")
            {
                nowstr = infomations[2];
                if (nowstr == "0" || nowstr.ToLower() == "n" || nowstr.ToLower() == "normal")               //圆心，圆上一点构造圆
                {
                    int pid1, pid2;
                    if (!int.TryParse(infomations[3], out pid1)) return 0;                //id不正常
                    if (!int.TryParse(infomations[4], out pid2)) return 0;                //id不正常
                    if (pid1 == pid2) return 0;
                    Models.Geometry.Geometry p1 = coordinate.GetGeometryById(pid1);
                    if (p1 == null) return 0;
                    Models.Geometry.Geometry p2 = coordinate.GetGeometryById(pid2);
                    if (p2 == null) return 0;
                    if (p1 is Point2 && p2 is Point2)                       //两个必须都是点集
                    {
                        Circle circle = new Circle()
                        {
                            center = p1 as Point2,
                            radius = p2 as Point2,
                            id = id,
                        };
                        coordinate.AddCircle(circle);
                        circle.id = id;
                    }
                }
            }
            else
            {
                return 0;                   //创建失败
            }

            return 0;
        }
    }
}
