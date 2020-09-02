using System;
using System.Collections.Generic;
using System.IO;

using Mochizuki.Fantasma.Extensions;

using NUnit.Framework;

using UnityEngine;

using Object = UnityEngine.Object;

namespace Mochizuki.Fantasma.Tests.Extensions
{
    [TestFixture]
    internal class TypeExtensionsTest
    {
        private interface IInterfaceA { }

        private interface IInterfaceB : IInterfaceA { }

        private class BaseClass { }

        // ReSharper disable once ClassNeverInstantiated.Local
        private class AClass : BaseClass, IDisposable, IInterfaceB
        {
            public delegate void DelegateA();

            // ReSharper disable once UnusedMember.Local
            // ReSharper disable once UnassignedGetOnlyAutoProperty
            public DelegateA SomeDelegate { get; }

            public void Dispose() { }

            // ReSharper disable once UnusedParameter.Local
            public void MethodA<T1, T2, T3, T4>(T1 a, in T2 b, out T3 c, ref T4 d, string e)
            {
                c = default;
            }

            public class BClass
            {
                public class CClass { }
            }

            public class DClass<T>
            {
                public class EClass
                {
                    public class LClass<T1> { }
                }
            }
        }

        private class FClass<T>
        {
            public class GClass
            {
                public class HClass<T1> { }
            }

            public class JClass<T1, T2>
            {
                public class KClass { }
            }
        }

        [Test]
        [TestCase(typeof(BinaryReader), new[] { typeof(IDisposable) })]
        [TestCase(typeof(AClass), new[] { typeof(IDisposable), typeof(IInterfaceB) })]
        [TestCase(typeof(AClass.BClass), new Type[] { })]
        public void GetDirectImplementedInterfacesTest(Type t, Type[] expected)
        {
            CollectionAssert.AreEquivalent(expected, t.GetDirectImplementedInterfaces());
        }

        [Test]
        [TestCase(typeof(AClass.DelegateA), true)]
        [TestCase(typeof(Action), true)] // System.Action is inherited from System.Delegate
        [TestCase(typeof(AClass), false)]
        public void IsDelegateTest(Type t, bool expected)
        {
            Assert.AreEqual(expected, t.IsDelegate());
        }

        [Test]
        [TestCase(typeof(AClass), "MethodA", 0, true)]
        [TestCase(typeof(AClass), "MethodA", 1, true)]
        [TestCase(typeof(AClass), "MethodA", 2, true)]
        [TestCase(typeof(AClass), "MethodA", 3, true)]
        [TestCase(typeof(AClass), "MethodA", 4, false)]
        public void IsGenericParameterTest(Type t, string method, int i, bool expected)
        {
            var m = t.GetMethod(method);

            // ReSharper disable once PossibleNullReferenceException
            Assert.AreEqual(expected, m.GetParameters()[i].ParameterType.IsGenericParameter());
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

        [Test]
        [TestCase(typeof(bool), "bool")]
        [TestCase(typeof(GameObject), "GameObject")]
        [TestCase(typeof(GameObject[]), "GameObject[]")]
        [TestCase(typeof(IEnumerable<string>), "IEnumerable<string>")]
        [TestCase(typeof(AClass), "TypeExtensionsTest.AClass")]
        [TestCase(typeof(AClass.BClass.CClass), "TypeExtensionsTest.AClass.BClass.CClass")]
        [TestCase(typeof(AClass.DClass<string>), "TypeExtensionsTest.AClass.DClass<string>")]
        [TestCase(typeof(AClass.DClass<string>.EClass), "TypeExtensionsTest.AClass.DClass<string>.EClass")]
        [TestCase(typeof(AClass.DClass<>.EClass), "TypeExtensionsTest.AClass.DClass<T>.EClass")]
        [TestCase(typeof(AClass.DClass<>.EClass.LClass<>), "TypeExtensionsTest.AClass.DClass<T>.EClass.LClass<T1>")]
        [TestCase(typeof(FClass<string>.GClass), "TypeExtensionsTest.FClass<string>.GClass")]
        [TestCase(typeof(FClass<string>.GClass[]), "TypeExtensionsTest.FClass<string>.GClass[]")]
        [TestCase(typeof(FClass<string>.GClass.HClass<int>), "TypeExtensionsTest.FClass<string>.GClass.HClass<int>")]
        [TestCase(typeof(FClass<int>.JClass<GameObject, string>.KClass), "TypeExtensionsTest.FClass<int>.JClass<GameObject, string>.KClass")]
        public void NormalizedNameTest(Type t, string expected)
        {
            Assert.AreEqual(expected, t.NormalizedName());
        }
    }
}