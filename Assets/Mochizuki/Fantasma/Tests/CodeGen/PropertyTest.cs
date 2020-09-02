using System;
using System.Reflection;

using Microsoft.CodeAnalysis;

using Mochizuki.Fantasma.CodeGen;

using NUnit.Framework;

using UnityEngine;

#pragma warning disable RCS1102 // Make class static.

namespace Mochizuki.Fantasma.Tests.CodeGen
{
    [TestFixture]
    internal class PropertyTest
    {
        private class AClass<T>
        {
            // ReSharper disable once ClassNeverInstantiated.Local
            public class BClass
            {
                public class CClass { }
            }

            // ReSharper disable UnusedMember.Local
            // ReSharper disable UnassignedGetOnlyAutoProperty

            public int PropertyA { get; }

            public GameObject PropertyB { get; private set; }

            public int PropertyC { private get; set; }

            public T PropertyD { get; set; }

            #region PropertyE

            private string _propertyE;

            public string PropertyE
            {
                get => _propertyE;
                set
                {
                    if (string.Equals(_propertyE, value, StringComparison.Ordinal))
                        return;
                    _propertyE = value;
                }
            }

            #endregion

            // ReSharper disable once StaticMemberInGenericType
            public static double PropertyF { get; private set; }

            public BClass PropertyG { get; set; }

            public BClass.CClass PropertyH { get; set; }

            // ReSharper restore UnusedMember.Local
            // ReSharper restore UnassignedGetOnlyAutoProperty
        }

        [Test]
        [TestCase(typeof(AClass<>), 0, true, "public int PropertyA{get;}")]
        [TestCase(typeof(AClass<>), 1, true, "public GameObject PropertyB{get;}")]
        [TestCase(typeof(AClass<>), 2, true, "public int PropertyC{set;}")]
        [TestCase(typeof(AClass<>), 3, true, "public T PropertyD{get;set;}")]
        [TestCase(typeof(AClass<>), 4, true, "public string PropertyE{get;set;}")]
        [TestCase(typeof(AClass<>), 5, true, "public static double PropertyF{get;}")]
        [TestCase(typeof(AClass<>), 6, true, "public PropertyTest.AClass<T>.BClass PropertyG{get;set;}")]
        [TestCase(typeof(AClass<>), 7, true, "public PropertyTest.AClass<T>.BClass.CClass PropertyH{get;set;}")]
        [TestCase(typeof(AClass<>), 1, false, "GameObject PropertyB{get;}")]
        public void DeclarationToSyntax(Type cls, int idx, bool implementation, string expected)
        {
            var property = cls.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly)[idx];
            Assert.AreEqual(expected, new Property(property).DeclarationToSyntax(implementation).NormalizeWhitespace().ToFullString().Replace(Environment.NewLine, "").Replace("    ", ""), expected);
        }
    }
}