using Geomystery.Views.Geometry;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GeomysteryTest
{
    [TestClass]
    public class OutputCoordinateTest
    {
        float delta = 0.000001f;                //误差项

        [TestMethod]
        public void TestDistanceOfPointAndLine()
        {
            Vector2 lpo = new Vector2() { X = 0, Y = 10 };                  //直线上的点点
            Vector2 lv = new Vector2() { X = 1, Y = 1 } ;                   //直线方向向量（非0）
            Vector2 outerPoint = new Vector2() { X = 10, Y = 10 };          //直线上或者直线外的一点
            Vector2 result = new Vector2() { X = 1, Y = 1 };                //过outerPoint作l的垂线，result为垂足
            float distance = OutputCoordinate.DistanceOfPointAndLine(lpo, lv, outerPoint,ref result);   //点到直线距离
            Assert.AreEqual(distance,5 * Math.Sqrt(2), delta);
            Assert.IsFalse(result.X == 1);
            Assert.IsFalse(result.Y == 1);
            float dotMulti = Vector2.Dot(outerPoint - result, lv);          //垂线
            Assert.AreEqual(dotMulti, 0f, delta);
            Vector2 result2 = new Vector2();
            float distance2 = OutputCoordinate.DistanceOfPointAndLine(lpo, lv, result, ref result2);       //垂足在l上
            Assert.AreEqual(distance2, 0f, delta);
        }

        [TestMethod]
        public void TestDistanceOfPointAndCircle()
        {
            Vector2 center = new Vector2() { X = 10, Y = 10 };              //圆心
            float radius = 5;                                           //半径（>0）
            Vector2 outerPoint = new Vector2() { X = 5, Y = 5 };        //圆外一点或圆内一点或圆上一点
            Vector2 result = new Vector2() { X = 0, Y = 0 };            //类似与垂足，result为圆上与outerPoint最接近的点(连接圆心与outerPoint并两端延长，与圆的最近的交点)
            float distance = OutputCoordinate.DistanceOfPointAndCircle(center, radius, outerPoint, ref result);     //点到圆最近的距离
            Assert.AreEqual(distance, 5 * Math.Sqrt(2) - 5, delta);
            Assert.IsFalse(result.X == 0);
            Assert.IsFalse(result.Y == 0);
            Vector2 result2 = new Vector2();
            float distance2 = OutputCoordinate.DistanceOfPointAndCircle(center, radius, result, ref result2);       //最近的点在圆上
            Assert.AreEqual(distance2, 0f, delta);
        }
    }
}
