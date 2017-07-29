using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geomystery.Controllers.Geometry
{

    public class AnotherConditionsList
    {
        /// <summary>
        /// 达成条件列表
        /// </summary>
        public List<AnotherCondition> reachedConditions { get; set; }

        /// <summary>
        /// 未达成条件列表
        /// </summary>
        public List<AnotherCondition> unmetConditions { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public AnotherConditionsList()
        {
            reachedConditions = new List<AnotherCondition>();
            unmetConditions = new List<AnotherCondition>();
        }
    }

    /// <summary>
    /// 后门条件抽象父类
    /// </summary>
    public abstract class AnotherCondition 
    {
        /// <summary>
        /// 可选控制id，小于0代表没有限制
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 该条件是否达成
        /// </summary>
        public bool isReached { get; set; }
    }

    /// <summary>
    /// 点条件
    /// </summary>
    public class PointCondition : AnotherCondition
    {
        /// <summary>
        /// 期望的 X
        /// </summary>
        public float wantX { get; set; }

        /// <summary>
        /// 期望的 Y
        /// </summary>
        public float wantY { get; set; }
    }

    /// <summary>
    /// 线条件
    /// </summary>
    public class LineCondition : AnotherCondition
    {
        /// <summary>
        /// 某一点的X
        /// </summary>
        public float wantX { get; set; }

        /// <summary>
        /// 某一点的Y
        /// </summary>
        public float wantY { get; set; }

        /// <summary>
        /// 如果直线的斜率存在
        /// </summary>
        public bool isExistsSlope { get; set; }

        /// <summary>
        /// 斜率
        /// </summary>
        public float slope { get; set; }
    }

    public class CircleCondition : AnotherCondition
    {
        /// <summary>
        /// 圆心的X
        /// </summary>
        public float wantX { get; set; }

        /// <summary>
        /// 圆心的Y
        /// </summary>
        public float wantY { get; set; }

        /// <summary>
        /// 圆的半径
        /// </summary>
        public float radius { get; set; }
    }
}
