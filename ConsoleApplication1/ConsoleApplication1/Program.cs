using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            var solutionPath = @"C:\git\roslyn\Roslyn.sln";
            var outputDirectory = @"D:\output";

            var workspace = Microsoft.CodeAnalysis.MSBuild.MSBuildWorkspace.Create();
            Console.WriteLine("Loading solution");
            var solution = workspace.OpenSolutionAsync(solutionPath).Result;
            var project = solution.Projects.First(n => n.Name.Equals("CSharpCompilerSemanticTest"));
            Console.WriteLine("Getting compilation");
            var compilation = project.GetCompilationAsync().Result;

            Console.WriteLine("Emission");
            var assemblyOutputPath = Path.Combine(outputDirectory, project.AssemblyName + ".dll");
            var pdbOutputPath = Path.Combine(outputDirectory, project.AssemblyName + ".pdb");
            var result = compilation.Emit(assemblyOutputPath, pdbOutputPath);
            if (!result.Success)
            {
                foreach (var err in result.Diagnostics.Where(n => n.Severity >= DiagnosticSeverity.Error))
                {
                    Console.WriteLine(err);
                }
            }
            else
            {
                Console.WriteLine("Success");
            }

            Console.ReadLine();
        }
    }
}
