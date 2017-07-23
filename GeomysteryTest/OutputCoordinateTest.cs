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
        [TestMethod]
        public void TestDistanceOfPointAndLine()
        {
            Vector2 lpo = new Vector2() { X = 0, Y = 10 };
            Vector2 lv = new Vector2() { X = 1, Y = 1 } ;
            Vector2 outerPoint = new Vector2() { X = 10, Y = 10 };
            Vector2 result = new Vector2();
            float distance = OutputCoordinate.DistanceOfPointAndLine(lpo, lv, outerPoint,ref result);
            Assert.AreEqual(distance,5 * Math.Sqrt(2),0.00001f);
            float dotMulti = Vector2.Dot(outerPoint - result, lv);
            Assert.AreEqual(dotMulti, 0f, 0.00001f);
            Vector2 result2 = new Vector2();
            float distance2 = OutputCoordinate.DistanceOfPointAndLine(lpo, lv, result, ref result2);
            Assert.AreEqual(distance2, 0f, 0.00001f);
        }

        [TestMethod]
        public void TestDistanceOfPointAndCircle()
        {
            Vector2 center = new Vector2() { X = 10, Y = 10 };
            float radius = 5;
            Vector2 outerPoint = new Vector2() { X = 5, Y = 5 };
            Vector2 result = new Vector2();
            float distance = OutputCoordinate.DistanceOfPointAndCircle(center, radius, outerPoint, ref result);
            Assert.AreEqual(distance, 5 * Math.Sqrt(2), 0.00001);
            Vector2 result2 = new Vector2();
            float distance2 = OutputCoordinate.DistanceOfPointAndCircle(center, radius, result, ref result2);
            Assert.AreEqual(distance2, 0f, 0.00001f);
        }
    }
}
