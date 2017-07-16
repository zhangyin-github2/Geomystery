using Geomystery.Models.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geomystery.Controllers.Geometry
{
    //用户工具，比如点工具，直线工具，圆工具
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
        /// 这个工具需要多少个定义点
        /// </summary>
        public int NeedPointNumber { get; set; }

        Models.Geometry.Geometry GetResult(List<Point2> pointList)
        {
            Models.Geometry.Geometry result = null;
            if(toolName.Equals("点工具"))
            {
                result = null;

            }
            else if(toolName.Equals("直线工具"))
            {
                if(pointList.Count != 2)                //两点确定一条直线
                {
                    result = null;
                }
                else
                {

                }
            }
            return result;
        }
    }
}
