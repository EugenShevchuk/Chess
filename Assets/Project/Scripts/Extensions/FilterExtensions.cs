using Leopotam.EcsLite;

namespace Project.Extensions
{
    public static class FilterExtensions
    {
        public static bool IsEmpty(this EcsFilter filter)
        {
            return filter.GetEntitiesCount() == 0;
        }
    }
}