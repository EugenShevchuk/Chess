using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Project.Components;
using Project.Extensions;
using Project.Infrastructure;
using UnityEngine;

namespace Project.Systems
{
    internal sealed class FigureMoveSystem : IEcsRunSystem
    {
        [EcsWorld] private readonly EcsWorld _world = default;
        [EcsInject] private readonly Configuration _configuration = default;
        
        [EcsFilter(typeof(Figure), typeof(MoveRequest))]
        private readonly EcsFilter _figuresToMove = default;

        [EcsFilter(typeof(Figure), typeof(White), typeof(IsMoving))]
        private readonly EcsFilter _whiteMovingFigures = default;

        [EcsFilter(typeof(Figure), typeof(Black), typeof(IsMoving))]
        private readonly EcsFilter _blackMovingFigures = default;

        [EcsPool] private readonly EcsPool<Figure> _figurePool = default;
        [EcsPool] private readonly EcsPool<White> _whitePool = default;
        [EcsPool] private readonly EcsPool<Black> _blackPool = default;
        [EcsPool] private readonly EcsPool<Knight> _knightPool = default;
        [EcsPool] private readonly EcsPool<IsMoving> _isMovingPool = default;
        [EcsPool] private readonly EcsPool<WorldObject> _worldObjectPool = default;
        [EcsPool] private readonly EcsPool<MoveRequest> _moveRequestPool = default;
        
        public void Run(EcsSystems systems)
        {
            foreach (var i in _figuresToMove)
            {
                if (_whitePool.Has(i) && _whiteMovingFigures.IsEmpty() == false || 
                    _blackPool.Has(i) && _blackMovingFigures.IsEmpty() == false)
                    return;
                
                MoveFigure(i).Forget();
            }
        }

        private async UniTask MoveFigure(int figureEntity)
        {
            if (_whitePool.Has(figureEntity) && _whiteMovingFigures.IsEmpty() == false)
                return;
            
            var figureTransform = _figurePool.Get(figureEntity).Transform;
            var targetEntity = _moveRequestPool.Get(figureEntity).Destination;
             
            if (targetEntity.Unpack(_world, out var entity))
            {
                if (_worldObjectPool.Has(entity) == false)
                    throw new Exception("Target entity doesn't have world object component");

                var target = _worldObjectPool.Get(entity).Transform.position;
                target.y = 0;

                var isKnight = _knightPool.Has(entity);

                var yMoveHeight = isKnight ? _configuration.KnightFlyUpHeight : _configuration.StandardFlyUpHeight;
                var yMoveDuration = isKnight ? _configuration.KnightFlyUpTime : _configuration.StandardFlyUpTime;

                _isMovingPool.Add(figureEntity);
                
                await FlyUpAsync(figureTransform, yMoveHeight, yMoveDuration);

                await FlyToTargetAsync(figureTransform, target);

                _isMovingPool.Del(figureEntity);
                
                await FlyDownAsync(figureTransform, yMoveDuration);
            }
        }

        private async UniTask FlyUpAsync(Transform transform, float height, float duration)
        {
            await transform.DOMoveY(height, duration)
                .SetEase(_configuration.FlyUpCurve)
                .AsyncWaitForCompletion()
                .AsUniTask();
        }

        private async UniTask FlyToTargetAsync(Transform transform, Vector3 target)
        {
            target.y = transform.position.y;
            var distance = Vector3.Distance(transform.position, target);
            var duration = distance / _configuration.Speed;
            
            await transform.DOMove(target, duration)
                .SetEase(_configuration.FlyCurve)
                .AsyncWaitForCompletion()
                .AsUniTask();
        }

        private async UniTask FlyDownAsync(Transform transform, float duration)
        {
            await transform.DOMoveY(0, duration)
                .SetEase(_configuration.FlyDownCurve)
                .AsyncWaitForCompletion()
                .AsUniTask();
        }
    }
}