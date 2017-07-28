using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geomystery.Controllers.Geometry
{
    /// <summary>
    /// 条件项（过关条件项）
    /// </summary>
    public abstract class Condition
    {

    }

    /// <summary>
    /// 点的自由依赖（画笔自由画点）
    /// </summary>
    public class FreeCondition : Condition
    {

    }

    /// <summary>
    /// 点依附在点集上（笔在线上或者圆上画点）
    /// </summary>
    public class OnTheTreeCondition : Condition
    {

    }

    /// <summary>
    /// 线与圆的交点（直线或者圆互相之间的交点）
    /// </summary>
    public class TntersectCondition : Condition
    {

    }

    /// <summary>
    /// 两点确定一个点集（直尺，圆规）
    /// </summary>
    public class PenDrawCondition : Condition
    {

    }
}
