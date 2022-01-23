using System;
using Leopotam.EcsLite;
using Project.Components;
using Project.Infrastructure;
using Project.Infrastructure.Enums;

namespace Project.Systems
{
    internal sealed class FigureCreatingSystem : IEcsRunSystem
    {
        private readonly GameSceneData _sceneData;
        
        private readonly EcsFilter _createFigureRequest;

        private readonly EcsPool<CreateFigureRequest> _figureRequestPool;
        private readonly EcsPool<CreateViewRequest> _viewRequestPool;
        
        private readonly EcsPool<Figure> _figurePool;
        private readonly EcsPool<White> _whiteTeamPool;
        private readonly EcsPool<Black> _blackTeamPool;
        private readonly EcsPool<Pawn> _pawnPool;
        private readonly EcsPool<Bishop> _bishopPool;
        private readonly EcsPool<Knight> _knightPool;
        private readonly EcsPool<Rook> _rookPool;
        private readonly EcsPool<Queen> _queenPool;
        private readonly EcsPool<King> _kingPool;

        public FigureCreatingSystem(EcsWorld world, GameSceneData sceneData)
        {
            _sceneData = sceneData;
            
            _createFigureRequest = world
                .Filter<CreateFigureRequest>()
                .End();

            _figureRequestPool = world.GetPool<CreateFigureRequest>();
            _viewRequestPool = world.GetPool<CreateViewRequest>();
            
            _figurePool = world.GetPool<Figure>();
            _whiteTeamPool = world.GetPool<White>();
            _blackTeamPool = world.GetPool<Black>();
            _pawnPool = world.GetPool<Pawn>();
            _bishopPool = world.GetPool<Bishop>();
            _knightPool = world.GetPool<Knight>();
            _rookPool = world.GetPool<Rook>();
            _queenPool = world.GetPool<Queen>();
            _kingPool = world.GetPool<King>();
        }

        public void Run(EcsSystems systems)
        {
            foreach (var i in _createFigureRequest)
            {
                var request = _figureRequestPool.Get(i);
                
                CreateFigure(i, request.Team, request.Type);

                RequestViewCreation(i, request.Team);
            }
        }

        private void CreateFigure(int entity, Team team, FigureType type)
        {
            switch (team)
            {
                case Team.White:
                    _whiteTeamPool.Add(entity);
                    break;
                case Team.Black:
                    _blackTeamPool.Add(entity);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(team), team, null);
            }

            switch (type)
            {
                case FigureType.Pawn:
                    _pawnPool.Add(entity);
                    break;
                case FigureType.Bishop:
                    _bishopPool.Add(entity);
                    break;
                case FigureType.Knight:
                    _knightPool.Add(entity);
                    break;
                case FigureType.Rook:
                    _rookPool.Add(entity);
                    break;
                case FigureType.Queen:
                    _queenPool.Add(entity);
                    break;
                case FigureType.King:
                    _kingPool.Add(entity);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        private void RequestViewCreation(int entity, Team team)
        {
            ref var viewRequest = ref _viewRequestPool.Add(entity);

            viewRequest.Position = team switch
            {
                Team.White => _sceneData.WhiteTeamSpawnPoint.position,
                Team.Black => _sceneData.BlackTeamSpawnPoint.position,
                _ => throw new ArgumentOutOfRangeException(nameof(team), team, null)
            };
        }
    }
}