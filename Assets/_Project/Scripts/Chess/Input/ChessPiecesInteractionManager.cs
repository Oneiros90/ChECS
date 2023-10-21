using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CHECS
{
    public struct IsSelectedTag : IComponentData { }

    public class ChessPiecesInteractionManager : MonoBehaviour
    {
        private static InputAction SelectionAction => ChessInput.Actions.Game.Selection;
        private Camera _mainCamera;

        public static Entity SelectedPiece
        {
            get
            {
                var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
                var query = new EntityQueryBuilder(Allocator.Temp).WithAll<IsSelectedTag>().Build(entityManager);
                return !query.IsEmpty ? query.GetSingletonEntity() : Entity.Null;
            }
            set
            {
                var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
                var oldSelectedPiece = SelectedPiece;
                if (oldSelectedPiece == value)
                {
                    return;
                }

                if (oldSelectedPiece != Entity.Null)
                {
                    entityManager.RemoveComponent<IsSelectedTag>(oldSelectedPiece);
                }

                if (value != Entity.Null)
                {
                    entityManager.AddComponent<IsSelectedTag>(value);
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
                ChessBoardUtilitySystem.GetPieceAtPosition(clickedPosition, out var clickedPiece);
                if (clickedPiece != Entity.Null)
                {
                    SelectedPiece = clickedPiece;
                }
            }
            else
            {
                ChessPieceUtilitySystem.RequestMovement(SelectedPiece, clickedPosition);
                SelectedPiece = Entity.Null;
            }
        }
    }
}