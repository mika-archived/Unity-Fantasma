using System;

using Microsoft.CodeAnalysis;

using Mochizuki.Fantasma.CodeGen;

using NUnit.Framework;

using UnityEngine;

namespace Mochizuki.Fantasma.Tests.CodeGen
{
    [TestFixture]
    internal class NamespaceTest
    {
        public class AClass { }

        [Test]
        [TestCase(typeof(GameObject), "namespace UnityEngine{}")]
        [TestCase(typeof(AClass), "namespace Mochizuki.Fantasma.Tests.CodeGen{}")]
        public void DeclarationToSyntaxTest(Type t, string expected)
        {
            Assert.AreEqual(expected, new Namespace(t).DeclarationToSyntax(true).NormalizeWhitespace().ToFullString().Replace(Environment.NewLine, ""));
        }
    }
}