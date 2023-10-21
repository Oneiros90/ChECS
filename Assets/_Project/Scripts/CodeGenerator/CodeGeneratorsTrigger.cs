using System;
using System.Linq;
using UnityEditor;

namespace CHECS
{
    public static class CodeGeneratorsTrigger
    {
        [InitializeOnLoadMethod]
        private static void OnInitialized()
        {
            AssemblyReloadEvents.beforeAssemblyReload -= GenerateScripts;
            AssemblyReloadEvents.beforeAssemblyReload += GenerateScripts;
        }

        private static void GenerateScripts()
        {
            var codeGenerators = TypeCache.GetTypesDerivedFrom<CodeGenerator>()
                                          .Select(t => (CodeGenerator)Activator.CreateInstance(t));
            CodeGenerator.GenerateScripts(codeGenerators.ToArray());
            AssetDatabase.Refresh();
            AssemblyReloadEvents.afterAssemblyReload -= GenerateScripts;
            AssemblyReloadEvents.afterAssemblyReload += GenerateScripts;
        }
    }
}