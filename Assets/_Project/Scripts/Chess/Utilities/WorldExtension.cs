using Unity.Burst;
using Unity.Entities;

namespace CHECS
{
    [BurstCompile]
    public static class WorldExtension
    {
        [BurstCompile]
        public static void GetSystemAndState<T>(this WorldUnmanaged world, out T system, out SystemState state)
            where T : unmanaged, ISystem
        {
            var handle = world.GetExistingUnmanagedSystem<T>();
            system = world.GetUnsafeSystemRef<T>(handle);
            state = world.ResolveSystemStateRef(handle);
        }
    }
}