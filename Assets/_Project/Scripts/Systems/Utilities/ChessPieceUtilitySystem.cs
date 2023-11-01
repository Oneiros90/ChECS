// ReSharper disable Unity.Entities.SingletonMustBeRequested
using Unity.Burst;
using Unity.Entities;

namespace CHECS
{
    [BurstCompile]
    public partial struct ChessPieceUtilitySystem : ISystem
    {
        [BurstCompile]
        public void GetColor(ref SystemState _, in Entity piece, out PieceColorData colorData)
        {
            SystemAPI.GetComponentLookup<PieceColorData>().TryGetComponent(piece, out colorData);
        }

        [BurstCompile]
        public void RequestMovement(ref SystemState state, in Entity piece, in PositionData position)
        {
            var commandBuffer = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>()
                                         .CreateCommandBuffer(state.WorldUnmanaged);

            commandBuffer.AddComponent(piece, new MovementRequestData { Position = position });
        }
    }
}