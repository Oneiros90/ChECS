using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace CHECS
{
    [UpdateInGroup(typeof(DiscardInvalidMovesGroup))]
    public partial struct DiscardMovesOnSameColorPiecesSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EvaluateValidMovesTag>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var piecePositions = new NativeHashMap<PositionData, PieceColor>(32, Allocator.Temp);
            foreach (var (position, color) in SystemAPI.Query<RefRO<PositionData>, RefRO<PieceColorData>>()
                                                       .WithAll<PieceTag>())
            {
                piecePositions.Add(position.ValueRO, color.ValueRO.Color);
            }

            foreach (var (color, validMoves) in SystemAPI
                                                .Query<RefRO<PieceColorData>, DynamicBuffer<ValidMoveBufferElement>>()
                                                .WithAll<PieceTag, EvaluateValidMovesTag>())
            {
                for (var i = validMoves.Length - 1; i >= 0; i--)
                {
                    if (piecePositions.TryGetValue(validMoves[i], out var otherColor) &&
                        color.ValueRO.Color == otherColor)
                    {
                        validMoves.RemoveAt(i);
                    }
                }
            }
        }
    }
}