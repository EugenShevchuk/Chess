using System;
using Leopotam.EcsLite;
using Project.Components;
using Project.Extensions;
using Project.Infrastructure.BoardLayouts;
using Project.Infrastructure.Enums;
using UnityEngine;

namespace Project.Systems
{
    internal sealed class FigureArrangingSystem : IEcsRunSystem
    {
        private readonly EcsWorld _world;
        
        private readonly EcsFilter _arrangeRequest;
        private readonly EcsFilter _freeTiles;

        private readonly EcsPool<ArrangeFiguresRequest> _arrangeRequestPool;
        private readonly EcsPool<Tile> _tilePool;

        private readonly EcsPool<CreateFigureRequest> _figureRequestPool;
        private readonly EcsPool<MoveRequest> _moveRequestPool;

        internal FigureArrangingSystem(EcsWorld world)
        {
            _world = world;
            _arrangeRequest = world
                .Filter<ArrangeFiguresRequest>()
                .End();

            _freeTiles = world
                .Filter<Tile>()
                .Exc<Occupied>()
                .End();

            _arrangeRequestPool = world.GetPool<ArrangeFiguresRequest>();
            _tilePool = world.GetPool<Tile>();

            _figureRequestPool = world.GetPool<CreateFigureRequest>();
            _moveRequestPool = world.GetPool<MoveRequest>();
        }

        public void Run(EcsSystems systems)
        {
            foreach (var i in _arrangeRequest)
            {
                var layout = _arrangeRequestPool.Get(i).Layout;
                
                PlaceFiguresOnBoard(layout);
                
                _arrangeRequestPool.Del(i);
            }
        }

        private void PlaceFiguresOnBoard(FigureLayout layout)
        {
            for (var i = 0; i < layout.Tiles.Length; i++)
            {
                var team = layout.Tiles[i].Team;
                var figureType = layout.Tiles[i].FigureType;
                var position = layout.Tiles[i].Position;

                var entity = GetFigureEntity(team, figureType);

                var destination = FindDestinationTile(position);

                ref var moveRequest = ref _moveRequestPool.Add(entity);
                moveRequest.Destination = destination;
            }
        }

        private int GetFigureEntity(Team team, FigureType figureType)
        {
            if (FigureExists(team, figureType, true, out var filter1))
                return filter1.GetRawEntities()[0];

            return FigureExists(team, figureType, false, out var filter2) ? filter2.GetRawEntities()[0] : RequestFigureCreation(team, figureType);
        }

        private EcsPackedEntity FindDestinationTile(Vector2Int position)
        {
            foreach (var i in _freeTiles)
            {
                var tile = _tilePool.Get(i);
                
                if (tile.Position == position)
                    return _world.PackEntity(i);
            }
            throw new Exception("Cannot find destination tile");
        }

        private bool FigureExists(Team team, FigureType figureType, bool checkOnBoard, out EcsFilter filter)
        {
            filter = team switch
            {
                Team.White => figureType switch
                {
                    FigureType.Pawn => GetFilter<White, Pawn>(),
                    FigureType.Bishop => GetFilter<White, Bishop>(),
                    FigureType.Knight => GetFilter<White, Knight>(),
                    FigureType.Rook => GetFilter<White, Rook>(),
                    FigureType.Queen => GetFilter<White, Queen>(),
                    FigureType.King => GetFilter<White, King>(),
                    _ => throw new ArgumentOutOfRangeException()
                },
                Team.Black => figureType switch
                {
                    FigureType.Pawn => GetFilter<Black, Pawn>(),
                    FigureType.Bishop => GetFilter<Black, Bishop>(),
                    FigureType.Knight => GetFilter<Black, Knight>(),
                    FigureType.Rook => GetFilter<Black, Rook>(),
                    FigureType.Queen => GetFilter<Black, Queen>(),
                    FigureType.King => GetFilter<Black, King>(),
                    _ => throw new ArgumentOutOfRangeException()
                },
                _ => throw new ArgumentOutOfRangeException()
            };

            return filter.IsEmpty() == false;

            EcsFilter GetFilter<TTeam, TFigureType>() where TTeam : struct where TFigureType : struct
            {
                if (checkOnBoard)
                {
                    return _world
                        .Filter<TTeam>()
                        .Inc<TFigureType>()
                        .Inc<OnBoard>()
                        .Exc<MoveRequest>()
                        .End();
                }

                return _world
                    .Filter<TTeam>()
                    .Inc<TFigureType>()
                    .Exc<OnBoard>()
                    .Exc<MoveRequest>()
                    .End();
            }
        }

        private int RequestFigureCreation(Team team, FigureType type)
        {
            var entity = _world.NewEntity();

            ref var request = ref _figureRequestPool.Add(entity);

            request.Team = team;
            request.Type = type;
            
            return entity;
        }
    }
}