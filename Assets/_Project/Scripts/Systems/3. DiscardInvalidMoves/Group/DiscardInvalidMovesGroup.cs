using Unity.Entities;

namespace CHECS
{
    [UpdateInGroup(typeof(ChessUpdateSystemGroup)), UpdateAfter(typeof(MovementEvaluationGroup))]
    public partial class DiscardInvalidMovesGroup : ComponentSystemGroup { }
}