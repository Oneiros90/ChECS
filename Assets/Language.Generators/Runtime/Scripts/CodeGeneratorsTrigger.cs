using System.IO;
using UnityEditor;
using UnityEditor.Compilation;

namespace Language.Generators
{
    public static class CodeGeneratorsTrigger
    {
        [InitializeOnLoadMethod]
        private static void OnInitialized()
        {
            CompilationPipeline.assemblyCompilationFinished -= OnAssemblyCompilationFinished;
            CompilationPipeline.assemblyCompilationFinished += OnAssemblyCompilationFinished;
        }

        private static void OnAssemblyCompilationFinished(string assemblyPath, CompilerMessage[] messages)
        {
            var assemblyName = Path.GetFileNameWithoutExtension(assemblyPath);
            CodeGenerator.GenerateScripts(assemblyName);
        }
    }
}