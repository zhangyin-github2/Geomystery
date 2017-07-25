using Geomystery.Models.FMatrix;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeomysteryTest
{
    [TestClass]
    public class FMatrixText
    {
        [TestMethod]
        public void TestAij()
        {
            FMatrix<int> mat = new FMatrix<int>(3,3,-1);
            int number = 0;
            for(int i = 0; i < 3; i++)
            {
                for(int j = 0; j < 3; j++)
                {
                    mat.matrix[i][j] = number;
                    number++;
                }
            }
            FMatrix<int> matAij = FMatrix<int>.Aij(mat, 1, 1);
            Assert.IsTrue(matAij.row == 2);
            Assert.IsTrue(matAij.column == 2);
            Assert.IsTrue(matAij.matrix[0][0] == 0);
            Assert.IsTrue(matAij.matrix[0][1] == 2);
            Assert.IsTrue(matAij.matrix[1][0] == 6);
            Assert.IsTrue(matAij.matrix[1][1] == 8);

            FMatrix<int> matAij2 = FMatrix<int>.Aij(mat, 0, 1);
            Assert.IsTrue(matAij2.row == 2);
            Assert.IsTrue(matAij2.column == 2);
            Assert.IsTrue(matAij2.matrix[0][0] == -3);
            Assert.IsTrue(matAij2.matrix[0][1] == -5);
            Assert.IsTrue(matAij2.matrix[1][0] == -6);
            Assert.IsTrue(matAij2.matrix[1][1] == -8);
        }
    }
}
