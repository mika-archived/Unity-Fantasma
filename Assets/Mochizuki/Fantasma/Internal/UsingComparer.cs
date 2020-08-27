using System;
using System.Collections.Generic;

namespace Mochizuki.Fantasma.Internal
{
    internal class UsingComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            if (string.IsNullOrWhiteSpace(x))
                return 1;
            if (string.IsNullOrWhiteSpace(y))
                return 1;

            if (x.StartsWith("System") && y.StartsWith("System"))
                return string.Compare(x, y, StringComparison.Ordinal);
            if (x.StartsWith("System") && !y.StartsWith("System"))
                return -1;
            if (!x.StartsWith("System") && y.StartsWith("System"))
                return 1;
            return string.Compare(x, y, StringComparison.Ordinal);
        }
    }
}