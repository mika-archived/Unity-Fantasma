using System;
using System.Collections.ObjectModel;

using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Mochizuki.Fantasma.CodeGen.Interfaces
{
    internal interface ITypeDeclaration<out T> : IMemberDeclaration<T> where T : BaseTypeDeclarationSyntax
    {
        /// <summary>
        ///     the type declaration has nested class?
        /// </summary>
        bool HasNestedClass { get; }

        /// <summary>
        ///     direct nested types in this declaration
        /// </summary>
        ReadOnlyCollection<Type> NestedClasses { get; }

        /// <summary>
        ///     all type references in this declarations
        /// </summary>
        ReadOnlyCollection<Type> References { get; }

        /// <summary>
        ///     all alias type references in this declarations
        /// </summary>
        ReadOnlyCollection<Type> AliasReferences { get; }

        /// <summary>
        ///     add nested declarations to this declaration
        /// </summary>
        void AddDeclarations(params ITypeDeclaration<BaseTypeDeclarationSyntax>[] declarations);

        /// <summary>
        ///     extract class fields, methods, properties, references and base classes.
        /// </summary>
        void Extract();
    }
}