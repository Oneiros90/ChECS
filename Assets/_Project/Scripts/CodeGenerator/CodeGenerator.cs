using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace CHECS
{
    public abstract class CodeGenerator
    {
        protected abstract string GeneratedScriptsFolder { get; }
        protected abstract IEnumerable<GeneratedScript> GetScriptsToGenerate(); 
        
        public static void GenerateScripts(params CodeGenerator[] codeGenerators)
        {
            var oldGeneratedScripts = new List<GeneratedScript>();
            var newGeneratedScripts = new List<GeneratedScript>();

            foreach (var codeGenerator in codeGenerators)
            {
                oldGeneratedScripts.AddRange(GetAssets<MonoScript>(codeGenerator.GeneratedScriptsFolder)
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