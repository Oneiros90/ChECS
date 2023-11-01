using Unity.Burst;
using Unity.Entities;

namespace CHECS
{
    [BurstCompile, UpdateInGroup(typeof(SimulationSystemGroup), OrderFirst = true)]
    public partial struct RequestMovesEvaluationAfterMovementSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<BeginChessUpdateEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate<PieceHasMovedInThisFrameTag>();
        }

        public void OnUpdate(ref SystemState state)
        {
            var commandBuffer = SystemAPI.GetSingleton<BeginChessUpdateEntityCommandBufferSystem.Singleton>()
                                         .CreateCommandBuffer(state.WorldUnmanaged);

            foreach (var (_, entity) in SystemAPI.Query<PieceTag>().WithEntityAccess())
            {
                commandBuffer.AddComponent<EvaluateValidMovesTag>(entity);
            }
        }
    }
}