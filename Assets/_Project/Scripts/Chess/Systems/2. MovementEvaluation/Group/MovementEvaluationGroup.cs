using Unity.Entities;

namespace CHECS
{
    [UpdateInGroup(typeof(ChessUpdateSystemGroup)), UpdateAfter(typeof(MovementEvaluationPreparationGroup))]
    public partial class MovementEvaluationGroup : ComponentSystemGroup { }
}