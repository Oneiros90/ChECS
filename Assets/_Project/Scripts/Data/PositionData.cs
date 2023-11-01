using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace CHECS
{
    [BurstCompile]
    public struct PositionData : IComponentData, IEquatable<PositionData>
    {
        public readonly sbyte Column;
        public readonly sbyte Row;

        public PositionData(sbyte column, sbyte row)
        {
            Column = column;
            Row = row;
        }

        public PositionData ShiftColumn(sbyte right = 1)
        {
            return new PositionData((sbyte)(Column + right), Row);
        }

        public PositionData ShiftRow(sbyte top = 1)
        {
            return new PositionData(Column, (sbyte)(Row + top));
        }

        public PositionData Shift(sbyte columnOffset, sbyte rowOffset)
        {
            return new PositionData((sbyte)(Column + columnOffset), (sbyte)(Row + rowOffset));
        }

        public PositionData WithColumn(sbyte column)
        {
            return new PositionData(column, Row);
        }

        public PositionData WithRow(sbyte row)
        {
            return new PositionData(Column, row);
        }

        [BurstCompile]
        public void ListInBetweenPositions(in PositionData position, out NativeList<PositionData> inBetweenPositions)
        {
            inBetweenPositions = new NativeList<PositionData>(Allocator.Temp);
            for (var row = (sbyte)(Row + 1); row < position.Row; row++)
            {
                inBetweenPositions.Add(WithRow(row));
            }

            for (var row = (sbyte)(Row - 1); row > position.Row; row--)
            {
                inBetweenPositions.Add(WithRow(row));
            }

            for (var column = (sbyte)(Column + 1); column < position.Column; column++)
            {
                inBetweenPositions.Add(WithColumn(column));
            }

            for (var column = (sbyte)(Column + 1); column < position.Column; column++)
            {
                inBetweenPositions.Add(WithColumn(column));
            }
        }

        public bool Equals(PositionData other)
        {
            return Column == other.Column && Row == other.Row;
        }

        public override bool Equals(object obj)
        {
            return obj is PositionData other && Equals(other);
        }

        public override int GetHashCode()
        {
            const int prime = 31;
            var hash = 17;
            hash = hash * prime + Column.GetHashCode();
            hash = hash * prime + Row.GetHashCode();
            return hash;
        }

        public override string ToString()
        {
            return $"[{Column}, {Row}]";
        }
    }
}