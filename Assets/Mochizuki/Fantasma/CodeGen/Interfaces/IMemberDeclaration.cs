using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Mochizuki.Fantasma.CodeGen.Interfaces
{
    internal interface IMemberDeclaration<out T> : ICodeGen<T> where T : MemberDeclarationSyntax { }
}