using System;
using System.IO;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Mochizuki.Fantasma.CodeGen
{
    public class CompilationUnit
    {
        private readonly string _directive;
        private readonly string _prefix;
        private readonly Type _type;
        private readonly CompilationUnitSyntax _unit;

        public CompilationUnit(Type type, CompilationUnitSyntax unit, string directive, string prefix = "")
        {
            _type = type;
            _unit = unit;
            _directive = directive;
            _prefix = prefix;
        }

        public void Flush(string baseDir)
        {
            var path = Path.Combine(baseDir, CreateFileSystemPath());
            if (!Directory.Exists(Path.GetDirectoryName(path)))
                Directory.CreateDirectory(Path.GetDirectoryName(path) ?? throw new InvalidOperationException());

            using (var sr = new StreamWriter(path))
            {
                sr.WriteLine("// Auto Generated with Mochizuki Fantasma");
                sr.WriteLine($"// Type: {_type.FullName}");
                sr.WriteLine($"// Assembly: {_type.Assembly.FullName}");
                sr.WriteLine($"// GUID: {_type.GUID}");
                sr.WriteLine();
                if (!string.IsNullOrWhiteSpace(_directive))
                    sr.WriteLine($"#if {_directive}");
                sr.WriteLine();

                foreach (var s in _unit.NormalizeWhitespace().ToFullString().Split(new[] { Environment.NewLine }, StringSplitOptions.None))
                    sr.WriteLine(s);

                sr.WriteLine();
                if (!string.IsNullOrWhiteSpace(_directive))
                    sr.WriteLine("#endif");
            }
        }

        private string CreateFileSystemPath()
        {
            var path = string.IsNullOrWhiteSpace(_type.Namespace) ? $"{_prefix}{_type.Name}.cs" : Path.Combine(_type.Namespace.Replace(".", "/"), $"{_prefix}{_type.Name}.cs");
            return Path.Combine(_type.Assembly.GetName().Name, path);
        }
    }
}