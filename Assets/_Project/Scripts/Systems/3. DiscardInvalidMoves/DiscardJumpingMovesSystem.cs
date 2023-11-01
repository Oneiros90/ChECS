using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace CHECS
{
    [UpdateInGroup(typeof(DiscardInvalidMovesGroup))]
    public partial struct DiscardJumpingMovesSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EvaluateValidMovesTag>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var piecePositions = new NativeHashSet<PositionData>(32, Allocator.Temp);
            foreach (var position in SystemAPI.Query<RefRO<PositionData>>().WithAll<PieceTag>())
            {
                piecePositions.Add(position.ValueRO);
            }

            foreach (var (position, validMoves) in SystemAPI
                                                   .Query<RefRO<PositionData>, DynamicBuffer<ValidMoveBufferElement>>()
                                                   .WithAll<PieceTag, EvaluateValidMovesTag>()
                                                   .WithNone<CanJumpOtherPiecesTag>())
            {
                for (var i = validMoves.Length - 1; i >= 0; i--)
                {
                    var destination = validMoves[i].Destination;
                    position.ValueRO.ListInBetweenPositions(destination, out var inBetweenPositions);
                    foreach (var inBetweenPosition in inBetweenPositions)
                    {
                        if (piecePositions.Contains(inBetweenPosition))
                        {
                            validMoves.RemoveAt(i);
                            break;
                        }
                    }
                }
            }
        }
    }
}