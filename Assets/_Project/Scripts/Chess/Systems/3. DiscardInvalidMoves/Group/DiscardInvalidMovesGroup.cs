using Unity.Entities;

namespace CHECS
{
    [UpdateInGroup(typeof(ChessUpdateGroup)), UpdateAfter(typeof(MovementEvaluationGroup))]
    public partial class DiscardInvalidMovesGroup : ComponentSystemGroup { }
}