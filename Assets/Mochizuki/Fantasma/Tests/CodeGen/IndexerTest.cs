using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Microsoft.CodeAnalysis;

using Mochizuki.Fantasma.CodeGen;

using NUnit.Framework;

namespace Mochizuki.Fantasma.Tests.CodeGen
{
    [TestFixture]
    internal class IndexerTest
    {
        private class ListWrapper
        {
            private readonly List<string> _items;

            public string this[int index]
            {
                get => _items[index];
                set => _items[index] = value;
            }

            public string this[string str] => _items[int.Parse(str)];

            public string this[int index, string @default] => index >= _items.Count ? @default : _items[index];

            public ListWrapper()
            {
                _items = new List<string>();
            }
        }

        private const string TestCase1 = @"
public string this[int index]
{
    get
    {
        return default;
    }

    set
    {
    }
}
";

        private const string TestCase2 = @"
public string this[string str]
{
    get
    {
        return default;
    }
}
";

        private const string TestCase3 = @"
public string this[int index, string @default]
{
    get
    {
        return default;
    }
}
";

        [Test]
        [TestCase(typeof(ListWrapper), 0, TestCase1)]
        [TestCase(typeof(ListWrapper), 1, TestCase2)]
        [TestCase(typeof(ListWrapper), 2, TestCase3)]
        public void DeclarationToSyntax(Type cls, int index, string expected)
        {
            var members = cls.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly).Where(w => w.Name == "Item").ToList();
            Assert.AreEqual(new Indexer(members[index]).DeclarationToSyntax(true).NormalizeWhitespace().ToFullString(), expected.Trim());
        }
    }
}