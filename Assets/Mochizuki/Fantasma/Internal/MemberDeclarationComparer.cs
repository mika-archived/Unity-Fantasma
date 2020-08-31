using System.Collections.Generic;

using Microsoft.CodeAnalysis.CSharp.Syntax;

using Mochizuki.Fantasma.CodeGen;
using Mochizuki.Fantasma.CodeGen.Interfaces;

namespace Mochizuki.Fantasma.Internal
{
    internal class MemberDeclarationComparer : IComparer<IMemberDeclaration<MemberDeclarationSyntax>>
    {
        public int Compare(IMemberDeclaration<MemberDeclarationSyntax> x, IMemberDeclaration<MemberDeclarationSyntax> y)
        {
            if (x == null || y == null)
                return 1;

            return OrderBy<Field>(x, y) ?? OrderBy<Property>(x, y) ?? OrderBy<Constructor>(x, y) ?? OrderBy<MethodInterface>(x, y) ?? OrderBy<Delegate>(x, y) ?? OrderBy<Event>(x, y) ?? 0;
        }

        private static int? OrderBy<T>(IMemberDeclaration<MemberDeclarationSyntax> x, IMemberDeclaration<MemberDeclarationSyntax> y) where T : IMemberDeclaration<MemberDeclarationSyntax>
        {
            if (x is T && y is T)
                return 0;
            if (x is T)
                return -1;
            if (y is T)
                return 1;
            return null;
        }
    }
}