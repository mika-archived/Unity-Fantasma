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

        public bool IsDefaultConstructor => _constructor.GetParameters().Length == 0 && _constructor.IsPublic;

        public Constructor(ConstructorInfo constructor, bool isInternal)
        {
            _constructor = constructor;
            _isInternal = isInternal;
        }

        public ConstructorDeclarationSyntax DeclarationToSyntax(bool implementation)
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

        public bool Test(MemberInfo member)
        {
            return member.MemberType == MemberTypes.Constructor;
        }

        public Constructor CreateDefaultConstructor()
        {
            return new Constructor(_constructor, true);
        }
    }
}