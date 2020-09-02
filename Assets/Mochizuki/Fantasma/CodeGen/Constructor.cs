using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using Mochizuki.Fantasma.CodeGen.Interfaces;
using Mochizuki.Fantasma.Extensions;

namespace Mochizuki.Fantasma.CodeGen
{
    internal class Constructor : IMemberDeclaration<ConstructorDeclarationSyntax>
    {
        private readonly ConstructorInfo _constructor;
        private readonly bool _isInternal;

        public ReadOnlyCollection<Type> ArgumentTypes => _constructor.GetParameters().Select(w => w.ParameterType).ToList().AsReadOnly();

        public Constructor(ConstructorInfo constructor, bool isInternal)
        {
            _constructor = constructor;
            _isInternal = isInternal;
        }

        public ConstructorDeclarationSyntax DeclarationToSyntax()
        {
            if (_isInternal)
            {
                var modifiers = new List<SyntaxKind> { SyntaxKind.ProtectedKeyword, SyntaxKind.InternalKeyword };

                return SyntaxFactory.ConstructorDeclaration(SyntaxFactory.ParseToken(_constructor.DeclaringType?.Name))
                                    .AddModifiers(modifiers.Select(SyntaxFactory.Token).ToArray())
                                    .WithBody(SyntaxFactory.Block());
            }
            else
            {
                var modifiers = new List<SyntaxKind> { SyntaxKind.PublicKeyword };

                var parameters = _constructor.GetParameters()
                                             .Select(w => SyntaxFactory.Parameter(SyntaxFactory.Identifier(w.Name)).WithType(SyntaxFactory.ParseTypeName(w.ParameterType.NormalizedName())))
                                             .ToList();

                return SyntaxFactory.ConstructorDeclaration(SyntaxFactory.ParseToken(_constructor.DeclaringType?.Name))
                                    .AddModifiers(modifiers.Select(SyntaxFactory.Token).ToArray())
                                    .WithParameterList(SyntaxFactory.ParameterList(SyntaxFactory.SeparatedList(parameters)))
                                    .WithBody(SyntaxFactory.Block());
            }
        }

        public ReadOnlyCollection<Type> References => ArgumentTypes;

        public Constructor CreateDefaultConstructor()
        {
            return new Constructor(_constructor, true);
        }
    }
}