using System;
using System.Collections.ObjectModel;
using System.Reflection;

using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Mochizuki.Fantasma.CodeGen.Interfaces
{
    internal interface IMemberDeclaration<out T> : ICodeGen<T> where T : MemberDeclarationSyntax
    {
        /// <summary>
        ///     all type references in this declarations
        /// </summary>
        ReadOnlyCollection<Type> References { get; }

        bool Test(MemberInfo member);
    }
}