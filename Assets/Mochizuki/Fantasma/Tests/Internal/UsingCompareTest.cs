using System.Linq;

using Mochizuki.Fantasma.Internal;

using NUnit.Framework;

namespace Mochizuki.Fantasma.Tests.Internal
{
    [TestFixture]
    internal class UsingCompareTest
    {
        [Test]
        [TestCase(new[] { "Apple", "System", "Microsoft" }, new[] { "System", "Apple", "Microsoft" })]
        [TestCase(new[] { "System.Collection.Generic", "System", "System.Diagnostics" }, new[] { "System", "System.Collection.Generic", "System.Diagnostics" })]
        public void CompareTest(string[] input, string[] expected)
        {
            CollectionAssert.AreEqual(expected, input.OrderBy(w => w, new UsingComparer()).ToArray());
        }
    }
}