using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework; 

namespace DiplomKurs
{
    [TestFixture]
    class Testing
    {
        [Test]
        public void Test1()
        {
            Assert.AreEqual(true,true);
        }

        [Test]
        public void TestMathHope()
        {
            double[] arr = new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            Assert.AreEqual(5,Mathematic.MathematicHope(arr)); 
        }

        [Test]
        public void TestDispersia()
        {
            double[] arr = new double[] { 1, 2, 3, 4, 5, 6 };
            Assert.AreEqual((double)(35.0/12.0),Mathematic.Dispersia(arr)); 
        }

        [Test]
        public void TestGet_dti()
        {
            List<bool> ls = new List<bool> {true,true,true,false,true,false,false,true,false,true,false,false,false,false,false,true };
            var newLs = Imovirnist.Get_dti(ls);

            List<int> realLs = new List<int>() {0,0,1,2,1,5 };

            Assert.AreEqual(realLs,newLs); 

        }

        [Test]
        public void TestGetLitelP()
        {
            List<bool> ls = new List<bool> { true, true, true, false, true, false, false, true, false, true, false, false, false, false, false, true };
            var res = Imovirnist.GetLitelP(ls);

            List<TwoIntPair> realRes = new List<TwoIntPair>() 
            {new TwoIntPair(0,2),new TwoIntPair(1,2),new TwoIntPair(2,1),new TwoIntPair(5,1) };

            Assert.AreEqual(realRes, res); 
        }
    }
}
