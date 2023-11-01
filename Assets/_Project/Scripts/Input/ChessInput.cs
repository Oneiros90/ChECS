using UnityEngine;

namespace CHECS
{
    public static class ChessInput
    {
        public static ChessInputActions Actions { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void RuntimeInit()
        {
            Actions = new ChessInputActions();
        }
    }
}