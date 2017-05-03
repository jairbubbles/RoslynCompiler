using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comparer
{
    class Program
    {
        static void Main(string[] args)
        {
            var frameworks = new[] { "4.6", "4.6.1", "4.6.2", "4.7" };
            foreach (var framework in frameworks)
            {
#if DEBUG
                var exePath = Path.Combine(Environment.CurrentDirectory, $@"../../../RoslynCompiler_{framework}\bin\Debug\RoslynCompiler.exe");
#else
                var exePath = Path.Combine(Environment.CurrentDirectory, $@"../../../RoslynCompiler_{framework}\bin\Release\RoslynCompiler.exe");
#endif

                if (!File.Exists(exePath))
                {
                    Console.WriteLine("RoslynCompiler.exe not found for that framework!");
                    continue;
                }

                Console.WriteLine($"== Run Roslyn Compiler with .NET Framework {framework} ==");

                Console.WriteLine(" Creating startup profile:");
                LaunchCompiler(exePath, true);
                LaunchCompiler(exePath, true);
                LaunchCompiler(exePath, true);

                Console.WriteLine(" Use startup profile:");
                LaunchCompiler(exePath);
                LaunchCompiler(exePath);
                LaunchCompiler(exePath);

                Console.WriteLine("");
            }
        }

        private static void LaunchCompiler(string exePath, bool deleteStartupProfile = false)
        {
            if (deleteStartupProfile)
            {
                var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var profileFolder = Path.Combine(appData, "RoslynCompiler");
                if (Directory.Exists(profileFolder))
                {
                    Directory.Delete(profileFolder, true);
                }
            }

            var w = new Stopwatch();
            w.Start();

            Console.Write("  ");

            Process.Start(new ProcessStartInfo()
            {
                FileName = exePath,
                UseShellExecute = false,
            }).WaitForExit();

            Console.WriteLine($"  Execution time: {w.ElapsedMilliseconds} ms");
        }
    }
}
