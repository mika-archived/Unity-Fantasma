using Mochizuki.Fantasma.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mochizuki.Fantasma.Extensions
{
    internal static class TypeExtensions
    {
        private static readonly List<string> DenyInterfaces = new List<string> { "_Attribute" };

        public static bool IsKeywordType(this Type type)
        {
            return type.FullNameWithoutNamespace() != type.KeywordNormalizedName();
        }

        public static bool IsDelegate(this Type t)
        {
            return t.IsSubclassOf(typeof(Delegate)) || t == typeof(Delegate);
        }

        public static string FullNameWithoutNamespace(this Type t)
        {
            return t.DeclaringType == null ? t.Name : $"{FullNameWithoutNamespace(t.DeclaringType)}.{t.Name}";
        }

        [NoTest]
        public static string NormalizedName(this Type t)
        {
            if (t.IsKeywordType())
                return t.KeywordNormalizedName();
            if (t.IsGenericParameter)
                return t.Name;

            var sb = new StringBuilder();
            if (t.IsGenericType)
            {
                sb.Append(t.FullNameWithoutNamespace(), 0, t.FullNameWithoutNamespace().IndexOf('`'))
                  .Append("<");

                foreach (var (type, index) in t.GenericTypeArguments.Select((w, i) => (Type: w, Index: i)))
                {
                    if (index > 0)
                        sb.Append(", ");
                    sb.Append(type.NormalizedName());
                }

                sb.Append(">");
            }
            else if (t.IsArray)
            {
                sb.Append(t.GetElementType().NormalizedName());
                sb.Append("[]");
            }
            else
            {
                sb.Append(t.FullNameWithoutNamespace());
            }

            return sb.ToString();
        }

        [NoTest]
        public static string NormalizedFullName(this Type t)
        {
            var @namespace = t.Namespace;

            return $"{@namespace}.{t.NormalizedName()}";
        }

        public static string KeywordNormalizedName(this Type type)
        {
            switch (type)
            {
                // ReSharper disable PatternAlwaysOfType

                case Type _ when type == typeof(bool):
                    return "bool";

                case Type _ when type == typeof(byte):
                    return "byte";

                case Type _ when type == typeof(sbyte):
                    return "sbyte";

                case Type _ when type == typeof(char):
                    return "char";

                case Type _ when type == typeof(decimal):
                    return "decimal";

                case Type _ when type == typeof(double):
                    return "double";

                case Type _ when type == typeof(float):
                    return "float";

                case Type _ when type == typeof(int):
                    return "int";

                case Type _ when type == typeof(uint):
                    return "uint";

                case Type _ when type == typeof(long):
                    return "long";

                case Type _ when type == typeof(ulong):
                    return "ulong";

                case Type _ when type == typeof(short):
                    return "short";

                case Type _ when type == typeof(ushort):
                    return "ushort";

                case Type _ when type == typeof(object):
                    return "object";

                case Type _ when type == typeof(string):
                    return "string";

                case Type _ when type == typeof(void):
                    return "void";

                default:
                    return type.FullNameWithoutNamespace();

                // ReSharper restore PatternAlwaysOfType
            }
        }

        public static Type[] GetDirectImplementedInterfaces(this Type t)
        {
            if (!t.IsClass)
                return Array.Empty<Type>();

            var interfaces = new HashSet<Type>(t.GetInterfaces());
            var removals = new HashSet<Type>();

            foreach (var @interface in interfaces)
            foreach (var i in @interface.GetInterfaces())
                removals.Add(i);

            interfaces.ExceptWith(removals);

            return interfaces.Where(w => DenyInterfaces.Exists(v => w.Name != v)).ToArray();
        }

        public static bool IsGenericParameter(this Type t)
        {
            if (t.IsGenericParameter)
                return true;

            if (t.IsByRef)
                return t.GetElementType()?.IsGenericParameter ?? false;

            return false;
        }
    }
}