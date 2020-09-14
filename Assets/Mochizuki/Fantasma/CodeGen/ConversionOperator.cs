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
    internal class ConversionOperator : IMemberDeclaration<ConversionOperatorDeclarationSyntax>
    {
        private static readonly ReadOnlyDictionary<string, SyntaxKind> MethodOperatorMap = new ReadOnlyDictionary<string, SyntaxKind>(new Dictionary<string, SyntaxKind>
        {
            { "op_Implicit", SyntaxKind.ImplicitKeyword },
            { "op_Explicit", SyntaxKind.ExplicitKeyword }
        });

        private readonly MethodInfo _overload;
        private readonly List<Type> _references;

        public ConversionOperator(MethodInfo overload)
        {
            _overload = overload;
            _references = new List<Type> { _overload.ReturnType };
            _references.AddRange(_overload.GetParameters().Select(w => w.ParameterType));
        }

        public ConversionOperatorDeclarationSyntax DeclarationToSyntax(bool implementation)
        {
            var modifiers = new List<SyntaxKind> { SyntaxKind.PublicKeyword, SyntaxKind.StaticKeyword };
            var statements = SyntaxFactory.Block(SyntaxFactory.ParseStatement("return default;"));
            var parameters = _overload.GetParameters().Select(w => SyntaxFactory.Parameter(SyntaxFactory.Identifier(w.Name)).WithType(SyntaxFactory.ParseTypeName(w.ParameterType.NormalizedName())));

            return SyntaxFactory.ConversionOperatorDeclaration(SyntaxFactory.Token(MethodOperatorMap[_overload.Name]), SyntaxFactory.ParseTypeName(_overload.ReturnType.NormalizedName()))
                                .AddModifiers(modifiers.Select(SyntaxFactory.Token).ToArray())
                                .AddParameterListParameters(parameters.ToArray())
                                .WithBody(statements);
        }

        public ReadOnlyCollection<Type> References => _references.AsReadOnly();

        public static bool Test(MemberInfo member)
        {
            return member.MemberType == MemberTypes.Method && ((MethodInfo) member).IsSpecialName && MethodOperatorMap.ContainsKey(member.Name);
        }
    }
}