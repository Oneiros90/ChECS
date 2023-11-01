using DOTS.Extensions;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CHECS
{
    public struct IsSelectedTag : IComponentData { }

    public class ChessPiecesInteractionManager : MonoBehaviour
    {
        private static World World => World.DefaultGameObjectInjectionWorld;
        private static InputAction SelectionAction => ChessInput.Actions.Game.Selection;

        private Camera _mainCamera;

        public static Entity SelectedPiece
        {
            get
            {
                var query = World.EntityManager.CreateEntityQuery(new EntityQueryBuilder(Allocator.Temp)
                    .WithAll<IsSelectedTag>());
                return !query.IsEmpty ? query.GetSingletonEntity() : Entity.Null;
            }
            set
            {
                var oldSelectedPiece = SelectedPiece;
                if (oldSelectedPiece == value)
                {
                    return;
                }

                if (oldSelectedPiece != Entity.Null)
                {
                    World.EntityManager.RemoveComponent<IsSelectedTag>(oldSelectedPiece);
                }

                if (value != Entity.Null)
                {
                    World.EntityManager.AddComponent<IsSelectedTag>(value);
                }
            }
        }

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private void OnEnable()
        {
            SelectionAction.performed += OnSelectionPerformed;
        }

        private void OnDisable()
        {
            SelectionAction.performed -= OnSelectionPerformed;
        }

        private void OnSelectionPerformed(InputAction.CallbackContext ctx)
        {
            var position = _mainCamera.ScreenToWorldPoint(Mouse.current.position.value);
            var clickedPosition =
                new PositionData((sbyte)Mathf.RoundToInt(position.x), (sbyte)Mathf.RoundToInt(position.z));

            if (SelectedPiece == Entity.Null)
            {
                World.GetExistingSystemAndState(out ChessBoardUtilitySystem system, out var state);
                system.GetPieceAtPosition(ref state, clickedPosition, out var clickedPiece);
                if (clickedPiece != Entity.Null)
                {
                    SelectedPiece = clickedPiece;
                }
            }
            else
            {
                World.GetExistingSystemAndState(out ChessPieceUtilitySystem system, out var state);
                system.RequestMovement(ref state, SelectedPiece, clickedPosition);
                SelectedPiece = Entity.Null;
            }
        }
    }
}