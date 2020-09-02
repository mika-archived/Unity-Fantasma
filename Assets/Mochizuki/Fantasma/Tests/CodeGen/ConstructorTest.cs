using System;
using System.Reflection;

using Microsoft.CodeAnalysis;

using Mochizuki.Fantasma.CodeGen;

using NUnit.Framework;

namespace Mochizuki.Fantasma.Tests.CodeGen
{
    [TestFixture]
    internal class ConstructorTest
    {
        [Test]
        [TestCase(typeof(AClass), 0, false, "public AClass(){}")]
        [TestCase(typeof(AClass), 1, false, "public AClass(string str, int i){}")]
        [TestCase(typeof(AClass), 1, true, "protected internal AClass(){}")]
        [TestCase(typeof(BClass<>), 0, false, "public BClass(T t){}")]
        [TestCase(typeof(BClass<>), 0, true, "protected internal BClass(){}")]
        [TestCase(typeof(CClass), 0, true, "protected internal CClass(){}")]
        public void DeclarationToSyntaxTest(Type cls, int ctor, bool isInternal, string expected)
        {
            var constructor = cls.GetConstructors(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly)[ctor];
            Assert.AreEqual(expected, new Constructor(constructor, isInternal).DeclarationToSyntax(true).NormalizeWhitespace().ToFullString().Replace(Environment.NewLine, ""));
        }

        // ReSharper disable UnusedParameter.Local

        private class AClass
        {
            public AClass() : this(string.Empty, 0) { }

            public AClass(string str, int i) { }
        }

        private class BClass<T> : AClass
        {
            public BClass(T t) { }
        }

        private class CClass { }

        // ReSharper restore UnusedParameter.Local
    }
}