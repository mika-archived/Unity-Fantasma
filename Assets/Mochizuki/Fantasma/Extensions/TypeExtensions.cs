using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Mochizuki.Fantasma.Extensions
{
    internal static class TypeExtensions
    {
        private static readonly List<string> DenyInterfaces = new List<string> { "_Attribute" };

        private static readonly List<KeyValuePair<Type, string>> KeywordTypes = new List<KeyValuePair<Type, string>>
        {
            new KeyValuePair<Type, string>(typeof(bool), "bool"),
            new KeyValuePair<Type, string>(typeof(byte), "byte"),
            new KeyValuePair<Type, string>(typeof(sbyte), "sbyte"),
            new KeyValuePair<Type, string>(typeof(char), "char"),
            new KeyValuePair<Type, string>(typeof(decimal), "decimal"),
            new KeyValuePair<Type, string>(typeof(double), "double"),
            new KeyValuePair<Type, string>(typeof(float), "float"),
            new KeyValuePair<Type, string>(typeof(int), "int"),
            new KeyValuePair<Type, string>(typeof(uint), "uint"),
            new KeyValuePair<Type, string>(typeof(long), "long"),
            new KeyValuePair<Type, string>(typeof(ulong), "ulong"),
            new KeyValuePair<Type, string>(typeof(short), "short"),
            new KeyValuePair<Type, string>(typeof(ushort), "ushort"),
            new KeyValuePair<Type, string>(typeof(object), "object"),
            new KeyValuePair<Type, string>(typeof(string), "string"),
            new KeyValuePair<Type, string>(typeof(void), "void")
        };

        public static bool IsKeywordType(this Type t)
        {
            return KeywordTypes.Any(w => w.Key == t);
        }

        public static bool IsDelegate(this Type t)
        {
            return t.IsSubclassOf(typeof(Delegate)) || t == typeof(Delegate);
        }

        public static string NormalizedName(this Type t, bool isRecursiveCall = false)
        {
            if (t.IsKeywordType())
                return t.KeywordNormalizedName();
            if (t.IsGenericParameter)
                return t.Name;
            if (t.IsPointer && !string.IsNullOrWhiteSpace(t.FullName))
            {
                var underling = Type.GetType(t.FullName.Substring(0, t.FullName.Length - 1));
                if (underling.IsKeywordType())
                    return $"{underling.KeywordNormalizedName()}*";
            }

            var sb = new StringBuilder();
            if (t.DeclaringType != null)
                sb.Append(t.DeclaringType.NormalizedName(true)).Append(".");

            if (t.IsGenericType)
            {
                // NOTE: in the case of the nested types, the type parameter is given to the last type.
                //       therefore, it does not replace the type placeholder in this section, but is replaced by a later process.

                // Has Generic Type Arguments
                if (t.GenericTypeArguments.Length > 0)
                {
                    if (t.Name.Contains("`"))
                    {
                        var inherit = t.DeclaringType != null ? InheritedGenericTypeParametersCount(t.DeclaringType) : 0;
                        if (inherit > 0)
                            for (var j = 0; j < inherit; j++)
                            {
                                var g = t.GetGenericArguments()[j];
                                var i = sb.ToString().IndexOf("`1", StringComparison.Ordinal);
                                sb.Replace("`1", g.NormalizedName(true), i, "`1".Length);
                            }

                        sb.Append(t.Name, 0, t.Name.IndexOf("`", StringComparison.Ordinal)).Append("<");

                        foreach (var (g, i) in t.GetGenericArguments().Skip(inherit).Select((w, i) => (w, i)))
                        {
                            if (i > 0)
                                sb.Append(", ");
                            sb.Append(g.NormalizedName(true));
                        }

                        sb.Append(">");
                    }
                    else
                    {
                        foreach (var g in t.GetGenericArguments())
                        {
                            var i = sb.ToString().IndexOf("`1", StringComparison.Ordinal);
                            sb.Replace("`1", g.NormalizedName(true), i, "`1".Length);
                        }

                        sb.Append(t.Name);
                    }
                }
                else
                {
                    if (t.Name.Contains("`"))
                    {
                        sb.Append(t.Name, 0, t.Name.IndexOf("`", StringComparison.Ordinal)).Append("<");

                        var inherit = t.DeclaringType != null ? InheritedGenericTypeParametersCount(t.DeclaringType) : 0;
                        var own = t.GetGenericTypeDefinition().GetTypeInfo().GenericTypeParameters.Length;

                        for (var i = 0; i < own - inherit; i++)
                        {
                            if (i > 0)
                                sb.Append(", ");
                            sb.Append("`1");
                        }

                        sb.Append(">");
                    }
                    else
                    {
                        sb.Append(t.Name);
                    }
                }
            }
            else if (t.IsArray)
            {
                sb.Append(t.GetElementType().NormalizedName(true));
                sb.Append("[]");
            }
            else
            {
                sb.Append(t.Name);
            }

            if (!isRecursiveCall && t.IsGenericType && t.GetGenericTypeDefinition().GetTypeInfo().GenericTypeParameters.Length == CountOfStringInString(sb.ToString(), "`1"))
                foreach (var g in t.GetGenericTypeDefinition().GetTypeInfo().GenericTypeParameters)
                {
                    var i = sb.ToString().IndexOf("`1", StringComparison.Ordinal);
                    sb.Replace("`1", g.NormalizedName(true), i, "`1".Length);
                }

            return sb.ToString();
        }

        public static string KeywordNormalizedName(this Type t)
        {
            return KeywordTypes.Find(w => w.Key == t).Value ?? t.NormalizedName();
        }

        public static Type[] GetDirectImplementedInterfaces(this Type t)
        {
            if (!t.IsClass)
                return Array.Empty<Type>();

            var interfaces = new HashSet<Type>(t.GetInterfaces());
            var removals = new HashSet<Type>();

            foreach (var i in interfaces.SelectMany(@interface => @interface.GetInterfaces()))
                removals.Add(i);

            interfaces.ExceptWith(removals);

            return interfaces.Where(w => DenyInterfaces.Exists(v => w.Name != v)).ToArray();
        }

        public static IEnumerable<Type> RecursiveExtract(this Type t)
        {
            var types = new List<Type>();

            void RecursiveExtractType(Type t1)
            {
                if (t1.IsGenericType)
                    t1.GenericTypeArguments.ToList().ForEach(RecursiveExtractType);

                if (t1.IsKeywordType())
                    return;

                types.Add(t1);
            }

            RecursiveExtractType(t);

            return types.Distinct().ToList();
        }

        public static bool IsGenericParameter(this Type t)
        {
            if (t.IsGenericParameter)
                return true;

            if (t.IsByRef)
                return t.GetElementType()?.IsGenericParameter ?? false;

            return false;
        }

        private static int InheritedGenericTypeParametersCount(Type t)
        {
            return t.IsGenericType ? t.GetGenericTypeDefinition().GetTypeInfo().GenericTypeParameters.Length : 0;
        }

        private static int CountOfStringInString(string str, string term)
        {
            var i = 0;
            var l = 0;

            while (str.IndexOf(term, l, StringComparison.Ordinal) >= 0)
            {
                i++;
                l = str.IndexOf(term, l, StringComparison.Ordinal) + term.Length;
            }

            return i;
        }
    }
}