using Unity.Collections;
using Unity.Entities;

namespace CHECS
{
    public struct GameStateLookup
    {
        private readonly EntityQuery _allPiecesQuery;

        private readonly ComponentLookup<PositionData> _positionLookup;

        public GameStateLookup(ref SystemState state)
        {
            _positionLookup = state.GetComponentLookup<PositionData>(true);
            _allPiecesQuery = state.GetEntityQuery(new EntityQueryBuilder(Allocator.Temp).WithAll<PieceTag>());
        }

        public void Update(ref SystemState state)
        {
            _positionLookup.Update(ref state);
        }

        public void GetAllPiecesPositions(out NativeHashSet<PositionData> piecePositions)
        {
            piecePositions = new NativeHashSet<PositionData>(32, Allocator.Temp);
            foreach (var pieceEntity in _allPiecesQuery.ToEntityArray(Allocator.Temp))
            {
                piecePositions.Add(_positionLookup[pieceEntity]);
            }
        }
    }
}