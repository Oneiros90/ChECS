using Unity.Entities;

namespace CHECS
{
    [UpdateInGroup(typeof(ChessUpdateSystemGroup))]
    public partial class MovementEvaluationPreparationGroup : ComponentSystemGroup { }
}