using System;
using System.Collections.Generic;
using System.Linq;
using Language.Generators;
using UnityEditor;

namespace DOTS.Generators
{
    public class OneFrameOnlyComponentsSystemCleanersGenerator : CodeGenerator
    {
        private const string TEMPLATE = @"using Unity.Burst;
using Unity.Entities;

namespace CHECS
{{
    [UpdateInGroup(typeof(CleanUpSystemGroup))]
    public partial struct {0}CleanUpSystem : ISystem
    {{
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {{
            state.RequireForUpdate<EndCleanUpEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate<{0}>();
        }}

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {{
            var entityCommandBuffer = SystemAPI.GetSingleton<EndCleanUpEntityCommandBufferSystem.Singleton>()
                                               .CreateCommandBuffer(state.WorldUnmanaged);

            foreach (var (_, entity) in SystemAPI.Query<{0}>().WithEntityAccess())
            {{
                entityCommandBuffer.RemoveComponent<{0}>(entity);
            }}
        }}
    }}
}}";

        protected override IEnumerable<GeneratedScript> GetScriptsToGenerate()
        {
            return TypeCache.GetTypesWithAttribute<OneFrameOnlyAttribute>().Select(GetScriptToGenerate);
        }

        private static GeneratedScript GetScriptToGenerate(Type type)
        {
            var generatedFolder = GetGeneratedFolderForType(type);
            var scriptName = $"{type.Name}CleanUpSystem";
            var content = string.Format(TEMPLATE, type.Name);
            return new GeneratedScript(generatedFolder, scriptName, content);
        }
    }
}