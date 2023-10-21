// ----------------------------------------------------------------------------
// This code has been generated. Please do not modify.
// ----------------------------------------------------------------------------

using Unity.Burst;
using Unity.Entities;

namespace CHECS
{
    public partial struct ChessBoardUtilitySystem
    {
        public static void GetDefault(out ChessBoardUtilitySystem system, out SystemState state)
        {
            Get(World.DefaultGameObjectInjectionWorld, out system, out state);
        }

        public static void Get(in World world, out ChessBoardUtilitySystem system, out SystemState state)
        {
            Get(world.Unmanaged, out system, out state);
        }
        
        [BurstCompile]
        public static void Get(in WorldUnmanaged world, out ChessBoardUtilitySystem system, out SystemState state)
        {
            var handle = world.GetExistingUnmanagedSystem<ChessBoardUtilitySystem>();
            system = world.GetUnsafeSystemRef<ChessBoardUtilitySystem>(handle);
            state = world.ResolveSystemStateRef(handle);
        }
        
        public static void GetPieceAtPosition(in PositionData positionData, out Entity piece)
        {
            GetDefault(out var system, out var state);
            system.GetPieceAtPosition(ref state, in positionData, out piece);
        }
        
        public static void GetPieceAtPosition(in World world, in PositionData positionData, out Entity piece)
        {
            Get(world, out var system, out var state);
            system.GetPieceAtPosition(ref state, in positionData, out piece);
        }
        
        [BurstCompile]
        public static void GetPieceAtPosition(in WorldUnmanaged world, in PositionData positionData, out Entity piece)
        {
            Get(world, out var system, out var state);
            system.GetPieceAtPosition(ref state, in positionData, out piece);
        }

    }
}