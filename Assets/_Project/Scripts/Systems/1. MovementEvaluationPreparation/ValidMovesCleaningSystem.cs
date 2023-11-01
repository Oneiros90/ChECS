using Unity.Burst;
using Unity.Entities;

namespace CHECS
{
    [UpdateInGroup(typeof(MovementEvaluationPreparationGroup))]
    public partial struct ValidMovesCleaningSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EvaluateValidMovesTag>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var validMoves in SystemAPI.Query<DynamicBuffer<ValidMoveBufferElement>>()
                                                .WithAll<EvaluateValidMovesTag>())
            {
                validMoves.Clear();
            }
        }
    }
}