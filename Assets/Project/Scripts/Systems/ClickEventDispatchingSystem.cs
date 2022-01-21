using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Project.Components;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Project.Systems
{
    internal sealed class ClickEventDispatchingSystem : IEcsInitSystem
    {
        [EcsWorld] private readonly EcsWorld _world = default;
        
        [EcsInject] private readonly PlayerInput _input = default;

        [EcsPool] private readonly EcsPool<ClickEvent> _clickEventPool = default;

        private InputAction _mousePosition;

        public void Init(EcsSystems systems)
        {
            _mousePosition = _input.actions["MousePosition"];
            _input.actions["Click"].performed += _ => OnLeftMouseButtonClicked();
        }

        private void OnLeftMouseButtonClicked()
        {
            var entity = _world.NewEntity();
            ref var evt = ref _clickEventPool.Add(entity);
            evt.Position = _mousePosition.ReadValue<Vector2>();
        }
    }
}