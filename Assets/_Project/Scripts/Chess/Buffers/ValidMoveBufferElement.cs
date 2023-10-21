using Unity.Entities;

namespace CHECS
{
    public struct ValidMoveBufferElement : IBufferElementData
    {
        public readonly PositionData Destination;
        public readonly PieceMovementType MovementType;

        public ValidMoveBufferElement(sbyte column, sbyte row,
            PieceMovementType movementType = PieceMovementType.MoveOrTake)
        {
            Destination = new PositionData(column, row);
            MovementType = movementType;
        }

        public ValidMoveBufferElement(PositionData destination,
            PieceMovementType movementType = PieceMovementType.MoveOrTake)
        {
            Destination = destination;
            MovementType = movementType;
        }

        public static implicit operator ValidMoveBufferElement(PositionData position)
        {
            return new ValidMoveBufferElement(position.Column, position.Row);
        }

        public static implicit operator PositionData(ValidMoveBufferElement move)
        {
            return move.Destination;
        }
    }

    public static class ValidMoveBufferExtensions
    {
        public static bool Contains(this DynamicBuffer<ValidMoveBufferElement> moves, in PositionData positionData)
        {
            foreach (var move in moves)
            {
                if (move.Destination.Equals(positionData))
                {
                    return true;
                }
            }

            return false;
        }
    }
}