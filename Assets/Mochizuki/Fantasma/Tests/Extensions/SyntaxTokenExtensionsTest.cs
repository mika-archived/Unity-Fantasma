using Microsoft.CodeAnalysis.CSharp;

using Mochizuki.Fantasma.Extensions;

using NUnit.Framework;

namespace Mochizuki.Fantasma.Tests.Extensions
{
    [TestFixture]
    internal class SyntaxTokenExtensionsTest
    {
        [Test]
        [TestCase("hello", "hello")]
        [TestCase("class", "@class")]
        public void Normalize(string identifier, string expected)
        {
            Assert.AreEqual(SyntaxFactory.Identifier(identifier).Normalize().ToFullString(), expected);
        }
    }
}