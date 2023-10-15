using Unity.Entities;

namespace CHECS
{
    [UpdateInGroup(typeof(ChessUpdateGroup))]
    public partial class MovementEvaluationPreparationGroup : ComponentSystemGroup { }
}