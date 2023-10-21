using UnityEngine;
using UnityEngine.InputSystem;

namespace CHECS
{
    public class ChessPiecesInteractionManager : MonoBehaviour
    {
        private static InputAction SelectionAction => ChessInput.Actions.Game.Selection;
        private Camera _mainCamera;

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

            ChessBoardUtilitySystem.GetPieceAtPosition(clickedPosition, out var piece);
            Debug.Log(piece);
        }
    }
}