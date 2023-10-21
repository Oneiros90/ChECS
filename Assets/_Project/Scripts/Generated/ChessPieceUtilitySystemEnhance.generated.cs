// ----------------------------------------------------------------------------
// This code has been generated. Please do not modify.
// ----------------------------------------------------------------------------

using Unity.Burst;
using Unity.Entities;

namespace CHECS
{
    public partial struct ChessPieceUtilitySystem
    {
        public static void GetDefault(out ChessPieceUtilitySystem system, out SystemState state)
        {
            Get(World.DefaultGameObjectInjectionWorld, out system, out state);
        }

        public static void Get(in World world, out ChessPieceUtilitySystem system, out SystemState state)
        {
            Get(world.Unmanaged, out system, out state);
        }
        
        [BurstCompile]
        public static void Get(in WorldUnmanaged world, out ChessPieceUtilitySystem system, out SystemState state)
        {
            var handle = world.GetExistingUnmanagedSystem<ChessPieceUtilitySystem>();
            system = world.GetUnsafeSystemRef<ChessPieceUtilitySystem>(handle);
            state = world.ResolveSystemStateRef(handle);
        }
        
        public static void GetColor(in Entity piece, out PieceColorData colorData)
        {
            GetDefault(out var system, out var state);
            system.GetColor(ref state, in piece, out colorData);
        }
        
        public static void GetColor(in World world, in Entity piece, out PieceColorData colorData)
        {
            Get(world, out var system, out var state);
            system.GetColor(ref state, in piece, out colorData);
        }
        
        [BurstCompile]
        public static void GetColor(in WorldUnmanaged world, in Entity piece, out PieceColorData colorData)
        {
            Get(world, out var system, out var state);
            system.GetColor(ref state, in piece, out colorData);
        }

        public static void RequestMovement(in Entity piece, in PositionData position)
        {
            GetDefault(out var system, out var state);
            system.RequestMovement(ref state, in piece, in position);
        }
        
        public static void RequestMovement(in World world, in Entity piece, in PositionData position)
        {
            Get(world, out var system, out var state);
            system.RequestMovement(ref state, in piece, in position);
        }
        
        [BurstCompile]
        public static void RequestMovement(in WorldUnmanaged world, in Entity piece, in PositionData position)
        {
            Get(world, out var system, out var state);
            system.RequestMovement(ref state, in piece, in position);
        }

    }
}