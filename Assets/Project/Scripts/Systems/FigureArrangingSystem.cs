using System;
using Cysharp.Threading.Tasks;
using Leopotam.EcsLite;
using Project.Components;
using Project.Infrastructure;
using Project.Infrastructure.BoardLayouts;
using UnityEngine;

namespace Project.Systems
{
    internal sealed class FigureArrangingSystem : IEcsRunSystem
    {
        private readonly Configuration _config;
        
        private readonly EcsFilter _arrangeRequest;
        private readonly EcsFilter _freeTiles;

        private readonly EcsPool<ArrangeFiguresRequest> _arrangeRequestPool;
        private readonly EcsPool<Tile> _tilePool;

        private readonly EcsPool<PlaceFigureRequest> _placeFigureRequestPool;

        internal FigureArrangingSystem(EcsWorld world, Configuration config)
        {
            _config = config;
            
            _arrangeRequest = world
                .Filter<ArrangeFiguresRequest>()
                .End();

            _freeTiles = world
                .Filter<Tile>()
                .Exc<Occupied>()
                .End();

            _arrangeRequestPool = world.GetPool<ArrangeFiguresRequest>();
            _tilePool = world.GetPool<Tile>();

            _placeFigureRequestPool = world.GetPool<PlaceFigureRequest>();
        }

        public void Run(EcsSystems systems)
        {
            foreach (var i in _arrangeRequest)
            {
                var layout = _arrangeRequestPool.Get(i).Layout;
                
                PlaceFiguresOnBoard(layout).Forget();
                
                _arrangeRequestPool.Del(i);
            }
        }

        private async UniTask PlaceFiguresOnBoard(FigureLayout layout)
        {
            foreach (var data in layout.Tiles)
            {
                RequestFigurePlacement(data);

                await UniTask.Delay(TimeSpan.FromSeconds(_config.FigurePlacementInterval));
            }
        }

        private void RequestFigurePlacement(FigureLayout.TileData tileData)
        {
            var destinationTileEntity = FindDestinationTileEntity(tileData.Position);
            ref var request = ref _placeFigureRequestPool.Add(destinationTileEntity);
            request.Team = tileData.Team;
            request.Type = tileData.FigureType;
        }

        private int FindDestinationTileEntity(Vector2Int position)
        {
            foreach (var i in _freeTiles)
            {
                var tile = _tilePool.Get(i);
                
                if (tile.Position == position)
                    return i;
            }
            throw new Exception("Cannot find destination tile");
        }
    }
}