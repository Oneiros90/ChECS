using UnityEngine;

namespace CHECS
{
    public class GamePhaseManager : MonoBehaviour
    {
        private void OnEnable()
        {
            ChessInput.Actions.Game.Enable();
        }

        private void OnDisable()
        {
            ChessInput.Actions.Game.Disable();
        }
    }
}