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
    internal class Field : IMemberDeclaration<FieldDeclarationSyntax>
    {
        private readonly FieldInfo _field;

        public Type Type => _field.FieldType;

        public string Name => _field.Name;

        public Field(FieldInfo field)
        {
            _field = field;
        }

        public FieldDeclarationSyntax DeclarationToSyntax(bool implementation)
        {
            var variable = SyntaxFactory.VariableDeclaration(SyntaxFactory.ParseTypeName(Type.NormalizedName())).AddVariables(SyntaxFactory.VariableDeclarator(Name));
            var modifiers = new List<SyntaxKind> { SyntaxKind.PublicKeyword };
            if (_field.IsStatic && !_field.IsLiteral)
                modifiers.Add(SyntaxKind.StaticKeyword);
            if (_field.IsInitOnly)
                modifiers.Add(SyntaxKind.ReadOnlyKeyword);
            if (_field.IsLiteral)
            {
                modifiers.Add(SyntaxKind.ConstKeyword);

                var equals = SyntaxFactory.EqualsValueClause(SyntaxFactory.Token(SyntaxKind.EqualsToken), CreateInitializerSyntax());
                variable = variable.WithVariables(SyntaxFactory.SeparatedList(new List<VariableDeclaratorSyntax> { variable.Variables.First().WithInitializer(equals) }));
            }

            return SyntaxFactory.FieldDeclaration(variable).AddModifiers(modifiers.Select(SyntaxFactory.Token).ToArray());
        }

        public ReadOnlyCollection<Type> References => new ReadOnlyCollection<Type>(new List<Type> { Type });

        private ExpressionSyntax CreateInitializerSyntax()
        {
            if (Type == typeof(string))
                return SyntaxFactory.ParseExpression($"\"{_field.GetValue(null)}\"");
            if (Type.IsEnum)
                return SyntaxFactory.ParseExpression($"{Type.NormalizedName()}.{_field.GetValue(null)}");
            return SyntaxFactory.ParseExpression(_field.GetValue(null).ToString());
        }
    }
}