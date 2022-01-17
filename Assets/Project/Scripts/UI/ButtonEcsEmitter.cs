using System;
using Leopotam.EcsLite;
using Leopotam.EcsLite.ExtendedSystems;
using Project.Infrastructure;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Button))]
    public abstract class ButtonEcsEmitter<T> : MonoBehaviour  where T : struct
    {
        private EcsWorld _world;
        private Button _button;

        private EcsPool<T> _pool;
        private EcsPool<EcsGroupSystemState> _statePool;

        protected virtual void Awake()
        {
            _button = GetComponent<Button>();
            _world = Worlds.Main;
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(OnButtonClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnButtonClick);
        }

        protected abstract void OnButtonClick();

        protected ref T CreateEvent()
        {
            if (_world.IsAlive() == false)
                throw new Exception("Can't create event because world is dead");

            _pool ??= _world.GetPool<T>();
            return ref _pool.Add(_world.NewEntity());
        }

        protected ref EcsGroupSystemState CreateGroupSystemStateEvent()
        {
            if (_world.IsAlive() == false)
                throw new Exception("Can't create event because world is dead");

            _statePool ??= _world.GetPool<EcsGroupSystemState>();
            return ref _statePool.Add(_world.NewEntity());
        }
    }
}