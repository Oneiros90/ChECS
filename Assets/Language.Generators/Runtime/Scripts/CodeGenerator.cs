using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using Object = UnityEngine.Object;

namespace Language.Generators
{
    public abstract class CodeGenerator
    {
        protected abstract IEnumerable<GeneratedScript> GetScriptsToGenerate(string assemblyPath);

        public static void GenerateScripts(string assemblyName)
        {
            var codeGenerators = TypeCache.GetTypesDerivedFrom<CodeGenerator>(assemblyName)
                                          .Select(t => (CodeGenerator)Activator.CreateInstance(t));
            GenerateScripts(assemblyName, codeGenerators.ToArray());
        }

        public static void GenerateScripts(string assemblyName, params CodeGenerator[] codeGenerators)
        {
            var oldGeneratedScripts = new List<GeneratedScript>();
            var newGeneratedScripts = new List<GeneratedScript>();
            
            foreach (var codeGenerator in codeGenerators)
            {
                oldGeneratedScripts.AddRange(GetAssets<MonoScript>(assemblyName)
                    .Select(GeneratedScript.FromAsset));

                newGeneratedScripts.AddRange(codeGenerator.GetScriptsToGenerate(assemblyName));
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