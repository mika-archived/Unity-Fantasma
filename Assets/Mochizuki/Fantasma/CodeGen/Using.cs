using System;

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using Mochizuki.Fantasma.CodeGen.Interfaces;

namespace Mochizuki.Fantasma.CodeGen
{
    internal class Using : ICodeGen<UsingDirectiveSyntax>
    {
        private readonly bool _isAlias;
        private readonly Type _typeReference;

        public Using(Type typeReference, bool isAlias)
        {
            _typeReference = typeReference;
            _isAlias = isAlias;
        }

        public UsingDirectiveSyntax DeclarationToSyntax()
        {
            if (_isAlias)
                return SyntaxFactory.UsingDirective(SyntaxFactory.NameEquals(_typeReference.Name), SyntaxFactory.ParseName(_typeReference.FullName));
            return SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(_typeReference.Namespace));
        }
    }
}