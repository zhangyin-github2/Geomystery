using Geomystery.Models.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geomystery.Controllers.Geometry
{
    /// <summary>
    /// 某种过关条件序列
    /// </summary>
    public class ConditionsList
    {
        /// <summary>
        /// 条件列表
        /// </summary>
        public List<Condition> conditions { get; set; }

        /// <summary>
        /// 复制
        /// </summary>
        public void copy()
        {

        }
    }


    /// <summary>
    /// 条件项（过关条件项）
    /// </summary>
    public abstract class Condition
    {
        /// <summary>
        /// 本条件是否得到满足
        /// </summary>
        public bool isMeetTheConditions { get; set; }
    }

    /// <summary>
    /// 点的自由依赖（画笔自由画点）条件
    /// </summary>
    public class FreeCondition : Condition
    {
        /// <summary>
        /// 自由的点，应该是关卡初始给定的点
        /// </summary>
        public Point2 point { get; set; }

        /// <summary>
        /// 自由的点的id
        /// </summary>
        public int pid { get; set; }
    }

    /// <summary>
    /// 两点确定一个点集（直尺，圆规）
    /// </summary>
    public class PenDrawCondition : Condition
    {
        /// <summary>
        /// 直线或者圆，依赖于两个不同的点，这两个点的顺序不分先后
        /// </summary>
        public IPointSet pointSet { get; set; }

        /// <summary>
        /// 这个点集的id
        /// </summary>
        public int iid { get; set; }

        /// <summary>
        /// 其中一个点
        /// </summary>
        public Point2 p1 { get; set; }

        /// <summary>
        /// 其中另一个点
        /// </summary>
        public Point2 p2 { get; set; }

        /// <summary>
        /// 某个点的id
        /// </summary>
        public int p1id { get; set; }

        /// <summary>
        /// 某另一个点的id
        /// </summary>
        public int p2id { get; set; }
    }

    /// <summary>
    /// 点依附在点集上（笔在线上或者圆上画点）
    /// </summary>
    public class OnTheTreeCondition : Condition
    {
        /// <summary>
        /// 某个点，依赖于一个圆，或者依赖于一条直线
        /// </summary>
        public Point2 point { get; set; }

        /// <summary>
        /// 点的id
        /// </summary>
        public int pid { get; set; }

        /// <summary>
        /// 点依赖的某个圆，或者某条直线
        /// </summary>
        public IPointSet pointSet { get; set; }

        /// <summary>
        /// 点集的id
        /// </summary>
        public int iid { get; set; }
    }

    /// <summary>
    /// 线与圆的交点（直线或者圆互相之间的交点）
    /// </summary>
    public class IntersectCondition : Condition
    {
        /// <summary>
        /// 某个点，是两个点集的交点
        /// </summary>
        public Point2 point { get; set; }

        /// <summary>
        /// 点的id
        /// </summary>
        public int pid { get; set; }

        /// <summary>
        /// 点依赖的某个圆，或者某条直线
        /// </summary>
        public IPointSet pointSet1 { get; set; }

        /// <summary>
        /// 点集的id
        /// </summary>
        public int i1id { get; set; }

        /// <summary>
        /// 点依赖的某个圆，或者某条直线
        /// </summary>
        public IPointSet pointSet2 { get; set; }

        /// <summary>
        /// 点集的id
        /// </summary>
        public int i2id { get; set; }
    }
}
