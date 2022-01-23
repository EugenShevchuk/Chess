using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Project.Components;

namespace Project.Systems
{
    internal sealed class FigureDeselectionSystem : IEcsRunSystem
    {
        [EcsFilter(typeof(Selected))] 
        private readonly EcsFilter _selected = default;

        [EcsPool] private readonly EcsPool<Selected> _selectedPool = default;
        
        public void Run(EcsSystems systems)
        {
            if (_selected.GetEntitiesCount() > 1)
            {
                for (int i = 0; i < _selected.GetEntitiesCount() - 1; i++)
                {
                    _selectedPool.Del(_selected.GetRawEntities()[i]);
                }
            }
        }
    }
}