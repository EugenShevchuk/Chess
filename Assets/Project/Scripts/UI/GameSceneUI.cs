using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Project.Infrastructure;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI
{
    public interface IInitializableUI
    {
        public void Init(UIConfiguration configuration);
    }
    
    public sealed class GameSceneUI : MonoBehaviour
    {
        [SerializeField] private UIConfiguration _configuration;
        
        [Header("Pop ups general")]
        [SerializeField] private float _timeToAppear;
        [SerializeField] private AnimationCurve _appearCurve;
        [SerializeField] private float _timeToDisappear;
        [SerializeField] private AnimationCurve _disappearCurve;
        
        [Header("Create board")]
        [SerializeField] private Button _createBoardButton;
        [SerializeField] private RectTransform _createBoardPopUp;

        private IInitializableUI[] _initializable;

        private void Awake()
        {
            _initializable = GetComponentsInChildren<IInitializableUI>();

            foreach (var ui in _initializable)
                ui.Init(_configuration);
        }

        private void OnEnable()
        {
            _createBoardButton.onClick.AddListener(ShowCreateBoardPopUpAsync);
        }

        private async void ShowCreateBoardPopUpAsync()
        {
            _createBoardPopUp.gameObject.SetActive(true);

            await _createBoardPopUp.DOScale(Vector3.one, _timeToAppear)
                .SetEase(_appearCurve)
                .AsyncWaitForCompletion();
        }

        private async UniTask ShowPopUpAsync(RectTransform popUpTransform)
        {
            popUpTransform.gameObject.SetActive(true);
            
            await popUpTransform.DOScale(Vector3.one, _timeToAppear)
                .SetEase(_appearCurve)
                .AsyncWaitForCompletion();
        }

        private async UniTask HidePopUpAsync(RectTransform popUpTransform)
        {
            await popUpTransform.DOScale(Vector3.zero, _timeToDisappear)
                .SetEase(_disappearCurve)
                .AsyncWaitForCompletion();
            
            popUpTransform.gameObject.SetActive(false);
        }
    }
}