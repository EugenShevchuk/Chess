using Leopotam.EcsLite;
using Leopotam.EcsLite.ExtendedSystems;

namespace Project.Extensions
{
    public static class WorldExtensions
    {
        public static void EnableSystemGroup(this EcsWorld world, string groupName)
        {
            var entity = world.NewEntity();
            ref var evt = ref world.GetPool<EcsGroupSystemState>().Add(entity);
            evt.Name = groupName;
            evt.State = true;
        }

        public static void DisableSystemGroup(this EcsWorld world, string groupName)
        {
            var entity = world.NewEntity();
            ref var evt = ref world.GetPool<EcsGroupSystemState>().Add(entity);
            evt.Name = groupName;
            evt.State = false;
        }
    }
}