using Unity.Entities;

namespace CHECS
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public partial class ChessUpdateGroup : ComponentSystemGroup { }
}