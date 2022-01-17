using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.ExtendedSystems;
using Leopotam.EcsLite.UnityEditor;
using Project.Systems;
using UnityEngine;

namespace Project.Infrastructure
{
    internal sealed class GameSceneStartup : MonoBehaviour
    {
        [SerializeField] private Configuration _configuration;
        [SerializeField] private GameSceneData _sceneData;
        
        private EcsSystems _systems;
        private EcsWorld _world;

        private void Start () 
        {
            _world = Worlds.Main;
            _systems = new EcsSystems(_world);
            
            _systems
                .AddGroup(SystemGroups.BOARD_GENERATION, false, null,
                    new BoardCreatingSystem(_world, _configuration),
                    new TileViewCreatingSystem(_world, _configuration, _sceneData),
                    new BoardCreationGroupDisablingSystem(_world))
                
                .AddGroup(SystemGroups.FIGURE_ARRANGEMENT, false, null,
                    new FigureArrangingSystem(_world),
                    new FigureCreatingSystem(_world, _sceneData),
                    new FigureViewCreatingSystem(_world, _configuration, _sceneData))
                
                .Add(new FigureMoveSystem())
                
#if UNITY_EDITOR
                .Add (new EcsWorldDebugSystem())
#endif
                
                .Inject(_configuration, _sceneData)
                .Init ();
        }

        private void Update () 
        {
            _systems?.Run ();
        }

        private void OnDestroy () 
        {
            if (_systems != null)
            {
                _systems.Destroy();
                _systems.GetWorld().Destroy();
                _systems = null;
            }
        }
    }
}