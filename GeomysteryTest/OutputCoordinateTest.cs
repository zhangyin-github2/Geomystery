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
            Assert.AreEqual(distance,5 * Math.Sqrt(2),0.0001);
        }
    }
}
