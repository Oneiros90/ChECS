using Unity.Burst;
using Unity.Entities;

namespace CHECS
{
    [BurstCompile, EnhanceSystem]
    public partial struct ChessBoardUtilitySystem : ISystem
    {
        [BurstCompile]
        public void GetPieceAtPosition(ref SystemState _, in PositionData positionData, out Entity result)
        {
            foreach (var (piecePosition, piece) in SystemAPI.Query<PositionData>().WithAll<PieceTag>()
                                                            .WithEntityAccess())
            {
                if (piecePosition.Equals(positionData))
                {
                    result = piece;
                    return;
                }
            }

            result = Entity.Null;
        }
    }
}