using Geomystery.Models.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Geomystery.Views.Geometry
{
    /// <summary>
    /// 一个结构体，用来记录某一个（屏幕）点，到屏幕上（模型）元素的屏幕模型的距离
    /// </summary>
    public class GeometryAndTheDistance : IComparable<GeometryAndTheDistance>
    {
        /// <summary>
        /// 因为同一个模型可能有多个视图，所以只记录这个“屏幕元素”的“模型”
        /// </summary>
        public Models.Geometry.Geometry geometry { get; set; }

        /// <summary>
        /// 这个距离是屏幕（视图）距离
        /// </summary>
        public float distance { get; set; }

        /// <summary>
        /// 显式实现IComparable接口
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        int IComparable<GeometryAndTheDistance>.CompareTo(GeometryAndTheDistance other)
        {
            if(geometry.rely.Count != other.geometry.rely.Count)
            {
                return other.geometry.rely.Count.CompareTo(geometry.rely.Count);
            }
            return distance.CompareTo(other.distance);
        }
    }

    /// <summary>
    /// 保存屏幕（视图坐标系）上，鼠标某一点周围有什么元素的数据结构
    /// </summary>
    public class Surroundings
    {
        /// <summary>
        /// 屏幕上的点
        /// </summary>
        public Vector2 screenPoint { get; set; }

        
        /// <summary>
        /// 周围的点
        /// </summary>
        public List<GeometryAndTheDistance> surroundingPoint { get; set; }

        /// <summary>
        /// 周围的线
        /// </summary>
        public List<GeometryAndTheDistance> surroundingLine { get; set; }

        /// <summary>
        /// 周围的园
        /// </summary>
        public List<GeometryAndTheDistance> surroundingCircle { get; set; }

        public Surroundings()
        {
            surroundingPoint = new List<GeometryAndTheDistance>();
            surroundingLine = new List<GeometryAndTheDistance>();
            surroundingCircle = new List<GeometryAndTheDistance>();
        }

        /// <summary>
        /// 将附近点线圆数组排序
        /// </summary>
        public void SortAll()
        {
            surroundingPoint.Sort();
            surroundingLine.Sort();
            surroundingCircle.Sort();
        }
    }
}
