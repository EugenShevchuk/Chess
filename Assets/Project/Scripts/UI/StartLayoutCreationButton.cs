using System;
using Cysharp.Threading.Tasks;
using Leopotam.EcsLite;
using Leopotam.EcsLite.ExtendedSystems;
using Project.Infrastructure;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Button))]
    public sealed class StartLayoutCreationButton : MonoBehaviour
    {
        [SerializeField] private DisplayableUIView _rightUIView;
        [SerializeField] private DisplayableUIView _layoutCreationBarView;
        private DisplayableUIView _parentView;
        
        private Button _button;
        private EcsWorld _world;

        private EcsPool<EcsGroupSystemState> _statePool;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _parentView = GetComponentInParent<DisplayableUIView>();
            _world = Worlds.Main;
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(StartLayoutCreation);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(StartLayoutCreation);
        }

        private void StartLayoutCreation()
        {
            StartLayoutCreationAsync().Forget();
        }

        private async UniTask StartLayoutCreationAsync()
        {
            var task1 = _parentView.HidePopUpAsync();
            var task2 = _rightUIView.HidePopUpAsync();

            await UniTask.WhenAll(task1, task2);

            await _layoutCreationBarView.ShowPopUpAsync();
            
            EnableLayoutCreationGroup();
        }

        private void EnableLayoutCreationGroup()
        {
            if (_world.IsAlive() == false)
                throw new Exception("Can't create event because world is dead");

            _statePool ??= _world.GetPool<EcsGroupSystemState>();
            ref var evt = ref _statePool.Add(_world.NewEntity());
            evt.Name = SystemGroups.LAYOUT_CREATION;
            evt.State = true;
        }
    }
}