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
    internal class Indexer : IMemberDeclaration<IndexerDeclarationSyntax>
    {
        private readonly PropertyInfo _indexer;
        private readonly List<Type> _references;

        public Indexer(PropertyInfo indexer)
        {
            _indexer = indexer;
            _references = new List<Type>();

            void AddReferenceIfNotExists(Type t)
            {
                if (_references.Any(w => w.FullName == t.FullName))
                    return;
                _references.Add(t);
            }

            if (_indexer.CanRead)
                _indexer.GetMethod.GetParameters().SelectMany(w => w.ParameterType.RecursiveExtract()).ToList().ForEach(AddReferenceIfNotExists);
            if (_indexer.CanWrite)
                _indexer.SetMethod.GetParameters().SelectMany(w => w.ParameterType.RecursiveExtract()).ToList().ForEach(AddReferenceIfNotExists);
        }

        public IndexerDeclarationSyntax DeclarationToSyntax(bool implementation)
        {
            var modifiers = new List<SyntaxKind> { SyntaxKind.PublicKeyword };
            var parameters = _indexer.GetIndexParameters()
                                     .Select(w => SyntaxFactory.Parameter(SyntaxFactory.Identifier(w.Name).Normalize()).WithType(SyntaxFactory.ParseTypeName(w.ParameterType.NormalizedName())));
            var accessors = new List<AccessorDeclarationSyntax>();
            if (_indexer.CanRead)
                accessors.Add(SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration, SyntaxFactory.Block(SyntaxFactory.ParseStatement("return default;"))));
            if (_indexer.CanWrite)
                accessors.Add(SyntaxFactory.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration, SyntaxFactory.Block()));

            return SyntaxFactory.IndexerDeclaration(SyntaxFactory.ParseTypeName(_indexer.PropertyType.NormalizedName()))
                                .AddModifiers(modifiers.Select(SyntaxFactory.Token).ToArray())
                                .WithParameterList(SyntaxFactory.BracketedParameterList(SyntaxFactory.SeparatedList(parameters)))
                                .WithAccessorList(SyntaxFactory.AccessorList(SyntaxFactory.List(accessors)));
        }

        public ReadOnlyCollection<Type> References => _references.AsReadOnly();

        public static bool Test(MemberInfo member)
        {
            if (member.MemberType != MemberTypes.Property)
                return false;

            var property = (PropertyInfo) member;
            if (property.CanRead && property.GetMethod.GetParameters().Length == 0)
                return false; // Getter Method must have 1 or more parameters (index, ...)
            if (property.CanWrite && property.SetMethod.GetParameters().Length == 1)
                return false; // Setter Method must have 2 or more parameters (index, value, ...)
            return true;
        }
    }
}