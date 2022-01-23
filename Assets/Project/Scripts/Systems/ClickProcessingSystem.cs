using System;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Project.Components;
using Project.Extensions;
using Project.Infrastructure;
using Project.Views;
using UnityEngine;

namespace Project.Systems
{
    internal sealed class ClickProcessingSystem : IEcsRunSystem
    {
        [EcsWorld] private readonly EcsWorld _world = default;
        
        [EcsInject] private readonly Camera _camera = default;
        [EcsInject] private readonly Configuration _config = default;
        
        [EcsFilter(typeof(ClickEvent))] 
        private readonly EcsFilter _filter = default;

        [EcsFilter(typeof(Selected))] 
        private readonly EcsFilter _selected = default;

        [EcsPool] private readonly EcsPool<ClickEvent> _clickEventPool = default;
        [EcsPool] private readonly EcsPool<Selected> _selectedPool = default;
        [EcsPool] private readonly EcsPool<MoveRequest> _moveRequestPool = default;
        
        public void Run(EcsSystems systems)
        {
            foreach (var i in _filter)
            {
                var evt = _clickEventPool.Get(i);
                ProcessClick(evt.Position);
            }
        }

        private void ProcessClick(Vector2 position)
        {
            var ray = _camera.ScreenPointToRay(position);

            if (Physics.Raycast(ray, out var hit, _config.RaycastDistance, _config.InteractableLayers.value))
            {
                if (hit.transform.TryGetComponent(out IObjectView view))
                {
                    switch (view)
                    {
                        case FigureView figureView:
                            ProcessClickOnFigure(figureView);
                            break;
                        case TileView tileView:
                            ProcessClickOnTile(tileView);
                            break;
                    }
                }
            }
            else
            {
                ProcessEmptyClick();
            }
        }

        private void ProcessClickOnFigure(FigureView view)
        {
            if (view.Entity.Unpack(_world, out var entity))
                _selectedPool.Add(entity);
        }

        private void ProcessClickOnTile(TileView view)
        {
            if (_selected.IsEmpty())
                return;
                                
            if (view.Entity.Unpack(_world, out var entity))
            {
                if (_selected.HasOneEntity())
                {
                    var selectedEntity = _selected.GetRawEntities()[0];
                    if (_moveRequestPool.Has(selectedEntity) == false)
                    {
                        ref var moveRequest = ref _moveRequestPool.Add(selectedEntity);
                        moveRequest.Destination = _world.PackEntity(entity);
                    }
                }
                else
                    throw new Exception("There is more than one entity selected");
            }
        }

        private void ProcessEmptyClick()
        {
            foreach (var i in _selected)
                _selectedPool.Del(i);
        }
    }
}