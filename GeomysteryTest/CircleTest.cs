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
    /// <summary>
    /// 测试圆.class
    /// </summary>
    [TestClass]
    public class CircleTest
    {
        private float delta = 1e-7F;

        /// <summary>
        /// 测试圆与直线的交点（借用直线与圆的交点）
        /// </summary>
        [TestMethod]
        public void TestCircleIntersectionWithLine()
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

            List<Point2> p1ist1 = ((IPointSet)c1).Intersection(l1);
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

            List<Point2> p1ist2 = ((IPointSet)c1).Intersection(l2);         //没有交点
            Assert.AreEqual(p1ist2.Count, 0);
        }

        /// <summary>
        /// 测试圆与圆的交点，需要大量测试
        /// </summary>
        [TestMethod]
        public void TestCircleIntersectionWithCircle()
        {
            Point2 center1 = new Point2() { X = 2, Y = 2 };
            Point2 radius1 = new Point2() { X = 0, Y = 2 };
            Circle c1 = new Circle() { center = center1, radius = radius1 };
            Point2 center2 = new Point2() { X = 4, Y = 4 };
            Point2 radius2 = new Point2() { X = 0, Y = 4 };
            Circle c2 = new Circle() { center = center2, radius = radius2 };

            List<Point2> p1ist1 = ((IPointSet)c1).Intersection(c2);
            Assert.AreEqual(p1ist1.Count, 2);
            Assert.AreEqual(p1ist1[1].X, 0.17712f, 1e-5f);
            Assert.AreEqual(p1ist1[1].Y, 2.82288f, 1e-5f);
            Assert.AreEqual(p1ist1[0].X, 2.82288f, 1e-5f);
            Assert.AreEqual(p1ist1[0].Y, 0.17712f, 1e-5f);

            List<Point2> p1ist2 = ((IPointSet)c2).Intersection(c1);
            Assert.AreEqual(p1ist2.Count, 2);
            Assert.AreEqual(p1ist2[0].X, 0.17712f, 1e-5f);
            Assert.AreEqual(p1ist2[0].Y, 2.82288f, 1e-5f);
            Assert.AreEqual(p1ist2[1].X, 2.82288f, 1e-5f);
            Assert.AreEqual(p1ist2[1].Y, 0.17712f, 1e-5f);
        }
    }
}
