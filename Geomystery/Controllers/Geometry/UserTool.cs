using Geomystery.Controllers.Geometry;
using Geomystery.Models.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geomystery.Controllers.Geometry
{
    /// <summary>
    /// 用户工具，比如点工具，直线工具，圆工具
    /// </summary>
    public class UserTool
    {
        /// <summary>
        /// 工具名
        /// </summary>
        public string toolName { get; set; }

        /// <summary>
        /// 工具图标地址（先写一个）
        /// </summary>
        public string toolIcon { get; set; }

        /// <summary>
        /// 这个工具需要需求列表
        /// </summary>
        public List<NeedGeometrySetItem> NeedGeometryList { get; set; }

        /// <summary>
        /// 返回相应工具的操作结果
        /// </summary>
        /// <param name="pointList"></param>
        /// <returns></returns>
        List<Models.Geometry.Geometry> GetResult(List<Models.Geometry.Geometry> geometryList)
        {
            List<Models.Geometry.Geometry> result = null;

            if (toolName.Equals("选择工具"))
            {
                result = null;
            }
            if (toolName.Equals("交点工具"))
            {
                if(geometryList.Count == 2)             //两个元素
                {
                    if(geometryList[0] is IPointSet && geometryList[1] is IPointSet)       //两个点集
                    {
                        result = new List<Models.Geometry.Geometry>();
                        var pointList = (geometryList[0] as IPointSet).Intersection(geometryList[1] as IPointSet);
                        foreach(Point2 p in pointList)
                        {
                            result.Add(p);
                        }
                    }
                }
            }
            else if (toolName.Equals("点工具"))
            {
                result = null;

            }
            else if(toolName.Equals("直线工具"))
            {
                if(geometryList.Count == 2)                //两点确定一条直线
                {
                    if(geometryList[0].coord == geometryList[1].coord)            //两个点必须在同一个坐标系下
                    {
                        Line line = new Line();
                        if(geometryList[0] is Point2 && geometryList[1] is Point2)      //两个实体都是点
                        {
                            line.p1 = geometryList[0] as Point2;
                            line.p2 = geometryList[1] as Point2;
                            line.coord = geometryList[0].coord;
                            line.type = LineType.Line;
                            line.lineRely = LineRely.Normal;
                            result = new List<Models.Geometry.Geometry>();
                            result.Add(line);
                        }
                    }
                }
            }
            else if(toolName.Equals("圆工具"))
            {
                if (geometryList.Count == 2)                //两点确定一条直线
                {
                    if (geometryList[0].coord == geometryList[1].coord)            //两个点必须在同一个坐标系下
                    {
                        Circle circle = new Circle();
                        if(geometryList[0] is Point2 && geometryList[1] is Point2)
                        {
                            circle.center = geometryList[0] as Point2;
                            circle.radius = geometryList[1] as Point2;
                            circle.coord = geometryList[0].coord;
                            result = new List<Models.Geometry.Geometry>();
                            result.Add(circle);
                        }
                    }
                }
            }
            return result;
        }
    }


    /// <summary>
    /// 尺规作图工具管理器
    /// </summary>
    public class UserToolsManager
    {
        /// <summary>
        /// 工具数组
        /// </summary>
        private List<UserTool> userTools;


        /// <summary>
        /// 单例模式（懒汉式单例）
        /// </summary>
        private static UserToolsManager _instance;


        /// <summary>
        /// 私有的构造函数
        /// </summary>
        private UserToolsManager()
        {
           
        }

        /// <summary>
        /// 获取单例
        /// </summary>
        /// <returns></returns>
        public static UserToolsManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new UserToolsManager();

                _instance.userTools = new List<UserTool>();

                _instance.userTools.Add(new UserTool()
                {
                    toolName = "选择工具",
                    toolIcon = "ms-appx:///Pictures/Tool/tool1.png",
                    NeedGeometryList = null,
                });

                List<NeedGeometrySetItem> crossTool = new List<NeedGeometrySetItem>();
                crossTool.Add(new NeedGeometrySetItem() { type = typeof(IPointSet), needNumber = 2 });
                _instance.userTools.Add(new UserTool()
                {
                    toolName = "交点工具",
                    toolIcon = "ms-appx:///Pictures/Tool/tool2.png",
                    NeedGeometryList = crossTool,
                });

                List<NeedGeometrySetItem> pointTool = new List<NeedGeometrySetItem>();
                pointTool.Add(new NeedGeometrySetItem() { type = typeof(Point2), needNumber = 1 });
                _instance.userTools.Add(new UserTool()
                {
                    toolName = "点工具",
                    toolIcon = "ms-appx:///Pictures/Tool/tool3.png",
                    NeedGeometryList = pointTool,
                });

                List<NeedGeometrySetItem> lineTool = new List<NeedGeometrySetItem>();
                lineTool.Add(new NeedGeometrySetItem() { type = typeof(Point2), needNumber = 2 });
                _instance.userTools.Add(new UserTool()
                {
                    toolName = "直线工具",
                    toolIcon = "ms-appx:///Pictures/Tool/tool4.png",
                    NeedGeometryList = lineTool,
                });

                List<NeedGeometrySetItem> circleTool = new List<NeedGeometrySetItem>();
                circleTool.Add(new NeedGeometrySetItem() { type = typeof(Point2), needNumber = 2 });
                _instance.userTools.Add(new UserTool()
                {
                    toolName = "圆工具",
                    toolIcon = "ms-appx:///Pictures/Tool/tool5.png",
                    NeedGeometryList = circleTool,
                });
            }

            return _instance;
        }

        /// <summary>
        /// 获得工具
        /// </summary>
        /// <returns></returns>
        public List<UserTool> GetTools()
        {
            return userTools;
        }
    }

}