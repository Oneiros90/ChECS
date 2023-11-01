using DOTS.Extensions;
using Unity.Burst;
using Unity.Entities;

namespace CHECS
{
    [UpdateInGroup(typeof(DiscardInvalidMovesGroup))]
    public partial struct DiscardInvalidMoveOnlyMovesSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EvaluateValidMovesTag>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.GetExistingSystem(out ChessBoardUtilitySystem system);
            system.GetPositionToPieceMap(ref state, out var piecesMap);

            foreach (var (color, validMoves) in SystemAPI
                                                .Query<RefRO<PieceColorData>, DynamicBuffer<ValidMoveBufferElement>>()
                                                .WithAll<PieceTag, EvaluateValidMovesTag>())
            {
                for (var i = validMoves.Length - 1; i >= 0; i--)
                {
                    if (validMoves[i].MovementType != PieceMovementType.MoveOnly)
                    {
                        continue;
                    }

                    if (piecesMap.TryGetValue(validMoves[i], out var piece) &&
                        SystemAPI.GetComponentLookup<PieceColorData>()[piece].Color != color.ValueRO.Color)
                    {
                        validMoves.RemoveAt(i);
                    }
                }
            }
        }
    }
}