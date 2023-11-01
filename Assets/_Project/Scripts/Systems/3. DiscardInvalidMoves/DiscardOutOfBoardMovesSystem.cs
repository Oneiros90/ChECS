using Unity.Burst;
using Unity.Entities;

namespace CHECS
{
    [UpdateInGroup(typeof(DiscardInvalidMovesGroup))]
    public partial struct DiscardOutOfBoardMovesSystem : ISystem
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
                                                .WithAll<PieceTag, EvaluateValidMovesTag>())
            {
                for (var i = validMoves.Length - 1; i >= 0; i--)
                {
                    var destination = validMoves[i].Destination;
                    if (destination.Column < 0 || destination.Column >= 8 || destination.Row < 0 ||
                        destination.Row >= 8)
                    {
                        validMoves.RemoveAt(i);
                    }
                }
            }
        }
    }
}