using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Project.UI
{
    public enum DisplayMethod
    {
        ScaleUp,
        FlyFromBottom,
        FlyFromLeft,
        FlyFromRight,
        FlyFromTop,
    }

    public class DisplayableUIView : MonoBehaviour
    {
        [SerializeField] private DisplayMethod _displayMethod;
        [SerializeField] private Ease _ease;
        [SerializeField] private float _timeToAnimate;
        [SerializeField] private bool _isActiveByDefault;

        private RectTransform _transform;

        private Vector3 _startPosition;
        private Vector3 _hiddenPosition;
    
        private void Awake()
        {
            _transform = GetComponent<RectTransform>();

            if (_displayMethod == DisplayMethod.ScaleUp)
            {
                if (_isActiveByDefault)
                    return;
                
                transform.localScale = Vector3.zero;
                
                if (gameObject.activeInHierarchy)
                    gameObject.SetActive(false);
            }
            else
            {
                _startPosition = transform.position;

                switch (_displayMethod)
                {
                    case DisplayMethod.FlyFromBottom:
                        _hiddenPosition = GetHiddenPositionBottom();
                        break;
                    case DisplayMethod.FlyFromLeft:
                        _hiddenPosition = GetHiddenPositionLeft();
                        break;
                    case DisplayMethod.FlyFromRight:
                        _hiddenPosition = GetHiddenPositionRight();
                        break;
                    case DisplayMethod.FlyFromTop:
                        _hiddenPosition = GetHiddenPositionTop();
                        break;
                }
                
                if (_isActiveByDefault)
                    return;

                transform.position = _hiddenPosition;
                
                if (gameObject.activeInHierarchy)
                    gameObject.SetActive(false);
            }
        }

        public async UniTask ShowPopUpAsync()
        {
            gameObject.SetActive(true);
            gameObject.SetActive(true);

            switch (_displayMethod)
            {
                case DisplayMethod.ScaleUp:
                    await ScaleIn();
                    break;
                case DisplayMethod.FlyFromBottom:
                    await FlyIn();
                    break;
                case DisplayMethod.FlyFromLeft:
                    await FlyIn();
                    break;
                case DisplayMethod.FlyFromRight:
                    await FlyIn();
                    break;
                case DisplayMethod.FlyFromTop:
                    await FlyIn();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public async UniTask HidePopUpAsync()
        {
            switch (_displayMethod)
            {
                case DisplayMethod.ScaleUp:
                    await ScaleOut();
                    break;
                case DisplayMethod.FlyFromBottom:
                    await FlyOut();
                    break;
                case DisplayMethod.FlyFromLeft:
                    await FlyOut();
                    break;
                case DisplayMethod.FlyFromRight:
                    await FlyOut();
                    break;
                case DisplayMethod.FlyFromTop:
                    await FlyOut();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            gameObject.SetActive(false);
        }

        private async UniTask FlyIn()
        {
            await transform.DOMove(_startPosition, _timeToAnimate)
                .SetEase(_ease)
                .AsyncWaitForCompletion()
                .AsUniTask();
        }

        private async UniTask FlyOut()
        {
            await transform.DOMove(_hiddenPosition, _timeToAnimate)
                .SetEase(_ease)
                .AsyncWaitForCompletion()
                .AsUniTask();
        }

        private async UniTask ScaleIn()
        {
            await transform.DOScale(Vector3.one, _timeToAnimate)
                .SetEase(_ease)
                .AsyncWaitForCompletion()
                .AsUniTask();
        }

        private async UniTask ScaleOut()
        {
            await transform.DOScale(Vector3.zero, _timeToAnimate)
                .SetEase(_ease)
                .AsyncWaitForCompletion()
                .AsUniTask();
        }

        private Vector3 GetHiddenPositionLeft()
        {
            var position = transform.position;
            
            position.x -= Screen.width;
            return position;
        }

        private Vector3 GetHiddenPositionRight()
        {
            var position = transform.position;
            position.x += Screen.width;
            return position;
        }

        private Vector3 GetHiddenPositionTop()
        {
            var position = transform.position;
            position.y += Screen.height;
            return position;
        }

        private Vector3 GetHiddenPositionBottom()
        {
            var position = transform.position;
            position.y -= Screen.height;
            return position;
        }
    }
}