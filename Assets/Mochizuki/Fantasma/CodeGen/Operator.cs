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
    internal class Operator : IMemberDeclaration<OperatorDeclarationSyntax>
    {
        private static readonly ReadOnlyDictionary<string, SyntaxKind> MethodOperatorMap = new ReadOnlyDictionary<string, SyntaxKind>(new Dictionary<string, SyntaxKind>
        {
            { "op_UnaryPlus", SyntaxKind.PlusToken },
            { "op_UnaryNegation", SyntaxKind.MinusToken },
            { "op_LogicalNot", SyntaxKind.ExclamationToken },
            { "op_OnesComplement", SyntaxKind.TildeToken },
            { "op_Increment", SyntaxKind.PlusPlusToken },
            { "op_Decrement", SyntaxKind.MinusMinusToken },
            { "op_True", SyntaxKind.TrueKeyword },
            { "op_False", SyntaxKind.FalseKeyword },
            { "op_Addition", SyntaxKind.PlusToken },
            { "op_Subtraction", SyntaxKind.MinusToken },
            { "op_Multiply", SyntaxKind.AsteriskToken },
            { "op_Division", SyntaxKind.SlashToken },
            { "op_Modulus", SyntaxKind.PercentToken },
            { "op_BitwiseAnd", SyntaxKind.AmpersandToken },
            { "op_BitwiseOr", SyntaxKind.BarToken },
            { "op_ExclusiveOr", SyntaxKind.CaretToken },
            { "op_LeftShift", SyntaxKind.LessThanLessThanToken },
            { "op_RightShift", SyntaxKind.GreaterThanGreaterThanToken },
            { "op_Equality", SyntaxKind.EqualsEqualsToken },
            { "op_Inequality", SyntaxKind.ExclamationEqualsToken },
            { "op_LessThan", SyntaxKind.LessThanToken },
            { "op_GreaterThan", SyntaxKind.GreaterThanToken },
            { "op_LessThanOrEqual", SyntaxKind.LessThanEqualsToken },
            { "op_GreaterThanOrEqual", SyntaxKind.GreaterThanEqualsToken },
            { "op_Implicit", SyntaxKind.ImplicitKeyword },
            { "op_Explicit", SyntaxKind.ExplicitKeyword }
        });

        private readonly MethodInfo _overload;

        public Operator(MethodInfo overload)
        {
            _overload = overload;
        }

        public OperatorDeclarationSyntax DeclarationToSyntax(bool implementation)
        {
            var modifiers = new List<SyntaxKind> { SyntaxKind.PublicKeyword, SyntaxKind.StaticKeyword };
            var statements = SyntaxFactory.Block(SyntaxFactory.ParseStatement("return default;"));
            var parameters = _overload.GetParameters().Select(w => SyntaxFactory.Parameter(SyntaxFactory.Identifier(w.Name)).WithType(SyntaxFactory.ParseTypeName(w.ParameterType.NormalizedName())));

            return SyntaxFactory.OperatorDeclaration(SyntaxFactory.ParseTypeName(_overload.ReturnType.NormalizedName()), SyntaxFactory.Token(MethodOperatorMap[_overload.Name]))
                                .AddModifiers(modifiers.Select(SyntaxFactory.Token).ToArray())
                                .AddParameterListParameters(parameters.ToArray())
                                .WithBody(statements);
        }

        public ReadOnlyCollection<Type> References { get; }
    }
}