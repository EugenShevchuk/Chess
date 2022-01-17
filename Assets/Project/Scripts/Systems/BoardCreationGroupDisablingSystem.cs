using Leopotam.EcsLite;
using Project.Components;
using Project.Extensions;
using Project.Infrastructure;

namespace Project.Systems
{
    internal sealed class BoardCreationGroupDisablingSystem : IEcsRunSystem
    {
        private readonly EcsWorld _world;
        
        private readonly EcsFilter _filter;

        private readonly EcsPool<BoardCreated> _boardCreatedPool;
        
        internal BoardCreationGroupDisablingSystem(EcsWorld world)
        {
            _world = world;

            _filter = world
                .Filter<BoardCreated>()
                .End();

            _boardCreatedPool = world.GetPool<BoardCreated>();
        }

        public void Run(EcsSystems systems)
        {
            foreach (var i in _filter)
            {
                _world.DisableSystemGroup(SystemGroups.BOARD_GENERATION);
                _boardCreatedPool.Del(i);
            }
        }
    }
}