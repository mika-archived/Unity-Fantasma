using System;
using System.Reflection;

using Microsoft.CodeAnalysis;

using Mochizuki.Fantasma.CodeGen;

using NUnit.Framework;

namespace Mochizuki.Fantasma.Tests.CodeGen
{
    [TestFixture]
    internal class ConversionOperatorTest
    {
        // IntWrapper works as Int32
        private class IntWrapper
        {
            private readonly int _value;

            public IntWrapper(int value)
            {
                _value = value;
            }

            #region Overloads

            // operator overloads
            // reference: https://docs.microsoft.com/ja-jp/dotnet/csharp/language-reference/operators/operator-overloading

            // user-defined conversion
            public static implicit operator bool(IntWrapper a)
            {
                return a._value != 0;
            }

            public static explicit operator IntWrapper(bool a)
            {
                return new IntWrapper(a ? 1 : 0);
            }

            #endregion
        }

        [Test]
        [TestCase(typeof(IntWrapper), "op_Implicit", "public static implicit operator bool (ConversionOperatorTest.IntWrapper a){    return default;}")]
        [TestCase(typeof(IntWrapper), "op_Explicit", "public static explicit operator ConversionOperatorTest.IntWrapper(bool a){    return default;}")]
        public void DeclarationToSyntax(Type cls, string @operator, string expected)
        {
            var overload = cls.GetMethod(@operator, BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly);
            Assert.AreEqual(expected, new ConversionOperator(overload).DeclarationToSyntax(true).NormalizeWhitespace().ToFullString().Replace(Environment.NewLine, ""));
        }
    }
}