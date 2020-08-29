using System.Linq;

using Mochizuki.Fantasma.Internal;

using NUnit.Framework;

namespace Mochizuki.Fantasma.Tests.Internal
{
    [TestFixture]
    internal class UsingCompareTest
    {
        [Test]
        public void CompareBetweenSystemAndOthers()
        {
            var array1 = new[] { "Apple", "System", "Microsoft" };
            var array2 = array1.OrderBy(w => w, new UsingComparer()).ToList();

            Assert.AreEqual("System", array2[0]);
            Assert.AreEqual("Apple", array2[1]);
            Assert.AreEqual("Microsoft", array2[2]);
        }

        [Test]
        public void CompareBetweenSystemAndSystem()
        {
            var array1 = new[] { "System.Collection.Generic", "System", "System.Diagnostics" };
            var array2 = array1.OrderBy(w => w, new UsingComparer()).ToList();

            Assert.AreEqual("System", array2[0]);
            Assert.AreEqual("System.Collection.Generic", array2[1]);
            Assert.AreEqual("System.Diagnostics", array2[2]);
        }
    }
}