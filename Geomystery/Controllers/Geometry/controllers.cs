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
        /// “过关条件序列 ”为空代表自由模式，不为空代表游戏模式，集合 0 号用来存放条件，其他用来存放已经达成的条件序列（们）
        /// </summary>
        public List<ConditionsList> conditionLists { get; set; }

        /// <summary>
        /// 正在达成的条件序列
        /// </summary>
        public List<ConditionsList> meetingconditionLists { get; set; }

        /// <summary>
        /// 另一种过关方式
        /// </summary>
        public AnotherConditionsList anotherConditionsList{get;set;}

        /// <summary>
        /// 过关通知委托
        /// </summary>
        public delegate void MissionSuccess();

        /// <summary>
        /// 游戏通关事件
        /// </summary>
        public event MissionSuccess missionSuccess;             //游戏通关事件

        public bool useAnotherCondition { get; set; }

        bool isPointPressing;                               //指针处于按下状态

        Point pressPoint;                                   //指针按下的点


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
                        TestGeometry(plist[i]);
                        coordinate.ToSelectGeometry(plist[i]);
                    }
                    historyDfaList.Add(runningDFA);
                    redoDfaList.Clear();
                    runningDFA = null;

                    passNotify();
                }
            }
            else if (userTool.toolName == "点工具")
            {
                if(surroundings.surroundingPoint.Count > 0)
                {
                    runningDFA.UserSelectGeomerty(surroundings.surroundingPoint[0].geometry, false);
                    coordinate.ToSelectGeometry(surroundings.surroundingPoint[0].geometry);
                }
                else //周边没有点
                {
                    Point2 point;
                    if (scross.surroundingPoint.Count > 0)
                    {
                        point = scross.surroundingPoint[0].geometry as Point2;
                    }
                    else
                    {
                        point = outputCoordinates[0].ToPoint2(vector2);
                    }
                    runningDFA.UserSelectGeomerty(point, true);
                    
                    coordinate.AddPoint(point);
                    coordinate.ToSelectGeometry(point);
                    TestGeometry(point);
                    passNotify();
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
                    int result = runningDFA.UserSelectGeomerty(surroundings.surroundingPoint[0].geometry, false);
                    if(result == -2)
                    {
                        runningDFA.Undo();
                    }
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
                    line.type = LineType.Line;
                    line.lineRely = LineRely.Normal;
                    
                    coordinate.AddLine(line);
                    coordinate.ToSelectGeometry(line);
                    runningDFA.result.Add(line);
                    historyDfaList.Add(runningDFA);
                    redoDfaList.Clear();
                    runningDFA = null;
                    TestGeometry(line);                         //测试条件
                    passNotify();
                }

                                                   //过关通知
            }
            else if (userTool.toolName == "圆工具")
            {
                if (surroundings.surroundingPoint.Count > 0)
                {
                    coordinate.ToSelectGeometry(surroundings.surroundingPoint[0].geometry);
                    int result = runningDFA.UserSelectGeomerty(surroundings.surroundingPoint[0].geometry, false);
                    if (result == -2)
                    {
                        runningDFA.Undo();
                    }
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
                    TestGeometry(circle);
                    coordinate.AddCircle(circle);
                    coordinate.ToSelectGeometry(circle);
                    runningDFA.result.Add(circle);
                    historyDfaList.Add(runningDFA);
                    redoDfaList.Clear();
                    runningDFA = null;
                    TestGeometry(circle);
                    passNotify();                       //过关通知
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
        /// 指针进入canvas
        /// </summary>
        /// <param name="userTool">用户当前选择的工具</param>
        /// <param name="sender">canvas</param>
        /// <param name="e">事件信息</param>
        public void PointerEntered(UserTool userTool, object sender, PointerRoutedEventArgs e)
        {
            Point currentPoint = e.GetCurrentPoint((CanvasAnimatedControl)sender).Position;
        }

        /// <summary>
        /// 指针从canvas移出
        /// </summary>
        /// <param name="userTool">用户当前选择的工具</param>
        /// <param name="sender">canvas</param>
        /// <param name="e">事件信息</param>
        private void PointerExited(UserTool userTool, object sender, PointerRoutedEventArgs e)
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

        /// <summary>
        /// 通过（一定格式的）字符串新建元素
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public int AddGeometryFromString(string args)
        {
            if (args == null || args == "") return -1;                           //无参数
            char[] splitter = new Char[] { ',', };
            string[] infomations = args.Split(splitter);                        //参数列表
            if (infomations.Count() < 4) return -2;
            string nowstr = null;
            int id = -1;                                //元素的id
            if (!int.TryParse(infomations[1], out id)) return -3;                //id不正常
            bool isVisible;                             //可见性
            if (!bool.TryParse(infomations[3], out isVisible)) return -4;        //可见性不正常
            if (infomations[0] == "0" || infomations[0].ToLower() == "p" || infomations[0].ToLower() == "point")
            {
                nowstr = infomations[2];
                if (nowstr == "0" || nowstr.ToLower() == "n" || nowstr.ToLower() == "normal")
                {
                    float pX, pY;
                    if (!float.TryParse(infomations[4], out pX)) return 0;                //id不正常
                    if (!float.TryParse(infomations[5], out pY)) return 0;                //id不正常
                    Point2 point = new Point2()
                    {
                        X = pX,
                        Y = pY,
                        coord = this.coordinate,
                        id = id,
                    };
                    coordinate.AddPoint(point);
                    point.id = id;
                    point.resultPoint.isVisible = isVisible;
                    return 1;
                }
                else if (nowstr == "1" || nowstr.ToLower() == "r" || nowstr.ToLower() == "rely")
                {
                    float pX, pY;
                    if (!float.TryParse(infomations[4], out pX)) return 0;                //id不正常
                    if (!float.TryParse(infomations[5], out pY)) return 0;                //id不正常
                    int rid;        //依赖点集id
                    if (!int.TryParse(infomations[6], out rid)) return 0;                //id不正常

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
                    pointActual.resultPoint.isVisible = isVisible;
                    pointActual.id = id;
                    return 1;
                }
                else if (nowstr == "2" || nowstr.ToLower() == "i" || nowstr.ToLower() == "intersect")
                {
                    int iid1, iid2;
                    if (!int.TryParse(infomations[4], out iid1)) return 0;                //id不正常
                    if (!int.TryParse(infomations[5], out iid2)) return 0;                //id不正常
                    bool cwiskmark;        //如果点集其中一个是圆，该点在第一个点集中是否为逆时针第一个
                    if (!bool.TryParse(infomations[6], out cwiskmark)) cwiskmark = true;
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
                                plist[i].resultPoint.isVisible = isVisible;
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
                    if (!int.TryParse(infomations[4], out pid1)) return 0;                //id不正常
                    if (!int.TryParse(infomations[5], out pid2)) return 0;                //id不正常
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
                            lineRely = LineRely.Normal,
                            type = LineType.Line,
                        };
                        coordinate.AddLine(line);
                        //line.resultLine.isVisible = isVisible;
                        for(int i = 0; i < line.resultLine.Count; i++)
                        {
                            line.resultLine[i].isVisible = isVisible;
                        }
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
                    if (!int.TryParse(infomations[4], out pid1)) return 0;                //id不正常
                    if (!int.TryParse(infomations[5], out pid2)) return 0;                //id不正常
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
                        circle.resultCircle.isVisible = isVisible;
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


        public int AddConditionFromString(string args, int conditionListIndex)
        {
            if (args == null || args == "") return -1;                           //无参数
            char[] splitter = new Char[] { ',', };
            string[] infomations = args.Split(splitter);                        //参数列表
            if (infomations.Count() < 2) return -2;

            if (conditionLists[conditionListIndex].reachedConditions == null) conditionLists[conditionListIndex].reachedConditions = new List<Condition>();
            if (conditionLists[conditionListIndex].unmetConditions == null) conditionLists[conditionListIndex].unmetConditions = new List<Condition>();

            bool meet;
            if (!bool.TryParse(infomations[1], out meet)) return -4;                //是否满足满足的标志不正常
            if (infomations[0] == "0" || infomations[0].ToLower() == "f" || infomations[0].ToLower() == "free" || infomations[0].ToLower() == "freecondition")
            {
                int pid;
                if (!int.TryParse(infomations[2], out pid)) return -4;                //id不正常
                Point2 point = null;
                if (meet)
                {
                    point = coordinate.GetGeometryById(pid) as Point2;
                    if (point == null) throw new Exception();
                    conditionLists[conditionListIndex].reachedConditions.Add(new FreeCondition() { isMeetTheConditions = meet, pid = pid, point = point });
                }
                else
                {
                    conditionLists[conditionListIndex].unmetConditions.Add(new FreeCondition() { isMeetTheConditions = meet, pid = pid, point = point });
                }
            }
            else if (infomations[0] == "1" || infomations[0].ToLower() == "dl" || infomations[0].ToLower() == "drawline")
            {
                int iid;
                if (!int.TryParse(infomations[2], out iid)) return -4;                //id不正常
                int p1id, p2id;
                if (!int.TryParse(infomations[3], out p1id)) return -4;                //id不正常
                if (!int.TryParse(infomations[4], out p2id)) return -4;                //id不正常
                IPointSet pointSet = null;
                Point2 p1 = null;
                Point2 p2 = null;
                if (meet)
                {
                    pointSet = coordinate.GetGeometryById(iid) as IPointSet;
                    p1 = coordinate.GetGeometryById(p1id) as Point2;
                    p2 = coordinate.GetGeometryById(p2id) as Point2;
                    if (pointSet == null) throw new Exception();
                    if (p1 == null) throw new Exception();
                    if (p2 == null) throw new Exception();
                    conditionLists[conditionListIndex].reachedConditions.Add(new PenDrawCondition() { isMeetTheConditions = meet, iid = iid, pointSet = pointSet, p1id = p1id, p1 = p1, p2id = p2id, p2 = p2, type = 1 });
                }
                else
                {
                    conditionLists[conditionListIndex].unmetConditions.Add(new PenDrawCondition() { isMeetTheConditions = meet, iid = iid, pointSet = pointSet, p1id = p1id, p1 = p1, p2id = p2id, p2 = p2, type = 1 });
                }
            }
            else if (infomations[0] == "2" || infomations[0].ToLower() == "dc" || infomations[0].ToLower() == "drawcircle")
            {
                int iid;
                if (!int.TryParse(infomations[2], out iid)) return -4;                //id不正常
                int p1id, p2id;
                if (!int.TryParse(infomations[3], out p1id)) return -4;                //id不正常
                if (!int.TryParse(infomations[4], out p2id)) return -4;                //id不正常
                IPointSet pointSet = null;
                Point2 p1 = null;
                Point2 p2 = null;
                if (meet)
                {
                    pointSet = coordinate.GetGeometryById(iid) as IPointSet;
                    p1 = coordinate.GetGeometryById(p1id) as Point2;
                    p2 = coordinate.GetGeometryById(p2id) as Point2;
                    if (pointSet == null) throw new Exception();
                    if (p1 == null) throw new Exception();
                    if (p2 == null) throw new Exception();
                    conditionLists[conditionListIndex].reachedConditions.Add(new PenDrawCondition() { isMeetTheConditions = meet, iid = iid, pointSet = pointSet, p1id = p1id, p1 = p1, p2id = p2id, p2 = p2, type = 2 });
                }
                else
                {
                    conditionLists[conditionListIndex].unmetConditions.Add(new PenDrawCondition() { isMeetTheConditions = meet, iid = iid, pointSet = pointSet, p1id = p1id, p1 = p1, p2id = p2id, p2 = p2, type = 2 });
                }
                
            }
            else if (infomations[0] == "3" || infomations[0].ToLower() == "o" || infomations[0].ToLower() == "on" || infomations[0].ToLower() == "onpointset")
            {
                int pid;
                if (!int.TryParse(infomations[2], out pid)) return -4;                //id不正常
                int iid;
                if (!int.TryParse(infomations[3], out iid)) return -4;                //id不正常
                Point2 point = null;
                IPointSet pointSet = null;
                if(meet)
                {
                    point = coordinate.GetGeometryById(pid) as Point2;
                    pointSet = coordinate.GetGeometryById(iid) as IPointSet;
                    if (point == null) throw new Exception();
                    if (pointSet == null) throw new Exception();
                    conditionLists[conditionListIndex].reachedConditions.Add(new OnTheTreeCondition() { isMeetTheConditions = meet, pid = pid, point = point, iid = iid, pointSet = pointSet });
                }
                else
                {
                    conditionLists[conditionListIndex].unmetConditions.Add(new OnTheTreeCondition() { isMeetTheConditions = meet, pid = pid, point = point, iid = iid, pointSet = pointSet });
                }
                
            }
            else if (infomations[0] == "4" || infomations[0].ToLower() == "i" || infomations[0].ToLower() == "intersect" || infomations[0].ToLower() == "intersection")
            {
                int pid;
                if (!int.TryParse(infomations[2], out pid)) return -4;                //id不正常
                int i1id, i2id;
                if (!int.TryParse(infomations[3], out i1id)) return -4;                //id不正常
                if (!int.TryParse(infomations[4], out i2id)) return -4;                //id不正常
                int clock;
                if (!int.TryParse(infomations[5], out clock)) return -4;                //点集其中一个是圆，选择逆时针，顺时针，还是默认
                Point2 point = null;
                IPointSet pointSet1 = null;
                IPointSet pointSet2 = null;
                if(meet)
                {
                    point = coordinate.GetGeometryById(pid) as Point2;
                    pointSet1 = coordinate.GetGeometryById(i1id) as IPointSet;
                    pointSet2 = coordinate.GetGeometryById(i2id) as IPointSet;
                    if (point == null) throw new Exception();
                    if (pointSet1 == null) throw new Exception();
                    if (pointSet2 == null) throw new Exception();
                    conditionLists[conditionListIndex].reachedConditions.Add(new IntersectCondition() { isMeetTheConditions = meet, pid = pid, point = point, i1id = i1id, pointSet1 = pointSet1, i2id = i2id, pointSet2 = pointSet2, clock = clock });
                }
                else
                {
                    conditionLists[conditionListIndex].unmetConditions.Add(new IntersectCondition() { isMeetTheConditions = meet, pid = pid, point = point, i1id = i1id, pointSet1 = pointSet1, i2id = i2id, pointSet2 = pointSet2, clock = clock });
                }
            }
            return 0;
        }

        /// <summary>
        /// 判断用户是否过关
        /// </summary>
        /// <returns>是否过关</returns>
        public bool isWin()
        {
            if(!useAnotherCondition)
            {
                int meetNumber = 0;                     //可以通关条件满足个数
                if (meetingconditionLists != null && meetingconditionLists.Count > 0)              //是游戏模式且存在过关条件
                {
                    for (int i = 0; i < conditionLists.Count; i++)
                    {
                        if (meetingconditionLists[i].reachedConditions.Count == conditionLists[i].reachedConditions.Count + conditionLists[i].unmetConditions.Count)
                        {
                            meetNumber++;
                        }
                    }
                }
                if (meetNumber > 0) return true;                //过关
            }
            else
            {
                float delta = 1e-5f;
                for(int i = 0; i < coordinate.pointList.Count; i++)                     //点
                {
                    for(int j = 0; j < anotherConditionsList.unmetConditions.Count; j++)
                    {
                        if(anotherConditionsList.unmetConditions[j] is PointCondition)
                        {
                            PointCondition pc = anotherConditionsList.unmetConditions[j] as PointCondition;
                            if(floatEqual(pc.wantX, coordinate.pointList[i].X, delta) == 0 && floatEqual(pc.wantY, coordinate.pointList[i].Y, delta) == 0)
                            {
                                pc.isReached = true;
                                anotherConditionsList.reachedConditions.Add(pc);
                                anotherConditionsList.unmetConditions.Remove(pc);
                            }
                        }
                    }
                }
                for (int i = 0; i < coordinate.pointSetList.Count; i++)                 //线 圆
                {
                    for (int j = 0; j < anotherConditionsList.unmetConditions.Count; j++)
                    {
                        if (anotherConditionsList.unmetConditions[j] is LineCondition && coordinate.pointSetList[i] is Line)
                        {
                            LineCondition lc = anotherConditionsList.unmetConditions[j] as LineCondition;
                            Line line = coordinate.pointSetList[i] as Line;
                            float slope = (line.p2.Y - line.p1.Y) / (line.p2.X - line.p1.X);
                            if((floatEqual(lc.wantX, (coordinate.pointSetList[i] as Line).p1.X, delta) == 0 && floatEqual(lc.wantY, (coordinate.pointSetList[i] as Line).p1.Y, delta)==0) || (floatEqual(lc.wantX, (coordinate.pointSetList[i] as Line).p2.X, delta) == 0 && floatEqual(lc.wantY, (coordinate.pointSetList[i] as Line).p2.Y, delta)== 0))
                            {
                                if(floatEqual(lc.slope, slope, delta) == 0 || floatEqual(lc.slope, -slope, delta)==0)
                                {
                                    lc.isReached = true;
                                    anotherConditionsList.reachedConditions.Add(lc);
                                    anotherConditionsList.unmetConditions.Remove(lc);
                                }
                            }
                        }
                        else if(anotherConditionsList.unmetConditions[j] is CircleCondition && coordinate.pointSetList[i] is Circle)
                        {
                            CircleCondition cc = anotherConditionsList.unmetConditions[j] as CircleCondition;
                            Circle circle = coordinate.pointSetList[i] as Circle;
                            if (floatEqual(cc.wantX, circle.center.X, delta) == 0 && floatEqual(cc.wantY,  circle.center.Y, delta) == 0 && floatEqual(cc.radius, circle.GetRadius(), delta) == 0)
                            {
                                cc.isReached = true;
                                anotherConditionsList.reachedConditions.Add(cc);
                                anotherConditionsList.unmetConditions.Remove(cc);
                            }
                        }
                    }
                }
                if(anotherConditionsList.unmetConditions.Count == 0)                    //过关
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 用户通关通知事件委托
        /// </summary>
        public void passNotify()
        {
            if (isWin())
            {
                if(missionSuccess != null) missionSuccess();                   //通知函数
            }
        }

        /// <summary>
        /// 测试当前元素是否符合某个过关条件的某一项
        /// </summary>
        /// <param name="newGeometry">新增元素</param>
        /// <returns></returns>
        public bool TestGeometry(Models.Geometry.Geometry newGeometry)
        {
            if (useAnotherCondition || this.conditionLists == null) return true;
            for(int i = 0; i < meetingconditionLists.Count; i++)
            {
                Condition condition = null;
                if (newGeometry is Point2)
                {
                    Point2 point = newGeometry as Point2;

                    for (int j = 0; j < meetingconditionLists[i].unmetConditions.Count; j++)
                    {
                        condition = meetingconditionLists[i].unmetConditions[j];
                        if (condition.isMeetTheConditions) throw new Exception("移动条件时出错");
                        if (condition is FreeCondition)                                     //自由的点
                        {
                            FreeCondition fc = condition as FreeCondition;
                            fc.pid = coordinate.GeometryCount;
                            fc.point = point;
                            fc.isMeetTheConditions = true;
                        }
                        else if (condition is OnTheTreeCondition)                           //附着点
                        {
                            OnTheTreeCondition oc = condition as OnTheTreeCondition;
                            if(point.rely.Count == 1)
                            {
                                if(oc.iid == point.rely[0].id)
                                {
                                    oc.pid = coordinate.GeometryCount;
                                    oc.point = point;
                                    oc.isMeetTheConditions = true;
                                    break;
                                }
                            }
                        }
                        else if (condition is IntersectCondition)                           //交点
                        {
                            IntersectCondition ic = condition as IntersectCondition;
                            if(point.rely.Count == 2)
                            {
                                if(point.rely[0] is Circle)
                                {
                                    if (point.rely[1] is Circle)
                                    {
                                        if (ic.i1id == point.rely[0].id && ic.i2id == point.rely[1].id && ((ic.clock == 1 && point.markOfTwoIntersectPointOnCircle) || (ic.clock == 2 && !point.markOfTwoIntersectPointOnCircle)))
                                        {
                                            ic.pid = coordinate.GeometryCount;
                                            ic.point = point;
                                            ic.isMeetTheConditions = true;
                                            break;
                                        }
                                        else if (ic.i1id == point.rely[1].id && ic.i2id == point.rely[0].id && ((ic.clock == 1 && !point.markOfTwoIntersectPointOnCircle) || (ic.clock == 2 && point.markOfTwoIntersectPointOnCircle)))
                                        {
                                            ic.pid = coordinate.GeometryCount;
                                            ic.point = point;
                                            ic.isMeetTheConditions = true;
                                            break;
                                        }
                                    }
                                    else if (point.rely[1] is Line)
                                    {
                                        if ((ic.i1id == point.rely[0].id && ic.i2id == point.rely[1].id || ic.i1id == point.rely[1].id && ic.i2id == point.rely[0].id) && ((ic.clock == 1 && point.markOfTwoIntersectPointOnCircle) || (ic.clock == 2 && !point.markOfTwoIntersectPointOnCircle)))
                                        {
                                            ic.pid = coordinate.GeometryCount;
                                            ic.point = point;
                                            ic.isMeetTheConditions = true;
                                            break;
                                        }
                                    }
                                }
                                else if(point.rely[0] is Line)
                                {
                                    if(point.rely[1] is Circle)
                                    {
                                        if ( (ic.i1id == point.rely[0].id && ic.i2id == point.rely[1].id || ic.i1id == point.rely[1].id && ic.i2id == point.rely[0].id) && ((ic.clock == 1 && point.markOfTwoIntersectPointOnCircle) || (ic.clock == 2 && !point.markOfTwoIntersectPointOnCircle)))
                                        {
                                            ic.pid = coordinate.GeometryCount;
                                            ic.point = point;
                                            ic.isMeetTheConditions = true;
                                            break;
                                        }
                                    }
                                    else if (point.rely[1] is Line)
                                    {
                                        if (ic.i1id == point.rely[0].id && ic.i2id == point.rely[1].id || ic.i1id == point.rely[1].id && ic.i2id == point.rely[0].id)
                                        {
                                            ic.pid = coordinate.GeometryCount;
                                            ic.point = point;
                                            ic.isMeetTheConditions = true;
                                            break;
                                        }
                                    }
                                }
                                
                            }
                        }
                        condition = null;
                    }

                }
                else if(newGeometry is Line)
                {
                    Line line = newGeometry as Line;

                    for (int j = 0; j < meetingconditionLists[i].unmetConditions.Count; j++)
                    {
                        condition = meetingconditionLists[i].unmetConditions[j];
                        if (condition.isMeetTheConditions) throw new Exception("移动条件时出错");
                        if (condition is PenDrawCondition)
                        {
                            PenDrawCondition pc = condition as PenDrawCondition;
                            if (pc.type == 1 && ((pc.p1id == line.p1.id && pc.p2id == line.p2.id) || (pc.p1id == line.p2.id && pc.p2id == line.p1.id) ))                 //此直线属于某个条件
                            {
                                if (meetingconditionLists[i].isReached(pc.p1id) && meetingconditionLists[i].isReached(pc.p2id))       //此直线满足此条件
                                {
                                    pc.iid = coordinate.GeometryCount;
                                    pc.pointSet = line;
                                    pc.isMeetTheConditions = true;
                                    break;
                                }
                            }
                        }
                        condition = null;
                    }
                }
                else if(newGeometry is Circle)
                {
                    Circle circle = newGeometry as Circle;
                    for (int j = 0; j < meetingconditionLists[i].unmetConditions.Count; j++)
                    {
                        condition = meetingconditionLists[i].unmetConditions[j];
                        if (condition.isMeetTheConditions) throw new Exception("移动条件时出错");
                        if (condition is PenDrawCondition)
                        {
                            PenDrawCondition pc = condition as PenDrawCondition;
                            if(pc.type == 2 && pc.p1id == circle.center.id && pc.p2id == circle.radius.id )                 //此圆属于某个条件
                            {
                                if (meetingconditionLists[i].isReached(circle.center.id) && meetingconditionLists[i].isReached(circle.radius.id))       //此圆满足此条件
                                {
                                    pc.iid = coordinate.GeometryCount;
                                    pc.pointSet = circle;
                                    pc.isMeetTheConditions = true;
                                    break;
                                }
                            }
                        }
                        condition = null;
                    }
                }
                if (condition != null)
                {
                    if (!condition.isMeetTheConditions) throw new Exception("不可能的异常");
                    meetingconditionLists[i].reachedConditions.Add(condition);
                    meetingconditionLists[i].unmetConditions.Remove(condition);
                    return true;
                }
            }
            return false;
        }

        private int floatEqual(float f1, float f2, float delta)
        {
            if(Math.Abs(f1-f2) < Math.Abs(delta))
            {
                return 0;
            }
            else if(f1 > f2)
            {
                return 1;
            }
            else if(f1 < f2)
            {
                return -1;
            }
            return 0;
        }
    }
}
