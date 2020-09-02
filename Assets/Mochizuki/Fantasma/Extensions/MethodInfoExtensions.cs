using System.Reflection;

namespace Mochizuki.Fantasma.Extensions
{
    internal static class MethodInfoExtensions
    {
        public static bool IsOverride(this MethodInfo obj)
        {
            return obj.GetBaseDefinition().DeclaringType != obj.DeclaringType;
        }
    }
}