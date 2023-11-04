using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Language.Generators
{
    public class GeneratedScript
    {
        private const string SUFFIX = ".generated.cs";

        private const string HEADER = @"// ----------------------------------------------------------------------------
// This code has been generated. Please do not modify.
// ----------------------------------------------------------------------------

";

        public readonly string FullPath;
        public readonly string Content;

        public GeneratedScript(string path, string name, string content)
        {
            FullPath = $"{path.Replace("\\", "/").TrimEnd('/')}/{name}{SUFFIX}";
            Content = HEADER + content;
        }

        public GeneratedScript(string fullPath, string content)
        {
            FullPath = fullPath;
            Content = content;
        }

        public static GeneratedScript FromAsset(TextAsset asset)
        {
            return new GeneratedScript(AssetDatabase.GetAssetPath(asset), asset.text);
        }

        public void Create()
        {
            var projectDir = Directory.GetParent(Application.dataPath)!.FullName;
            var filePath = Path.Combine(projectDir, FullPath);
            Directory.CreateDirectory(Path.GetDirectoryName(filePath) ?? filePath);
            File.WriteAllText(filePath, Content);
        }

        public void Delete()
        {
            var projectDir = Directory.GetParent(Application.dataPath)!.FullName;
            File.Delete(Path.Combine(projectDir, FullPath));
        }

        public override string ToString()
        {
            return FullPath + "\n" + Content;
        }

        protected bool Equals(GeneratedScript other)
        {
            return FullPath == other.FullPath && Content == other.Content;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((GeneratedScript)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(FullPath, Content);
        }

        public static bool IsGenerated(string path)
        {
            return path.EndsWith(SUFFIX);
        }
    }
}