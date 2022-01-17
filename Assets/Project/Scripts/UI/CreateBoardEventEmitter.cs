using Cysharp.Threading.Tasks;
using Project.Components;
using Project.Infrastructure;
using TMPro;
using UnityEngine;

namespace Project.UI
{
    public sealed class CreateBoardEventEmitter : ButtonEcsEmitter<CreateBoardRequest>
    {
        [SerializeField] private TMP_InputField _widthInput;
        [SerializeField] private TMP_InputField _lenghtInput;

        private PopUpView _popUp;

        protected override void Awake()
        {
            base.Awake();
            _popUp = GetComponentInParent<PopUpView>();
        }

        protected override void OnButtonClick()
        {
            OnButtonClickAsync().Forget();
        }

        private async UniTask OnButtonClickAsync()
        {
            await _popUp.HidePopUpAsync();
            
            RequestBoardCreation();
        }

        private void RequestBoardCreation()
        {
            EnableBoardGenerationSystemsGroup();
            
            ref var evt = ref CreateEvent();

            var x = int.Parse(_widthInput.text);
            var y = int.Parse(_lenghtInput.text);

            evt.Size = new Vector2Int(x, y);
        }

        private void EnableBoardGenerationSystemsGroup()
        {
            ref var evt = ref CreateGroupSystemStateEvent();
            evt.Name = SystemGroups.BOARD_GENERATION;
            evt.State = true;
        }
    }
}