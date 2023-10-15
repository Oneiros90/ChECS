﻿using Unity.Burst;
using Unity.Entities;

namespace CHECS
{
    [UpdateInGroup(typeof(MovementEvaluationGroup))]
    public partial struct QueenMovementSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EvaluateValidMovesTag>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (position, validMoves) in SystemAPI
                                                   .Query<RefRO<PositionData>, DynamicBuffer<ValidMoveBufferElement>>()
                                                   .WithAll<QueenTag, EvaluateValidMovesTag>())
            {
                for (sbyte offset = -7; offset <= 7; offset++)
                {
                    if (offset == 0)
                    {
                        continue;
                    }

                    validMoves.Add(position.ValueRO.ShiftColumn(offset));
                    validMoves.Add(position.ValueRO.ShiftRow(offset));
                    validMoves.Add(position.ValueRO.Shift(offset, offset));
                    validMoves.Add(position.ValueRO.Shift(offset, (sbyte)-offset));
                }
            }
        }
    }
}