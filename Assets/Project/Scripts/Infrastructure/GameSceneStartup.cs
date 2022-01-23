using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.ExtendedSystems;
using Leopotam.EcsLite.UnityEditor;
using Project.Components;
using Project.Systems;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Project.Infrastructure
{
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(GameSceneData))]
    internal sealed class GameSceneStartup : MonoBehaviour
    {
        private EcsSystems _systems;
        private EcsWorld _world;

        private void Start() 
        {
            _world = Worlds.Main;
            _systems = new EcsSystems(_world);

            var sceneData = GetComponent<GameSceneData>();
            var input = GetComponent<PlayerInput>();
            var config = Resources.Load<Configuration>("Configuration");
            var cam = Camera.main;
            
            _systems
                .AddGroup(SystemGroups.BOARD_GENERATION, false, null,
                    new BoardCreatingSystem(_world, config),
                    new TileViewCreatingSystem(_world, config, sceneData),
                    new BoardCreationGroupDisablingSystem(_world))
                
                .AddGroup(SystemGroups.FIGURE_ARRANGEMENT, false, null,
                    new FigureArrangingSystem(_world, config),
                    new FigurePlacingSystem(_world),
                    new FigureCreatingSystem(_world, sceneData),
                    new FigureViewCreatingSystem(_world, config, sceneData))
                
                .AddGroup(SystemGroups.LAYOUT_CREATION, false, null,
                    new LayoutCreationFigureSelectingSystem(_world),
                    new FigureCreatingSystem(_world, sceneData),
                    new FigureViewCreatingSystem(_world, config, sceneData))
                
                .AddGroup(SystemGroups.FIGURE_CREATION, false, null,
                    new FigureCreatingSystem(_world, sceneData),
                    new FigureViewCreatingSystem(_world, config, sceneData))
                
                .Add(new ClickEventDispatchingSystem())
                .Add(new ClickProcessingSystem())
                .DelHere<ClickEvent>()
                
                .Add(new FigureDeselectionSystem())
                
                .Add(new FigureMoveSystem())
                
#if UNITY_EDITOR
                .Add (new EcsWorldDebugSystem())
#endif
                
                .Inject(config, sceneData, cam, input)
                .Init ();
        }

        private void Update() 
        {
            _systems?.Run ();
        }

        private void OnDestroy() 
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