using System;
using Leopotam.EcsLite;
using Project.Components;
using Project.Extensions;
using Project.Infrastructure.Enums;

namespace Project.Systems
{
    internal sealed class LayoutCreationFigureSelectingSystem : IEcsRunSystem
    {
        private readonly EcsWorld _world;
        
        private readonly EcsFilter _filter;

        private readonly EcsPool<SelectFigureRequest> _selectFigureRequestPool;
        private readonly EcsPool<CreateFigureRequest> _createFigureRequestPool;
        private readonly EcsPool<Selected> _selectedPool;

        internal LayoutCreationFigureSelectingSystem(EcsWorld world)
        {
            _world = world;
            
            _filter = world
                .Filter<SelectFigureRequest>()
                .End();

            _selectFigureRequestPool = world.GetPool<SelectFigureRequest>();
            _createFigureRequestPool = world.GetPool<CreateFigureRequest>();
            _selectedPool = world.GetPool<Selected>();
        }

        public void Run(EcsSystems systems)
        {
            foreach (var i in _filter)
            {
                var request = _selectFigureRequestPool.Get(i);

                if (UnusedFigureExists(request.Team, request.Type, out var filter))
                {
                    _selectedPool.Add(filter.GetRawEntities()[0]);
                }
                else
                {
                    var entity = _world.NewEntity();
                    
                    ref var creationRequest = ref _createFigureRequestPool.Add(entity);
                    creationRequest.Team = request.Team;
                    creationRequest.Type = request.Type;
                    
                    _selectedPool.Add(entity);
                }

                _selectFigureRequestPool.Del(i);
            }
        }
        
        private bool UnusedFigureExists(Team team, FigureType figureType, out EcsFilter filter)
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
                return _world
                    .Filter<TTeam>()
                    .Inc<TFigureType>()
                    .Exc<OnBoard>()
                    .Exc<MoveRequest>()
                    .End();
            }
        }
    }
}