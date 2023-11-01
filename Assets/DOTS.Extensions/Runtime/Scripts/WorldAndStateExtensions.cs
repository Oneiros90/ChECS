using Unity.Burst;
using Unity.Entities;

namespace DOTS.Extensions
{
    [BurstCompile]

    public static class WorldAndStateExtensions
    {
        public static void GetExistingSystemAndState<T>(this World world, out T system, out SystemState state)
            where T : unmanaged, ISystem
        {
            GetExistingSystemAndState(world.Unmanaged, out system, out state);
        }

        [BurstCompile]
        public static void GetExistingSystemAndState<T>(this WorldUnmanaged world, out T system, out SystemState state)
            where T : unmanaged, ISystem
        {
            world.GetExistingSystem(out var handle, out system);
            state = world.ResolveSystemStateRef(handle);
        }

        [BurstCompile]
        private static void GetExistingSystem<T>(this WorldUnmanaged world, out SystemHandle handle, out T system)
            where T : unmanaged, ISystem
        {
            handle = world.GetExistingUnmanagedSystem<T>();
            system = world.GetUnsafeSystemRef<T>(handle);
        }

        [BurstCompile]
        public static void GetExistingSystem<T>(this SystemState state, out T system) where T : unmanaged, ISystem
        {
            GetExistingSystem(state.WorldUnmanaged, out var _, out system);
        }
    }
}