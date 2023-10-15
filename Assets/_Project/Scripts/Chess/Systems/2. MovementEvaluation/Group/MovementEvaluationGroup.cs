using Unity.Entities;

namespace CHECS
{
    [UpdateInGroup(typeof(ChessUpdateGroup)), UpdateAfter(typeof(MovementEvaluationPreparationGroup))]
    public partial class MovementEvaluationGroup : ComponentSystemGroup { }
}