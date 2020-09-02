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
    internal class Property : IMemberDeclaration<PropertyDeclarationSyntax>
    {
        private readonly PropertyInfo _property;

        public Type Type => _property.PropertyType;

        public Property(PropertyInfo property)
        {
            _property = property;
        }

        public PropertyDeclarationSyntax DeclarationToSyntax()
        {
            var modifiers = new List<SyntaxKind> { SyntaxKind.PublicKeyword };
            if (_property.GetAccessors().Any(w => w.IsStatic))
                modifiers.Add(SyntaxKind.StaticKeyword);

            var property = SyntaxFactory.PropertyDeclaration(SyntaxFactory.ParseTypeName(Type.NormalizedName()), _property.Name).AddModifiers(modifiers.Select(SyntaxFactory.Token).ToArray());
            var accessors = new List<AccessorDeclarationSyntax>();
            if (_property.CanRead && _property.GetMethod.IsPublic)
                accessors.Add(SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration));
            if (_property.CanWrite && _property.SetMethod.IsPublic)
                accessors.Add(SyntaxFactory.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration));

            return property.AddAccessorListAccessors(accessors.Select(w => w.WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken))).ToArray());
        }

        public ReadOnlyCollection<Type> References => new ReadOnlyCollection<Type>(new List<Type> { Type });
    }
}