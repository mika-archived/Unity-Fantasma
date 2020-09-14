using System;
using System.Reflection;

using Microsoft.CodeAnalysis;

using Mochizuki.Fantasma.CodeGen;

using NUnit.Framework;

#pragma warning disable 660,661

namespace Mochizuki.Fantasma.Tests.CodeGen
{
    [TestFixture]
    internal class OperatorTest
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

            // unary plus
            public static IntWrapper operator +(IntWrapper a)
            {
                return new IntWrapper(+a._value);
            }

            // unary minus
            public static IntWrapper operator -(IntWrapper a)
            {
                return new IntWrapper(-a._value);
            }

            // logical negation
            public static bool operator !(IntWrapper a)
            {
                return a._value != 0;
            }

            // bitwise complement
            public static IntWrapper operator ~(IntWrapper a)
            {
                return new IntWrapper(~a._value);
            }

            // postfix/prefix increment
            public static IntWrapper operator ++(IntWrapper a)
            {
                return new IntWrapper(a._value + 1);
            }

            // postfix/prefix decrement
            public static IntWrapper operator --(IntWrapper a)
            {
                return new IntWrapper(a._value - 1);
            }

            // true
            public static bool operator true(IntWrapper a)
            {
                return a._value != 0;
            }

            // false
            public static bool operator false(IntWrapper a)
            {
                return a._value == 0;
            }

            // addition
            public static IntWrapper operator +(IntWrapper a, IntWrapper b)
            {
                return new IntWrapper(a._value + b._value);
            }

            // subtraction
            public static IntWrapper operator -(IntWrapper a, IntWrapper b)
            {
                return new IntWrapper(a._value - b._value);
            }

            // multiplication
            public static IntWrapper operator *(IntWrapper a, IntWrapper b)
            {
                return new IntWrapper(a._value * b._value);
            }

            // division
            public static IntWrapper operator /(IntWrapper a, IntWrapper b)
            {
                return new IntWrapper(a._value / b._value);
            }

            // remainder
            public static IntWrapper operator %(IntWrapper a, IntWrapper b)
            {
                return new IntWrapper(a._value % b._value);
            }

            // logical and
            public static bool operator &(IntWrapper a, IntWrapper b)
            {
                return (bool) a & (bool) b;
            }

            // logical or
            public static bool operator |(IntWrapper a, IntWrapper b)
            {
                return (bool) a | (bool) b;
            }

            // logical exclusive
            public static bool operator ^(IntWrapper a, IntWrapper b)
            {
                return (bool) a ^ (bool) b;
            }

            // left-shift
            public static IntWrapper operator <<(IntWrapper a, int shift)
            {
                return new IntWrapper(a._value << shift);
            }

            // right-shift
            public static IntWrapper operator >>(IntWrapper a, int shift)
            {
                return new IntWrapper(a._value >> shift);
            }

            // value-types equality
            public static bool operator ==(IntWrapper a, IntWrapper b)
            {
                return a?._value == b?._value;
            }

            // inequality
            public static bool operator !=(IntWrapper a, IntWrapper b)
            {
                return a?._value != b?._value;
            }

            // less than
            public static bool operator <(IntWrapper a, IntWrapper b)
            {
                return a?._value < b?._value;
            }

            // greater than
            public static bool operator >(IntWrapper a, IntWrapper b)
            {
                return a?._value > b?._value;
            }

            // less than or equal
            public static bool operator <=(IntWrapper a, IntWrapper b)
            {
                return a?._value <= b?._value;
            }

            // greater than or equal
            public static bool operator >=(IntWrapper a, IntWrapper b)
            {
                return a?._value >= b?._value;
            }

            public int this[int _] => _value;

            // user-defined conversion. for test cast
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
        [TestCase(typeof(IntWrapper), "op_UnaryPlus", "public static OperatorTest.IntWrapper operator +(OperatorTest.IntWrapper a){    return default;}")]
        [TestCase(typeof(IntWrapper), "op_UnaryNegation", "public static OperatorTest.IntWrapper operator -(OperatorTest.IntWrapper a){    return default;}")]
        [TestCase(typeof(IntWrapper), "op_LogicalNot", "public static bool operator !(OperatorTest.IntWrapper a){    return default;}")]
        [TestCase(typeof(IntWrapper), "op_OnesComplement", "public static OperatorTest.IntWrapper operator ~(OperatorTest.IntWrapper a){    return default;}")]
        [TestCase(typeof(IntWrapper), "op_Increment", "public static OperatorTest.IntWrapper operator ++(OperatorTest.IntWrapper a){    return default;}")]
        [TestCase(typeof(IntWrapper), "op_Decrement", "public static OperatorTest.IntWrapper operator --(OperatorTest.IntWrapper a){    return default;}")]
        [TestCase(typeof(IntWrapper), "op_True", "public static bool operator true (OperatorTest.IntWrapper a){    return default;}")]
        [TestCase(typeof(IntWrapper), "op_False", "public static bool operator false (OperatorTest.IntWrapper a){    return default;}")]
        [TestCase(typeof(IntWrapper), "op_Addition", "public static OperatorTest.IntWrapper operator +(OperatorTest.IntWrapper a, OperatorTest.IntWrapper b){    return default;}")]
        [TestCase(typeof(IntWrapper), "op_Subtraction", "public static OperatorTest.IntWrapper operator -(OperatorTest.IntWrapper a, OperatorTest.IntWrapper b){    return default;}")]
        [TestCase(typeof(IntWrapper), "op_Multiply", "public static OperatorTest.IntWrapper operator *(OperatorTest.IntWrapper a, OperatorTest.IntWrapper b){    return default;}")]
        [TestCase(typeof(IntWrapper), "op_Division", "public static OperatorTest.IntWrapper operator /(OperatorTest.IntWrapper a, OperatorTest.IntWrapper b){    return default;}")]
        [TestCase(typeof(IntWrapper), "op_Modulus", "public static OperatorTest.IntWrapper operator %(OperatorTest.IntWrapper a, OperatorTest.IntWrapper b){    return default;}")]
        [TestCase(typeof(IntWrapper), "op_BitwiseAnd", "public static bool operator &(OperatorTest.IntWrapper a, OperatorTest.IntWrapper b){    return default;}")]
        [TestCase(typeof(IntWrapper), "op_BitwiseOr", "public static bool operator |(OperatorTest.IntWrapper a, OperatorTest.IntWrapper b){    return default;}")]
        [TestCase(typeof(IntWrapper), "op_ExclusiveOr", "public static bool operator ^(OperatorTest.IntWrapper a, OperatorTest.IntWrapper b){    return default;}")]
        [TestCase(typeof(IntWrapper), "op_LeftShift", "public static OperatorTest.IntWrapper operator <<(OperatorTest.IntWrapper a, int shift){    return default;}")]
        [TestCase(typeof(IntWrapper), "op_RightShift", "public static OperatorTest.IntWrapper operator >>(OperatorTest.IntWrapper a, int shift){    return default;}")]
        [TestCase(typeof(IntWrapper), "op_Equality", "public static bool operator ==(OperatorTest.IntWrapper a, OperatorTest.IntWrapper b){    return default;}")]
        [TestCase(typeof(IntWrapper), "op_Inequality", "public static bool operator !=(OperatorTest.IntWrapper a, OperatorTest.IntWrapper b){    return default;}")]
        [TestCase(typeof(IntWrapper), "op_LessThan", "public static bool operator <(OperatorTest.IntWrapper a, OperatorTest.IntWrapper b){    return default;}")]
        [TestCase(typeof(IntWrapper), "op_GreaterThan", "public static bool operator>(OperatorTest.IntWrapper a, OperatorTest.IntWrapper b){    return default;}")]
        [TestCase(typeof(IntWrapper), "op_LessThanOrEqual", "public static bool operator <=(OperatorTest.IntWrapper a, OperatorTest.IntWrapper b){    return default;}")]
        [TestCase(typeof(IntWrapper), "op_GreaterThanOrEqual", "public static bool operator >=(OperatorTest.IntWrapper a, OperatorTest.IntWrapper b){    return default;}")]
        public void DeclarationToSyntax(Type cls, string @operator, string expected)
        {
            var overload = cls.GetMethod(@operator, BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly);
            Assert.AreEqual(expected, new Operator(overload).DeclarationToSyntax(true).NormalizeWhitespace().ToFullString().Replace(Environment.NewLine, ""));
        }
    }
}