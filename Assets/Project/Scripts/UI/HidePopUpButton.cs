using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Button))]
    public sealed class HidePopUpButton : MonoBehaviour
    {
        private PopUpView _popUp;
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _popUp = GetComponentInParent<PopUpView>();
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
            _popUp.HidePopUpAsync().Forget();
        }
    }
}