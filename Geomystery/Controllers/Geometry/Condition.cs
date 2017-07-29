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
        /// 达成条件列表
        /// </summary>
        public List<Condition> reachedConditions { get; set; }

        /// <summary>
        /// 未达成条件列表
        /// </summary>
        public List<Condition> unmetCnditions { get; set; }

        /// <summary>
        /// 复制
        /// </summary>
        public ConditionsList Copy()
        {
            ConditionsList result = new ConditionsList();
            if(reachedConditions != null)
            {
                result.reachedConditions = new List<Condition>();
                for(int i = 0; i < reachedConditions.Count; i++)
                {
                    if(reachedConditions[i] is FreeCondition)
                    {
                        FreeCondition fc = reachedConditions[i] as FreeCondition;
                        result.reachedConditions.Add(new FreeCondition() { isMeetTheConditions = fc.isMeetTheConditions, pid = fc.pid, point = fc.point });
                    }
                    else if(reachedConditions[i] is PenDrawCondition)
                    {
                        PenDrawCondition pc = reachedConditions[i] as PenDrawCondition;
                        result.reachedConditions.Add(new PenDrawCondition() { isMeetTheConditions = pc.isMeetTheConditions, iid = pc.iid, p1 = pc.p1, p1id = pc.p1id, p2 = pc.p2, p2id = pc.p2id, pointSet = pc.pointSet });
                    }
                    else if (reachedConditions[i] is OnTheTreeCondition)
                    {
                        OnTheTreeCondition oc = reachedConditions[i] as OnTheTreeCondition;
                        result.reachedConditions.Add(new OnTheTreeCondition() { isMeetTheConditions = oc.isMeetTheConditions, pid = oc.pid, point = oc.point});
                    }
                    else if(reachedConditions[i] is IntersectCondition)
                    {
                        IntersectCondition ic = reachedConditions[i] as IntersectCondition;
                        result.reachedConditions.Add(new IntersectCondition() { isMeetTheConditions = ic.isMeetTheConditions, i1id = ic.i1id, i2id = ic.i2id, pid = ic.pid, point = ic.point, pointSet1 = ic.pointSet1, pointSet2 = ic.pointSet2 });
                    }
                }
            }
            if (unmetCnditions != null)
            {
                result.unmetCnditions = new List<Condition>();
                for (int i = 0; i < unmetCnditions.Count; i++)
                {
                    if (unmetCnditions[i] is FreeCondition)
                    {
                        FreeCondition fc = unmetCnditions[i] as FreeCondition;
                        result.unmetCnditions.Add(new FreeCondition() { isMeetTheConditions = fc.isMeetTheConditions, pid = fc.pid, point = fc.point });
                    }
                    else if (unmetCnditions[i] is PenDrawCondition)
                    {
                        PenDrawCondition pc = unmetCnditions[i] as PenDrawCondition;
                        result.unmetCnditions.Add(new PenDrawCondition() { isMeetTheConditions = pc.isMeetTheConditions, iid = pc.iid, p1 = pc.p1, p1id = pc.p1id, p2 = pc.p2, p2id = pc.p2id, pointSet = pc.pointSet });
                    }
                    else if (unmetCnditions[i] is OnTheTreeCondition)
                    {
                        OnTheTreeCondition oc = unmetCnditions[i] as OnTheTreeCondition;
                        result.unmetCnditions.Add(new OnTheTreeCondition() { isMeetTheConditions = oc.isMeetTheConditions, pid = oc.pid, point = oc.point, iid = oc.iid, pointSet = oc.pointSet });
                    }
                    else if (unmetCnditions[i] is IntersectCondition)
                    {
                        IntersectCondition ic = unmetCnditions[i] as IntersectCondition;
                        result.unmetCnditions.Add(new IntersectCondition() { isMeetTheConditions = ic.isMeetTheConditions, i1id = ic.i1id, i2id = ic.i2id, pid = ic.pid, point = ic.point, pointSet1 = ic.pointSet1, pointSet2 = ic.pointSet2 });
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 这个id是否已经完成
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool isReached(int id)
        {
            for(int i = 0; i < reachedConditions.Count; i++)
            {
                if (!reachedConditions[i].isMeetTheConditions) throw new Exception("曾经的已达成与未达成操作错误");
                if (reachedConditions[i] is FreeCondition)
                {
                    FreeCondition fc = reachedConditions[i] as FreeCondition;
                    if (fc.pid == id) return true;
                }
                else if (reachedConditions[i] is PenDrawCondition)
                {
                    PenDrawCondition pc = reachedConditions[i] as PenDrawCondition;
                    if (pc.iid == id) return true;
                }
                else if (reachedConditions[i] is OnTheTreeCondition)
                {
                    OnTheTreeCondition oc = reachedConditions[i] as OnTheTreeCondition;
                    if (oc.pid == id) return true;
                }
                else if (reachedConditions[i] is IntersectCondition)
                {
                    IntersectCondition ic = reachedConditions[i] as IntersectCondition;
                    if (ic.pid == id) return true;
                }
            }
            return false;
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
        /// 画的是线 1 画的是圆 2
        /// </summary>
        public int type { get; set; }

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

        /// <summary>
        /// 0 随机点 1 逆时针第一个 2 逆时针第二个（当相交点集有一个是圆的时候）
        /// </summary>
        public int clock { get; set; }
    }
}
