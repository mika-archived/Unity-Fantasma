using Microsoft.CodeAnalysis.CSharp;

namespace Mochizuki.Fantasma.CodeGen.Interfaces
{
    internal interface ICodeGen<out T> where T : CSharpSyntaxNode
    {
        T DeclarationToSyntax();
    }
}