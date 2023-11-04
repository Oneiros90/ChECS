using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using Object = UnityEngine.Object;

namespace Language.Generators
{
    public abstract class CodeGenerator
    {
        protected abstract IEnumerable<GeneratedScript> GetScriptsToGenerate();

        public static void GenerateScripts()
        {
            var codeGenerators = TypeCache.GetTypesDerivedFrom<CodeGenerator>()
                                          .Select(t => (CodeGenerator)Activator.CreateInstance(t));
            GenerateScripts(codeGenerators.ToArray());
        }

        public static void GenerateScripts(params CodeGenerator[] codeGenerators)
        {
            var oldGeneratedScripts = new List<GeneratedScript>();
            var newGeneratedScripts = new List<GeneratedScript>();

            foreach (var codeGenerator in codeGenerators)
            {
                oldGeneratedScripts.AddRange(GetAssets<MonoScript>()
                                             .Where(s => GeneratedScript.IsGenerated(AssetDatabase.GetAssetPath(s)))
                                             .Select(GeneratedScript.FromAsset));

                newGeneratedScripts.AddRange(codeGenerator.GetScriptsToGenerate());
            }

            foreach (var script in oldGeneratedScripts.Except(newGeneratedScripts))
            {
                script.Delete();
            }

            foreach (var script in newGeneratedScripts.Except(oldGeneratedScripts))
            {
                script.Create();
            }

            AssetDatabase.Refresh();
        }

        public static string GetGeneratedFolderForType(Type scriptType)
        {
            var assemblyName = scriptType.Assembly.GetName().Name;
            var assemblyPath = GetAssets<AssemblyDefinitionAsset>()
                               .Where(a => a.text.Contains($"\"name\": \"{assemblyName}\""))
                               .Select(AssetDatabase.GetAssetPath).FirstOrDefault();

            var assemblyParent = Path.GetDirectoryName(assemblyPath);
            return Path.Combine(assemblyParent ?? "Assets", "Generated");
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