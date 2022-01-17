using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Project.Infrastructure;
using UnityEngine;

namespace Project.UI
{
    public class PopUpView : MonoBehaviour
    {
        private UIConfiguration _config;
    
        private void Awake()
        {
            _config = Resources.Load<UIConfiguration>("UIConfiguration");
            
            transform.localScale = Vector3.zero;
        }

        public async UniTask ShowPopUpAsync()
        {
            if (gameObject.activeInHierarchy)
                throw new Exception($"Pop up {gameObject.name} is already active");

            gameObject.SetActive(true);
            
            await transform.DOScale(Vector3.one * _config.OvershootPopUpSize, _config.ShowingPopUpDuration)
                .SetEase(_config.ShowingPopUpCurve)
                .AsyncWaitForCompletion()
                .AsUniTask();
            
            await transform.DOScale(Vector3.one, _config.ShowingPopUpDuration)
                .SetEase(_config.ShowingPopUpCurve)
                .AsyncWaitForCompletion()
                .AsUniTask();
        }

        public async UniTask HidePopUpAsync()
        {
            if (gameObject.activeInHierarchy == false)
                throw new Exception($"Pop up {gameObject.name} is already deactivated");
            
            await transform.DOScale(Vector3.zero * _config.OvershootPopUpSize, _config.ShowingPopUpDuration)
                .SetEase(_config.ShowingPopUpCurve)
                .AsyncWaitForCompletion()
                .AsUniTask();
            
            gameObject.SetActive(false);
        }
    }
}