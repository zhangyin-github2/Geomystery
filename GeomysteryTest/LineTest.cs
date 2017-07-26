using Geomystery.Models.Geometry;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeomysteryTest
{
    [TestClass]
    public class LineTest
    {
        public float delta = 1e-7F;

        [TestMethod]
        public void TestIntersection()
        {
            Point2 p1 = new Point2() { X = 1, Y = 1,  };
            Point2 p2 = new Point2() { X = 2, Y = 2 };
            Point2 p3 = new Point2() { X = 4, Y = 2 };
            Point2 p4 = new Point2() { X = 5, Y = 1 };
            Line l1 = new Line() { p1 = p1, p2 = p2, lineRely=LineRely.Normal };
            Line l2 = new Line() { p1 = p3, p2 = p4, lineRely = LineRely.Normal };

            List<Point2> pl = ((IPointSet)l1).Intersection(l2);
            //Assert.AreEqual(pl.Count, 1);
            //Assert.AreEqual(pl[0].X, 3f, delta);
            //Assert.AreEqual(pl[0].Y, 3f, delta);
        }
    }
}
