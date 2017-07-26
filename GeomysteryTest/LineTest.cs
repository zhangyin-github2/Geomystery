using Geomystery.Models.Geometry;
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
    public class LineTest
    {
        public float delta = 1e-7F;

        /// <summary>
        /// 测试直线与直线相交
        /// </summary>
        [TestMethod]
        public void TestIntersectionWithLine()
        {
            Point2 p1 = new Point2() { X = 1, Y = 1 };
            Point2 p2 = new Point2() { X = 2, Y = 2 };
            Point2 p3 = new Point2() { X = 4, Y = 2 };
            Point2 p4 = new Point2() { X = 5, Y = 1 };
            Line l1 = new Line() { p1 = p1, p2 = p2, lineRely=LineRely.Normal };
            Line l2 = new Line() { p1 = p3, p2 = p4, lineRely = LineRely.Normal };

            List<Point2> p1ist = ((IPointSet)l1).Intersection(l2);
            Assert.AreEqual(p1ist.Count, 1);
            Assert.AreEqual(p1ist[0].X, 3f, delta);
            Assert.AreEqual(p1ist[0].Y, 3f, delta);

            Vector2 result1 = new Vector2();
            float distance1 = Geomystery.Views.Geometry.OutputCoordinate.DistanceOfPointAndLine(l1.GetCenterPoint().ToVector2(), l1.GetVector(), p1ist[0].ToVector2(), ref result1);
            Assert.AreEqual(distance1, 0f, delta);                      //交点在第一条线上
            Vector2 result2 = new Vector2();
            float distance2 = Geomystery.Views.Geometry.OutputCoordinate.DistanceOfPointAndLine(l2.GetCenterPoint().ToVector2(), l2.GetVector(), p1ist[0].ToVector2(), ref result2);
            Assert.AreEqual(distance2, 0f, delta);                      //交点在第二条线上
        }

        /// <summary>
        /// 测试直线与圆相交
        /// </summary>
        [TestMethod]
        public void TestIntersectionWithCircle()
        {
            Point2 center = new Point2() { X = 3, Y = 3 };
            Point2 radius = new Point2() { X = 1, Y = 3 };
            Point2 p1 = new Point2() { X = 0, Y = 4 };
            Point2 p2 = new Point2() { X = 4, Y = 0 };
            Point2 p3 = new Point2() { X = 0, Y = 1 };
            Point2 p4 = new Point2() { X = 1, Y = 0 };

            Circle c1 = new Circle() { center = center, radius = radius };
            Line l1 = new Line() { p1 = p1, p2 = p2, lineRely = LineRely.Normal };
            Line l2 = new Line() { p1 = p3, p2 = p4, lineRely = LineRely.Normal };

            List<Point2> p1ist1 = ((IPointSet)l1).Intersection(c1);
            Assert.AreEqual(p1ist1.Count, 2);
            Vector2 result1 = new Vector2();
            Vector2 result2 = new Vector2();
            float distance1 = Geomystery.Views.Geometry.OutputCoordinate.DistanceOfPointAndLine(l1.GetCenterPoint().ToVector2(), l1.GetVector(), p1ist1[0].ToVector2(), ref result1);
            float distance2 = Geomystery.Views.Geometry.OutputCoordinate.DistanceOfPointAndLine(l1.GetCenterPoint().ToVector2(), l1.GetVector(), p1ist1[1].ToVector2(), ref result2);
            Assert.AreEqual(distance1, 0, delta);
            Assert.AreEqual(distance2, 0, delta);           //两个点都在直线上
            Vector2 result3 = new Vector2();
            Vector2 result4 = new Vector2();
            float distance3 = Geomystery.Views.Geometry.OutputCoordinate.DistanceOfPointAndCircle(c1.center.ToVector2(), c1.GetRadius(), p1ist1[0].ToVector2(), ref result3);
            float distance4 = Geomystery.Views.Geometry.OutputCoordinate.DistanceOfPointAndCircle(c1.center.ToVector2(), c1.GetRadius(), p1ist1[1].ToVector2(), ref result4);
            Assert.AreEqual(distance3, 0, delta);
            Assert.AreEqual(distance4, 0, delta);           //两个点都在圆上

            List<Point2> p1ist2 = ((IPointSet)l2).Intersection(c1);         //没有交点
            Assert.AreEqual(p1ist2.Count, 0);
        }
    }
}
