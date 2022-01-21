using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Button))]
    public sealed class ShowPopUpButton : MonoBehaviour
    {
        [SerializeField] private DisplayableUIView DisplayableUI;
        
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
            DisplayableUI.ShowPopUpAsync().Forget();
        }
    }
}