using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Mochizuki.Fantasma.Extensions
{
    internal static class SyntaxTokenExtensions
    {
        public static SyntaxToken Normalize(this SyntaxToken token)
        {
            var keywords = SyntaxFacts.GetKeywordKinds().Select(SyntaxFactory.Token).Select(w => w.ToFullString());
            return keywords.Contains(token.ToFullString()) ? SyntaxFactory.ParseToken($"@{token.ToFullString()}") : token;
        }
    }
}