using Unity.Entities;

namespace CHECS
{
    [OneFrameOnly]
    public struct MovementRequestData : IComponentData
    {
        public PositionData Position;
    }
}