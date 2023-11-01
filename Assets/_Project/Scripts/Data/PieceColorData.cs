using Unity.Entities;

namespace CHECS
{
    public struct PieceColorData : IComponentData
    {
        public PieceColor Color;

        public PieceColorData(PieceColor color)
        {
            Color = color;
        }

        public static implicit operator PieceColorData(PieceColor color)
        {
            return new PieceColorData(color);
        }
    }
}