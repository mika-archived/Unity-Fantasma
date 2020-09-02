using System;

using Mochizuki.Fantasma.Extensions;

using NUnit.Framework;

namespace Mochizuki.Fantasma.Tests.Extensions
{
    [TestFixture]
    internal class MethodInfoExtensionsTest
    {
        public interface IInterfaceA
        {
            void MethodA();
        }

        public class InterfaceImplClassA : IInterfaceA
        {
            public void MethodA() { }
        }

        public abstract class BaseClassA
        {
            public abstract void MethodA();
        }

        public class AbstractImplClassA : BaseClassA
        {
            public override void MethodA() { }
        }

        public class ClassA
        {
            public virtual void MethodA() { }
        }

        public class InheritClassA : ClassA
        {
            public override void MethodA() { }
        }

        [Test]
        [TestCase(typeof(InterfaceImplClassA), "MethodA", false)]
        [TestCase(typeof(AbstractImplClassA), "MethodA", true)]
        [TestCase(typeof(ClassA), "MethodA", false)]
        [TestCase(typeof(InheritClassA), "MethodA", true)]
        public void IsOverrideTest(Type t, string m, bool expected)
        {
            var method = t.GetMethod(m);
            Assert.AreEqual(expected, method.IsOverride());
        }
    }
}