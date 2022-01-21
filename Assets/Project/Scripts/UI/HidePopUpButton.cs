using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Button))]
    public sealed class HidePopUpButton : MonoBehaviour
    {
        private DisplayableUIView _displayableUI;
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _displayableUI = GetComponentInParent<DisplayableUIView>();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(HidePopUp);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(HidePopUp);
        }

        private void HidePopUp()
        {
            _displayableUI.HidePopUpAsync().Forget();
        }
    }
}