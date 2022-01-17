using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Button))]
    public sealed class CreateBoardButton : MonoBehaviour
    {
        [SerializeField] private PopUpView _createBoardPopUp;
        
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(ShowPopUp);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(ShowPopUp);
        }

        private void ShowPopUp()
        {
            _createBoardPopUp.ShowPopUpAsync().Forget();
        }
    }
}