using Unity.Burst;
using Unity.Entities;

namespace CHECS
{
    [UpdateInGroup(typeof(MovementEvaluationGroup))]
    public partial struct KnightMovementSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EvaluateValidMovesTag>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (position, validMoves) in SystemAPI
                                                   .Query<RefRO<PositionData>, DynamicBuffer<ValidMoveBufferElement>>()
                                                   .WithAll<KnightTag, EvaluateValidMovesTag>())
            {
                validMoves.Add(position.ValueRO.Shift(-2, 1));
                validMoves.Add(position.ValueRO.Shift(-1, 2));
                
                validMoves.Add(position.ValueRO.Shift(1, 2));
                validMoves.Add(position.ValueRO.Shift(2, 1));
                
                validMoves.Add(position.ValueRO.Shift(-2, -1));
                validMoves.Add(position.ValueRO.Shift(-1, -2));
                
                validMoves.Add(position.ValueRO.Shift(1, -2));
                validMoves.Add(position.ValueRO.Shift(2, -1));
            }
        }
    }
}