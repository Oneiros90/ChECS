using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using Object = UnityEngine.Object;

namespace CHECS
{
    public static class CodeGeneratorsTrigger
    {
        [InitializeOnLoadMethod]
        private static void OnInitialized()
        {
            AssemblyReloadEvents.afterAssemblyReload -= AfterAssemblyReload;
            AssemblyReloadEvents.afterAssemblyReload += AfterAssemblyReload;
        }

        private static void AfterAssemblyReload()
        {
            var oldGeneratedScripts = new List<GeneratedScript>();
            var newGeneratedScripts = new List<GeneratedScript>();

            foreach (var codeGeneratorType in TypeCache.GetTypesDerivedFrom<CodeGenerator>())
            {
                var codeGenerator = (CodeGenerator)Activator.CreateInstance(codeGeneratorType);

                oldGeneratedScripts.AddRange(GetAssets<MonoScript>(codeGenerator.GeneratedScriptsFolder)
                    .Select(GeneratedScript.FromAsset));

                newGeneratedScripts.AddRange(codeGenerator.GenerateScripts());
            }

            foreach (var script in oldGeneratedScripts.Except(newGeneratedScripts))
            {
                script.Delete();
            }

            foreach (var script in newGeneratedScripts.Except(oldGeneratedScripts))
            {
                script.Create();
            }
        }

        private static IEnumerable<T> GetAssets<T>(string path = "Assets") where T : Object
        {
            return GetAssetsPaths<T>(path).Select(AssetDatabase.LoadAssetAtPath<T>);
        }

        private static IEnumerable<string> GetAssetsPaths<T>(string path = "Assets") where T : Object
        {
            return GetAssetsGUIDs<T>(path).Select(AssetDatabase.GUIDToAssetPath);
        }

        private static IEnumerable<string> GetAssetsGUIDs<T>(string path = "Assets") where T : Object
        {
            return AssetDatabase.FindAssets($"t:{typeof(T).Name}", new[] { path });
        }
    }
}