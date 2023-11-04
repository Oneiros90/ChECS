// ----------------------------------------------------------------------------
// This code has been generated. Please do not modify.
// ----------------------------------------------------------------------------

using Unity.Burst;
using Unity.Entities;

namespace CHECS
{
    [UpdateInGroup(typeof(CleanUpSystemGroup))]
    public partial struct EvaluateValidMovesTagCleanUpSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EndCleanUpEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate<EvaluateValidMovesTag>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var entityCommandBuffer = SystemAPI.GetSingleton<EndCleanUpEntityCommandBufferSystem.Singleton>()
                                               .CreateCommandBuffer(state.WorldUnmanaged);

            foreach (var (_, entity) in SystemAPI.Query<EvaluateValidMovesTag>().WithEntityAccess())
            {
                entityCommandBuffer.RemoveComponent<EvaluateValidMovesTag>(entity);
            }
        }
    }
}