using System;
using System.Collections.ObjectModel;
using System.Reflection;

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using Mochizuki.Fantasma.CodeGen.Interfaces;

namespace Mochizuki.Fantasma.CodeGen
{
    internal class Namespace : IMemberDeclaration<NamespaceDeclarationSyntax>
    {
        private readonly Type _type;

        public bool HasNamespace => !string.IsNullOrEmpty(_type.Namespace);

        public Namespace(Type type)
        {
            _type = type;
        }

        public NamespaceDeclarationSyntax DeclarationToSyntax(bool implementation)
        {
            return SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(_type.Namespace));
        }

        public ReadOnlyCollection<Type> References => new ReadOnlyCollection<Type>(Array.Empty<Type>());

        public bool Test(MemberInfo member)
        {
            return true;
        }
    }
}