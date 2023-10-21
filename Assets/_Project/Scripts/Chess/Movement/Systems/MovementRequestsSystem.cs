using Unity.Burst;
using Unity.Entities;

namespace CHECS
{
    [BurstCompile, UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial struct MovementRequestsSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<MovementRequestData>();
            state.RequireForUpdate<EndInitializationEntityCommandBufferSystem.Singleton>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var commandBuffer = SystemAPI.GetSingleton<EndInitializationEntityCommandBufferSystem.Singleton>()
                                         .CreateCommandBuffer(state.WorldUnmanaged);

            foreach (var (request, validMoves, entity) in SystemAPI
                                                          .Query<RefRO<MovementRequestData>,
                                                              DynamicBuffer<ValidMoveBufferElement>>()
                                                          .WithEntityAccess())
            {
                var requestedPosition = request.ValueRO.Position;
                if (validMoves.Contains(requestedPosition))
                {
                    commandBuffer.SetComponent(entity, requestedPosition);
                    commandBuffer.AddComponent<PieceHasMovedInThisFrameTag>(entity);
                }
            }
        }
    }
}