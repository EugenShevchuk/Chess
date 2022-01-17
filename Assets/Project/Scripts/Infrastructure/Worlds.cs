using Leopotam.EcsLite;

namespace Project.Infrastructure
{
    internal sealed class Worlds
    {
        public static readonly EcsWorld Main = new EcsWorld();

        public static readonly EcsWorld Events = new EcsWorld();
    }
}