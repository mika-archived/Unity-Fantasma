using System;

using Mochizuki.Fantasma.Extensions;

using NUnit.Framework;

using Object = UnityEngine.Object;

namespace Mochizuki.Fantasma.Tests.Extensions
{
    [TestFixture]
    internal class TypeExtensionsTest
    {
        // for test case
        private static class A
        {
            public class B { }
        }

        [Test]
        [TestCase(typeof(bool), "Boolean")]
        [TestCase(typeof(TypeExtensionsTest), "TypeExtensionsTest")]
        [TestCase(typeof(A.B), "TypeExtensionsTest.A.B")]
        public void FullnameWithoutNamespaceTest(Type t, string expected)
        {
            Assert.AreEqual(expected, t.FullNameWithoutNamespace());
        }

        [Test]
        [TestCase(typeof(bool), true)]
        [TestCase(typeof(byte), true)]
        [TestCase(typeof(sbyte), true)]
        [TestCase(typeof(char), true)]
        [TestCase(typeof(decimal), true)]
        [TestCase(typeof(double), true)]
        [TestCase(typeof(float), true)]
        [TestCase(typeof(int), true)]
        [TestCase(typeof(uint), true)]
        [TestCase(typeof(long), true)]
        [TestCase(typeof(ulong), true)]
        [TestCase(typeof(short), true)]
        [TestCase(typeof(ushort), true)]
        [TestCase(typeof(object), true)]
        [TestCase(typeof(string), true)]
        [TestCase(typeof(void), true)]
        [TestCase(typeof(Object), false)]
        public void IsKeywordTypeTest(Type t, bool expected)
        {
            Assert.AreEqual(expected, t.IsKeywordType());
        }

        [Test]
        [TestCase(typeof(bool), "bool")]
        [TestCase(typeof(byte), "byte")]
        [TestCase(typeof(sbyte), "sbyte")]
        [TestCase(typeof(char), "char")]
        [TestCase(typeof(decimal), "decimal")]
        [TestCase(typeof(double), "double")]
        [TestCase(typeof(float), "float")]
        [TestCase(typeof(int), "int")]
        [TestCase(typeof(uint), "uint")]
        [TestCase(typeof(long), "long")]
        [TestCase(typeof(ulong), "ulong")]
        [TestCase(typeof(short), "short")]
        [TestCase(typeof(ushort), "ushort")]
        [TestCase(typeof(object), "object")]
        [TestCase(typeof(string), "string")]
        [TestCase(typeof(void), "void")]
        [TestCase(typeof(Object), "Object")]
        public void KeywordNormalizedNameTest(Type t, string expected)
        {
            Assert.AreEqual(expected, t.KeywordNormalizedName());
        }
    }
}