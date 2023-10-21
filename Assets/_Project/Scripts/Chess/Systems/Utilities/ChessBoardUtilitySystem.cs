// ReSharper disable Unity.Entities.SingletonMustBeRequested
using Unity.Burst;
using Unity.Entities;

namespace CHECS
{
    [BurstCompile, EnhanceSystem]
    public partial struct ChessBoardUtilitySystem : ISystem
    {
        [BurstCompile]
        public void GetPieceAtPosition(ref SystemState _, in PositionData positionData, out Entity piece)
        {
            foreach (var (piecePosition, entity) in SystemAPI.Query<PositionData>().WithAll<PieceTag>()
                                                            .WithEntityAccess())
            {
                if (piecePosition.Equals(positionData))
                {
                    piece = entity;
                    return;
                }
            }

            piece = Entity.Null;
        }
    }
}