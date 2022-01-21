using System;
using Leopotam.EcsLite;
using Project.Components;
using Project.Extensions;
using Project.Infrastructure.Enums;

namespace Project.Systems
{
    internal sealed class FigurePlacingSystem : IEcsRunSystem
    {
        private readonly EcsWorld _world;
        
        private readonly EcsFilter _filter;

        private readonly EcsPool<PlaceFigureRequest> _placeFigureRequestPool;
        private readonly EcsPool<CreateFigureRequest> _createFigureRequestPool;
        private readonly EcsPool<MoveRequest> _moveRequestPool;

        internal FigurePlacingSystem(EcsWorld world)
        {
            _world = world;
            _filter = world
                .Filter<PlaceFigureRequest>()
                .Inc<Tile>()
                .End();

            _placeFigureRequestPool = world.GetPool<PlaceFigureRequest>();
            _createFigureRequestPool = world.GetPool<CreateFigureRequest>();
            _moveRequestPool = world.GetPool<MoveRequest>();
        }

        public void Run(EcsSystems systems)
        {
            foreach (var i in _filter)
            {
                var request = _placeFigureRequestPool.Get(i);
                
                var figureEntity = GetFigureEntity(request.Team, request.Type);
                
                var destinationEntity = _world.PackEntity(i);
                
                ref var moveRequest = ref _moveRequestPool.Add(figureEntity);
                
                moveRequest.Destination = destinationEntity;
            }
        }
        
        private int GetFigureEntity(Team team, FigureType figureType)
        {
            if (CheckIfFigureExists(team, figureType, true, out var filter1))
                return filter1.GetRawEntities()[0];

            return CheckIfFigureExists(team, figureType, false, out var filter2) ? 
                filter2.GetRawEntities()[0] : RequestFigureCreation(team, figureType);
        }

        private bool CheckIfFigureExists(Team team, FigureType figureType, bool checkOnBoard, out EcsFilter filter)
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

            ref var request = ref _createFigureRequestPool.Add(entity);

            request.Team = team;
            request.Type = type;
            
            return entity;
        }
    }
}