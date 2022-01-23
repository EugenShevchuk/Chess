using System.Collections.Generic;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Project.Components;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Project.Systems
{
    internal sealed class ClickEventDispatchingSystem : IEcsInitSystem
    {
        [EcsWorld] private readonly EcsWorld _world = default;
        
        [EcsInject] private readonly PlayerInput _input = default;

        [EcsPool] private readonly EcsPool<ClickEvent> _clickEventPool = default;

        private InputAction _mousePosition;

        private readonly PointerEventData _pointerEventData = new PointerEventData(EventSystem.current);
        private readonly List<RaycastResult> _results = new List<RaycastResult>(8);

        public void Init(EcsSystems systems)
        {
            _mousePosition = _input.actions["MousePosition"];
            _input.actions["Click"].canceled += _ => OnLeftMouseButtonClicked();
        }

        private void OnLeftMouseButtonClicked()
        {
            var position = _mousePosition.ReadValue<Vector2>();
            
            if (MouseOverUI(position))
                return;
            
            var entity = _world.NewEntity();
            ref var evt = ref _clickEventPool.Add(entity);
            evt.Position = position;
        }

        private bool MouseOverUI(Vector2 position)
        {
            _results.Clear();
            _pointerEventData.position = position;
            EventSystem.current.RaycastAll(_pointerEventData, _results);
            
            foreach (var raycastResult in _results)
                if (raycastResult.gameObject.layer == 5)
                    return true;

            return false;
        }
    }
}