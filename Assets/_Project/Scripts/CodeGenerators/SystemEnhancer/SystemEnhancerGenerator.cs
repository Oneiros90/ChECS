using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Unity.Entities;
using UnityEditor;

namespace CHECS
{
    public class SystemEnhancerGenerator : CodeGenerator
    {
        private const string GEN_FOLDER = "Assets/_Project/Scripts/Generated";
        private static readonly Type _systemStateRefType = typeof(SystemState).MakeByRefType();

        private static readonly HashSet<string> _methodsToSkip =
            new() { "OnCreate", "OnUpdate", "OnDestroy", "OnCreateForCompiler" };

        private const string CLASS_TEMPLATE = @"using Unity.Burst;
using Unity.Entities;

namespace {1}
{{
    public partial struct {0}
    {{
        public static void GetDefault(out {0} system, out SystemState state)
        {{
            Get(World.DefaultGameObjectInjectionWorld, out system, out state);
        }}

        public static void Get(in World world, out {0} system, out SystemState state)
        {{
            Get(world.Unmanaged, out system, out state);
        }}
        
        [BurstCompile]
        public static void Get(in WorldUnmanaged world, out {0} system, out SystemState state)
        {{
            var handle = world.GetExistingUnmanagedSystem<{0}>();
            system = world.GetUnsafeSystemRef<{0}>(handle);
            state = world.ResolveSystemStateRef(handle);
        }}
        {2}
    }}
}}";

        private const string METHOD_TEMPLATE = @"
        public static void {0}({1})
        {{
            GetDefault(out var system, out var state);
            system.{0}(ref state, {2});
        }}
        
        public static void {0}(in World world, {1})
        {{
            Get(world, out var system, out var state);
            system.{0}(ref state, {2});
        }}
        
        [BurstCompile]
        public static void {0}(in WorldUnmanaged world, {1})
        {{
            Get(world, out var system, out var state);
            system.{0}(ref state, {2});
        }}
";

        protected override string GeneratedScriptsFolder => GEN_FOLDER;

        protected override IEnumerable<GeneratedScript> GetScriptsToGenerate()
        {
            return TypeCache.GetTypesWithAttribute<EnhanceSystemAttribute>().Select(GetScriptToGenerate);
        }

        private static GeneratedScript GetScriptToGenerate(Type type)
        {
            var scriptName = $"{type.Name}Enhance";
            var methodsString = new StringBuilder();
            foreach (var method in type
                                   .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                                   .Where(p => p.GetParameters().Any() &&
                                       p.GetParameters()[0].ParameterType == _systemStateRefType)
                                   .Where(p => !_methodsToSkip.Contains(p.Name)))
            {
                var paramsDeclaration = GetParametersSignature(method, true);
                var paramsNames = GetParametersSignature(method, false);
                var methodString = string.Format(METHOD_TEMPLATE, method.Name, paramsDeclaration, paramsNames);
                methodsString.Append(methodString);
            }

            var content = string.Format(CLASS_TEMPLATE, type.Name, type.Namespace, methodsString);
            return new GeneratedScript(GEN_FOLDER, scriptName, content);
        }

        private static string GetParametersSignature(MethodInfo method, bool withType)
        {
            var parameters = method.GetParameters().Where(p => p.ParameterType != _systemStateRefType)
                                   .Select(p => GetParameterSignature(p, withType)).ToArray();
            return parameters.Length > 0 ? string.Join(", ", parameters) : string.Empty;
        }

        private static string GetParameterSignature(ParameterInfo parameter, bool withType)
        {
            return $"{OptionalToken("in", parameter.IsIn)}{OptionalToken("out", parameter.IsOut)}" +
                $"{OptionalToken(GetTypeName(parameter), withType)}{parameter.Name}";

            static string OptionalToken(string name, bool check)
            {
                return check ? $"{name} " : string.Empty;
            }
        }

        private static string GetTypeName(ParameterInfo parameter)
        {
            var type = parameter.ParameterType;
            return type.IsByRef ? type.GetElementType()!.Name : type.Name;
        }
    }
}