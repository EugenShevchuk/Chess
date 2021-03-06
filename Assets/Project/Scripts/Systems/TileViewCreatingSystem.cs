using DG.Tweening;
using Leopotam.EcsLite;
using Project.Components;
using Project.Extensions;
using Project.Infrastructure;
using Project.Views;
using UnityEngine;

namespace Project.Systems
{
    internal sealed class TileViewCreatingSystem : IEcsRunSystem
    {
        private readonly EcsWorld _world;
        private readonly Configuration _configuration;
        private readonly GameSceneData _sceneData;

        private readonly EcsFilter _filter;
        
        private readonly EcsPool<ObjectViewRef> _viewRefPool;
        private readonly EcsPool<WorldObject> _worldObjectPool;
        private readonly EcsPool<Tile> _tilePool;

        internal TileViewCreatingSystem(EcsWorld world, Configuration configuration, GameSceneData sceneData)
        {
            _world = world;
            _configuration = configuration;
            _sceneData = sceneData;

            _filter = world.Filter<CreateViewRequest>()
                .Inc<Tile>()
                .Exc<ObjectViewRef>()
                .End();
            
            _viewRefPool = world.GetPool<ObjectViewRef>();
            _worldObjectPool = world.GetPool<WorldObject>();
            _tilePool = world.GetPool<Tile>();
        }

        public void Run(EcsSystems systems)
        {
            foreach (var i in _filter)
            {
                var tileData = _tilePool.Get(i);

                ref var viewRef = ref _viewRefPool.Add(i);
                ref var worldObject = ref _worldObjectPool.Add(i);
                
                (viewRef.View, worldObject.Transform) = CreateView(tileData.Position, i);
            }
        }

        private (IObjectView, Transform) CreateView(Vector2Int pos, int entity)
        {
            var position = new Vector3(pos.x, -_configuration.TileYSize / 2, pos.y);
            var tile = Object.Instantiate(_configuration.TilePrefab, position, Quaternion.identity, _sceneData.Board);
            tile.transform.localScale = Vector3.zero;

            tile.gameObject.name = $"X:{pos.x.ToString()} Y:{pos.y.ToString()}";
            tile.gameObject.layer = LayerMask.NameToLayer("Board");

            var material = tile.GetComponent<MeshRenderer>().material;
            
            material.color = pos.x.Odd() == pos.y.Odd() ? _configuration.WhiteTileColor : _configuration.BlackTileColor;

            tile.Entity = _world.PackEntity(entity);
            
            tile.transform.DOScale(Vector3.one, _configuration.TimeToCreateTile)
                .SetEase(_configuration.TileEaseType);
            
            return (tile, tile.transform);
        }
    }
}