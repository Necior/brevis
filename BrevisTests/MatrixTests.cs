using Microsoft.VisualStudio.TestTools.UnitTesting;
using Brevis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brevis.Tests
{
    [TestClass()]
    public class MatrixTests
    {
        [TestMethod()]
        public void GetValueTest()
        {
            var m = new Matrix(1, 1);
            m.SetValue(0, 0, 5);
            Assert.AreEqual(5, m.GetValue(0, 0));
            m.SetValue(0, 0, 5);
            Assert.AreEqual(5, m.GetValue(0, 0));
            m.SetValue(0, 0, 10);
            Assert.AreEqual(10, m.GetValue(0, 0));
        }

        [TestMethod()]
        public void TestMultiplicationIdentityTimesIdentityGivesIdentity()
        {
            var I = new Matrix(2, 2);
            var result = Matrix.Multiply(I, I);
            Assert.AreEqual(I, result);
        }
        [TestMethod()]
        public void TestMultiplication2x2With1x2()
        {
            var a = new Matrix(2, 2);
            a.SetValue(0, 0, 1);
            a.SetValue(0, 1, 2);
            a.SetValue(1, 0, 3);
            a.SetValue(1, 1, 4);

            var b = new Matrix(2, 1);
            b.SetValue(0, 0, 1);
            b.SetValue(1, 0, 0);

            var expected = new Matrix(2, 1);
            expected.SetValue(0, 0, 1);
            expected.SetValue(1, 0, 3);

            Assert.AreEqual(expected, Matrix.Multiply(a, b));
        }
    }
}