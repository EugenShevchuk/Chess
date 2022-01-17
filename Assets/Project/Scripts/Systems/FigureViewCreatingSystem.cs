﻿using System;
using Leopotam.EcsLite;
using Project.Components;
using Project.Infrastructure;
using Project.Infrastructure.Enums;
using Project.Views;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Project.Systems
{
    internal sealed class FigureViewCreatingSystem : IEcsRunSystem
    {
        private readonly Configuration _configuration;
        private readonly GameSceneData _sceneData;
        
        private readonly EcsFilter _filter;

        private readonly EcsPool<Figure> _figurePool;
        private readonly EcsPool<CreateViewRequest> _viewRequestPool;
        private readonly EcsPool<CreateFigureRequest> _figureRequestPool;
        private readonly EcsPool<ObjectViewRef> _viewRefPool;
        
        public FigureViewCreatingSystem(EcsWorld world, Configuration configuration, GameSceneData sceneData)
        {
            _configuration = configuration;
            _sceneData = sceneData;

            _filter = world
                .Filter<Figure>()
                .Inc<CreateViewRequest>()
                .Inc<CreateFigureRequest>()
                .Exc<ObjectViewRef>()
                .End();

            _figurePool = world.GetPool<Figure>();
            _viewRequestPool = world.GetPool<CreateViewRequest>();
            _figureRequestPool = world.GetPool<CreateFigureRequest>();
            _viewRefPool = world.GetPool<ObjectViewRef>();
        }

        public void Run(EcsSystems systems)
        {
            foreach (var i in _filter)
            {
                var figureRequest = _figureRequestPool.Get(i);

                ref var viewRef = ref _viewRefPool.Add(i);
                ref var figure = ref _figurePool.Get(i);

                (viewRef.View, figure.Transform) = CreateView(figureRequest.Type, figureRequest.Team);
                
                _viewRequestPool.Del(i);
            }
        }

        private (FigureView, Transform) CreateView(FigureType type, Team team)
        {
            var prefab = _configuration.FigureMap[type];

            Color color;
            Vector3 position;
            Quaternion rotation;

            switch (team)
            {
                case Team.White:
                    color = _configuration.WhiteFigureColor;
                    position = _sceneData.WhiteTeamSpawnPoint.position;
                    rotation = Quaternion.identity;
                    break;
                case Team.Black:
                    color = _configuration.BlackFigureColor;
                    position = _sceneData.BlackTeamSpawnPoint.position;
                    rotation = new Quaternion(0, 180, 0, 0);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(team), team, null);
            }

            var view = Object.Instantiate(prefab, position, rotation);
            view.GetComponent<MeshRenderer>().material.color = color;
            return (view, view.transform);
        }
    }
}