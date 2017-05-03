using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Semantics;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime;
using System.Runtime.Versioning;

namespace RoslynCompiler
{
    public class Program
    {
        public static Assembly Compile(params string[] sources)
        {
            var assemblyFileName = "gen" + Guid.NewGuid().ToString().Replace("-", "") + ".dll";
            var compilation = CSharpCompilation.Create(assemblyFileName,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary),
                syntaxTrees: from source in sources
                             select CSharpSyntaxTree.ParseText(source),
                references: GetReferences());

            EmitResult emitResult;

            using (var dll = new MemoryStream())
            {
                emitResult = compilation.Emit(dll, options: new EmitOptions(false, DebugInformationFormat.Embedded));

                if (emitResult.Success)
                {
                    var assembly = Assembly.Load(dll.GetBuffer());
                    return assembly;
                }
            }

            var message = string.Join("\r\n", emitResult.Diagnostics);
            throw new ApplicationException(message);
        }

        private static IEnumerable<MetadataReference> GetReferences()
        {
            var frameworkVersion = "4.6";
            var frameworkPath = $@"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v{frameworkVersion}";
            yield return MetadataReference.CreateFromFile(Path.Combine(frameworkPath, "mscorlib.dll"));
            yield return MetadataReference.CreateFromFile(Path.Combine(frameworkPath, "System.Core.dll"));
        }

        public static string simpleExample =
            @"using System;
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine(""test"");
    }
}
 ";

        public static Assembly SimpleCompilation()
        {
            return Compile(new string[] { simpleExample });
        }

        static void Main(string[] args)
        {
            EnableMultiCoreJit();

            var stopWatch = new Stopwatch();
            stopWatch.Start();
            SimpleCompilation();
            Console.Write($"Compilation time: {stopWatch.ElapsedMilliseconds} ms");
        }

        private static void EnableMultiCoreJit()
        {
            // Enable Multicore JIT (https://blogs.msdn.microsoft.com/dotnet/2012/10/18/an-easy-solution-for-improving-app-launch-performance/)
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var profileFolder = Path.Combine(appData, "RoslynCompiler", GetTargetFramework());
            Directory.CreateDirectory(profileFolder);
            ProfileOptimization.SetProfileRoot(profileFolder);
            ProfileOptimization.StartProfile("Startup.Profile");
        }

        private static string GetTargetFramework()
        {
            var targetFramework = "Unknown";
            var targetFrameworkAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(TargetFrameworkAttribute), true);
            if (targetFrameworkAttributes.Length > 0)
            {
                var targetFrameworkAttribute = (TargetFrameworkAttribute)targetFrameworkAttributes.First();
                targetFramework = targetFrameworkAttribute.FrameworkDisplayName;
            }

            return targetFramework;
        }
    }
}
