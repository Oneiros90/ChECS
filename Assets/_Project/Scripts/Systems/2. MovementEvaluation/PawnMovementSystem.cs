// ReSharper disable UnusedParameter.Local

using Unity.Burst;
using Unity.Entities;

namespace CHECS
{
    [UpdateInGroup(typeof(MovementEvaluationGroup))]
    public partial struct PawnMovementSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EvaluateValidMovesTag>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (position, color, validMoves) in SystemAPI
                                                          .Query<RefRO<PositionData>, RefRO<PieceColorData>,
                                                              DynamicBuffer<ValidMoveBufferElement>>()
                                                          .WithAll<PawnTag, EvaluateValidMovesTag>())
            {
                var forward = (sbyte)(color.ValueRO.Color == PieceColor.White ? 1 : -1);

                var oneStepForward = position.ValueRO.ShiftRow(forward);
                validMoves.Add(new ValidMoveBufferElement(oneStepForward, PieceMovementType.MoveOnly));

                if (position.ValueRO.Row == (sbyte)(color.ValueRO.Color == PieceColor.White ? 1 : 6))
                {
                    var twoStepsForward = position.ValueRO.ShiftRow((sbyte)(forward + forward));
                    validMoves.Add(new ValidMoveBufferElement(twoStepsForward, PieceMovementType.MoveOnly));
                }

                var takeDiagonal1 = position.ValueRO.Shift(-1, forward);
                validMoves.Add(new ValidMoveBufferElement(takeDiagonal1, PieceMovementType.TakeOnly));

                var takeDiagonal2 = position.ValueRO.Shift(1, forward);
                validMoves.Add(new ValidMoveBufferElement(takeDiagonal2, PieceMovementType.TakeOnly));
            }
        }
    }
}