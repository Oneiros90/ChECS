using UnityEditor;

namespace Language.Generators
{
    public static class CodeGeneratorsTrigger
    {
        [InitializeOnLoadMethod]
        private static void OnInitialized()
        {
            AssemblyReloadEvents.afterAssemblyReload -= OnAfterAssemblyReload;
            AssemblyReloadEvents.afterAssemblyReload += OnAfterAssemblyReload;
        }

        private static void OnAfterAssemblyReload()
        {
            CodeGenerator.GenerateScripts();
        }
    }
}