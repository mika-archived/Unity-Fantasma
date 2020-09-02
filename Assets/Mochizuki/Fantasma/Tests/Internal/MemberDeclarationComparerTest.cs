using System.Linq;

using Microsoft.CodeAnalysis.CSharp.Syntax;

using Mochizuki.Fantasma.CodeGen;
using Mochizuki.Fantasma.CodeGen.Interfaces;
using Mochizuki.Fantasma.Internal;

using NUnit.Framework;

namespace Mochizuki.Fantasma.Tests.Internal
{
    [TestFixture]
    internal class MemberDeclarationComparerTest
    {
        private delegate void ADelegate();

        private static (IMemberDeclaration<MemberDeclarationSyntax>[], IMemberDeclaration<MemberDeclarationSyntax>[]) TestCase1()
        {
            var fields = new[] { new Field(null), new Field(null) };
            var properties = new[] { new Property(null), new Property(null) };
            var constructors = new[] { new Constructor(null, false), new Constructor(null, false) };
            var methods = new[] { new MethodInterface(null), new MethodInterface(null) };
            var delegates = new[] { new Delegate(typeof(ADelegate)), new Delegate(typeof(ADelegate)) };
            var events = new[] { new Event(null), new Event(null) };

            return (
                new IMemberDeclaration<MemberDeclarationSyntax>[] { fields[0], constructors[1], events[1], methods[1], delegates[1], constructors[0], events[0], properties[1], fields[1], methods[0], delegates[0], properties[0] },
                new IMemberDeclaration<MemberDeclarationSyntax>[] { fields[0], fields[1], properties[1], properties[0], constructors[1], constructors[0], methods[1], methods[0], delegates[1], delegates[0], events[1], events[0] }
            );
        }

        [Test]
        public void CompareTest()
        {
            var (input, expected) = TestCase1();
            CollectionAssert.AreEqual(expected, input.OrderBy(w => w, new MemberDeclarationComparer()).ToArray());
        }
    }
}