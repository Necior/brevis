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
    }
}