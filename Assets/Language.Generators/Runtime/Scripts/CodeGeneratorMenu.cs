using UnityEditor;

namespace Language.Generators
{
    public static class CodeGeneratorMenu
    {
        [MenuItem("Languages/Generate scripts")]
        private static void GenerateScripts()
        {
            CodeGenerator.GenerateScripts();
        }
    }
}