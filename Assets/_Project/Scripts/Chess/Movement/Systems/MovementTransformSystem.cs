using Unity.Burst;
using Unity.Entities;
using UnityEngine;

namespace CHECS
{
    [BurstCompile, UpdateInGroup(typeof(PresentationSystemGroup))]
    public partial struct MovementTransformSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PieceHasMovedInThisFrameTag>();
        }

        public void OnUpdate(ref SystemState state)
        {
            foreach (var (position, instanceData) in SystemAPI.Query<RefRO<PositionData>, AuthoringInstanceData>()
                                                              .WithAll<PieceHasMovedInThisFrameTag>())
            {
                instanceData.Object.transform.position = new Vector3(position.ValueRO.Column, 0, position.ValueRO.Row);
            }
        }
    }
}