using System;
using Cysharp.Threading.Tasks;
using Leopotam.EcsLite;
using Project.Components;
using Project.Extensions;
using Project.Infrastructure;
using UnityEngine;

namespace Project.Systems
{
    internal sealed class BoardCreatingSystem : IEcsRunSystem
    {
        private readonly EcsWorld _world;
        private readonly Configuration _config;
        
        private readonly EcsFilter _creationRequest;

        private readonly EcsPool<Tile> _tilePool;
        private readonly EcsPool<CreateViewRequest> _viewRequestPool;
        private readonly EcsPool<CreateBoardRequest> _boardRequestPool;
        private readonly EcsPool<BoardCreated> _boardCreatedPool;

        internal BoardCreatingSystem(EcsWorld world, Configuration config)
        {
            _world = world;
            _config = config;

            _creationRequest = world.Filter<CreateBoardRequest>().End();

            _boardRequestPool = world.GetPool<CreateBoardRequest>();
            _viewRequestPool = world.GetPool<CreateViewRequest>();
            _tilePool = world.GetPool<Tile>();
            _boardCreatedPool = world.GetPool<BoardCreated>();
        }

        public void Run(EcsSystems systems)
        {
            foreach (var i in _creationRequest)
            {
                var request = _boardRequestPool.Get(i);
                
                CreateBoard(request.Size).Forget();
                
                _boardRequestPool.Del(i);
            }
        }

        private async UniTaskVoid CreateBoard(Vector2Int size)
        {
            var spawnTime = _config.TimeToCreateTile / _config.NumTilesCreatingAtOnce;
            int lenght;

            if (size.y.Odd() == false)
            {
                lenght = (size.y - 1) / 2;
            }
            else
            {
                lenght = size.y / 2;
            }

            for (int yMin = 1, yMax = size.y; yMin <= lenght; yMin++, yMax--)
            {
                for (int xMin = 1, xMax = size.x; xMin <= size.x; xMin++, xMax--)
                {
                    CreateTileEntity(xMin, yMin);
                    CreateTileEntity(xMax, yMax);

                    await UniTask.Delay(TimeSpan.FromSeconds(spawnTime));
                }
            }

            if (size.y.Odd() == false)
            {
                var row = lenght + 1;

                if (size.x.Odd() == false)
                {
                    var width = (size.x - 1) / 2;
                    for (int xMin = 1, xMax = size.x; xMin <= width; xMin++, xMax--)
                    {
                        CreateTileEntity(xMin, row);
                        CreateTileEntity(xMax, row);
                        
                        await UniTask.Delay(TimeSpan.FromSeconds(spawnTime));
                    }
                    
                    CreateTileEntity(width + 1, row);
                }
                else
                {
                    var width = size.x / 2;
                    for (int xMin = 1, xMax = size.x; xMin <= width; xMin++, xMax--)
                    {
                        CreateTileEntity(xMin, row);
                        CreateTileEntity(xMax, row);
                        
                        await UniTask.Delay(TimeSpan.FromSeconds(spawnTime));
                    }
                }
            }

            _boardCreatedPool.Add(_world.NewEntity());
        }

        private void CreateTileEntity(int x, int y)
        {
            var entity = _world.NewEntity();
            ref var tileComponent = ref _tilePool.Add(entity);
                
            tileComponent.Position = new Vector2Int(x, y);

            _viewRequestPool.Add(entity);
        }
    }
}