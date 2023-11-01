// ReSharper disable Unity.Entities.SingletonMustBeRequested
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace CHECS
{
    [BurstCompile]
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

        [BurstCompile]
        public void GetPositionToPieceMap(ref SystemState _, out NativeHashMap<PositionData, Entity> map)
        {
            map = new NativeHashMap<PositionData, Entity>(32, Allocator.Temp);
            foreach (var (piecePosition, entity) in SystemAPI.Query<PositionData>().WithAll<PieceTag>()
                                                             .WithEntityAccess())
            {
                map[piecePosition] = entity;
            }
        }
    }
}