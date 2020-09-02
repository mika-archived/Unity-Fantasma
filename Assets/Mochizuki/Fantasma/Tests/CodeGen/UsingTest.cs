using System;

using Microsoft.CodeAnalysis;

using Mochizuki.Fantasma.CodeGen;

using NUnit.Framework;

using Object = UnityEngine.Object;

namespace Mochizuki.Fantasma.Tests.CodeGen
{
    [TestFixture]
    internal class UsingTest
    {
        [Test]
        [TestCase(typeof(Type), false, "using System;")]
        [TestCase(typeof(TestFixtureAttribute), false, "using NUnit.Framework;")]
        [TestCase(typeof(Object), true, "using Object = UnityEngine.Object;")]
        public void DeclarationToSyntax(Type cls, bool isAlias, string expected)
        {
            Assert.AreEqual(expected, new Using(cls, isAlias).DeclarationToSyntax().NormalizeWhitespace().ToFullString());
        }
    }
}