using System;
using System.Reflection;

using Microsoft.CodeAnalysis;

using Mochizuki.Fantasma.CodeGen;

using NUnit.Framework;

using UnityEngine;

namespace Mochizuki.Fantasma.Tests.CodeGen
{
    [TestFixture]
    internal class PropertyTest
    {
        private class AClass<T>
        {
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

            // ReSharper restore UnusedMember.Local
            // ReSharper restore UnassignedGetOnlyAutoProperty
        }

        [Test]
        [TestCase(typeof(AClass<>), 0, "public int PropertyA{get;}")]
        [TestCase(typeof(AClass<>), 1, "public GameObject PropertyB{get;}")]
        [TestCase(typeof(AClass<>), 2, "public int PropertyC{set;}")]
        [TestCase(typeof(AClass<>), 3, "public T PropertyD{get;set;}")]
        [TestCase(typeof(AClass<>), 4, "public string PropertyE{get;set;}")]
        [TestCase(typeof(AClass<>), 5, "public static double PropertyF{get;}")]
        public void DeclarationToSyntax(Type cls, int idx, string expected)
        {
            var property = cls.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly)[idx];
            Assert.AreEqual(expected, new Property(property).DeclarationToSyntax().NormalizeWhitespace().ToFullString().Replace(Environment.NewLine, "").Replace("    ", ""), expected);
        }
    }
}