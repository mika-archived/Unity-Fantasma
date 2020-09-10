using System;
using System.Collections.Generic;
using System.Reflection;

using Microsoft.CodeAnalysis;

using Mochizuki.Fantasma.CodeGen;

using NUnit.Framework;

using UnityEngine;

using Object = UnityEngine.Object;

// ReSharper disable ClassNeverInstantiated.Local

#pragma warning disable 649
#pragma warning disable RCS1102 // Make class static.

namespace Mochizuki.Fantasma.Tests.CodeGen
{
    [TestFixture]
    internal class FieldTest
    {
        private class ClassA
        {
            // ReSharper disable once UnusedMember.Local
            public const string FieldC = "Hello, World";

            // ReSharper disable once UnusedMember.Local
            public const long FieldG = long.MaxValue;

            // ReSharper disable once UnusedMember.Local
            public const DayOfWeek FieldH = DayOfWeek.Friday;

            public static long FieldD;
            public static readonly GameObject FieldE;
            public readonly List<GameObject> FieldB;
            public string FieldA;
            public ClassC<string>.ClassD FieldF;

            // ReSharper disable once UnusedTypeParameter
            public class ClassC<T>
            {
                public class ClassD
                {
                    // ReSharper disable once UnusedTypeParameter
                    public class ClassE<T1> { }
                }
            }
        }

        // ReSharper disable once UnusedTypeParameter
        private class ClassB<T>
        {
            public uint FieldA;
            public ClassA.ClassC<IEnumerable<KeyValuePair<string, object>>>.ClassD FieldB;
            public ClassA.ClassC<IEnumerable<KeyValuePair<string, Object>>>.ClassD.ClassE<List<string>> FieldC;
        }

        [Test]
        [TestCase(typeof(ClassA), "FieldA", "public string FieldA;")]
        [TestCase(typeof(ClassA), "FieldB", "public readonly List<GameObject> FieldB;")]
        [TestCase(typeof(ClassA), "FieldC", "public const string FieldC = \"Hello, World\";")]
        [TestCase(typeof(ClassA), "FieldD", "public static long FieldD;")]
        [TestCase(typeof(ClassA), "FieldE", "public static readonly GameObject FieldE;")]
        [TestCase(typeof(ClassA), "FieldF", "public FieldTest.ClassA.ClassC<string>.ClassD FieldF;")]
        [TestCase(typeof(ClassA), "FieldG", "public const long FieldG = 9223372036854775807;")]
        [TestCase(typeof(ClassA), "FieldH", "public const DayOfWeek FieldH = DayOfWeek.Friday;")]
        [TestCase(typeof(ClassB<>), "FieldA", "public uint FieldA;")]
        [TestCase(typeof(ClassB<>), "FieldB", "public FieldTest.ClassA.ClassC<IEnumerable<KeyValuePair<string, object>>>.ClassD FieldB;")]
        [TestCase(typeof(ClassB<>), "FieldC", "public FieldTest.ClassA.ClassC<IEnumerable<KeyValuePair<string, Object>>>.ClassD.ClassE<List<string>> FieldC;")]
        public void DeclarationToSyntax(Type cls, string fld, string expected)
        {
            var field = cls.GetField(fld, BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly);
            Assert.AreEqual(expected, new Field(field).DeclarationToSyntax(true).NormalizeWhitespace().ToFullString());
        }
    }
}