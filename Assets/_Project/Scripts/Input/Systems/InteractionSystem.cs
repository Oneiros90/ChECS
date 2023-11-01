using DOTS.Extensions;
using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CHECS
{
    [UpdateInGroup(typeof(InputUpdateSystemGroup))]
    public partial struct InteractionSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EndInputUpdateEntityCommandBufferSystem.Singleton>();
        }

        public void OnUpdate(ref SystemState state)
        {
            if (!ChessInput.Actions.Game.Selection.triggered)
            {
                return;
            }

            var position = Camera.main!.ScreenToWorldPoint(Mouse.current.position.value);
            var clickedPosition =
                new PositionData((sbyte)Mathf.RoundToInt(position.x), (sbyte)Mathf.RoundToInt(position.z));

            var commandBuffer = SystemAPI.GetSingleton<EndInputUpdateEntityCommandBufferSystem.Singleton>()
                                         .CreateCommandBuffer(state.WorldUnmanaged);

            if (!SystemAPI.TryGetSingletonEntity<IsSelectedTag>(out var selectedPiece))
            {
                state.GetExistingSystem(out ChessBoardUtilitySystem chessBoardUtilitySystem);
                chessBoardUtilitySystem.GetPieceAtPosition(ref state, clickedPosition, out var clickedPiece);
                if (clickedPiece != Entity.Null)
                {
                    commandBuffer.AddComponent<IsSelectedTag>(clickedPiece);
                }
            }
            else
            {
                state.GetExistingSystem(out ChessPieceUtilitySystem system);
                system.RequestMovement(ref state, selectedPiece, clickedPosition);
                commandBuffer.RemoveComponent<IsSelectedTag>(selectedPiece);
            }
        }
    }
}