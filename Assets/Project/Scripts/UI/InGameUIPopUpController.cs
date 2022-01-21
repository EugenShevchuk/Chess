using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI
{
    public sealed class InGameUIPopUpController : MonoBehaviour
    {
        [Header("Board creation")]
        [SerializeField] private Button _createBoardButton;
        [SerializeField] private DisplayableUIView BoardCreationDisplayableUI;
        
        [Header("Figures placing")]
        [SerializeField] private Button _placeFiguresButton;
        [SerializeField] private DisplayableUIView FigurePlacingDisplayableUI;

        private void OnEnable()
        {
            _createBoardButton.onClick.AddListener(ShowBoardCreationPopUp);
            _placeFiguresButton.onClick.AddListener(ShowFigurePlacingPopUp);
        }

        private void OnDisable()
        {
            _createBoardButton.onClick.RemoveListener(ShowBoardCreationPopUp);
            _placeFiguresButton.onClick.RemoveListener(ShowFigurePlacingPopUp);
        }

        private void ShowBoardCreationPopUp()
        {
            BoardCreationDisplayableUI.ShowPopUpAsync().Forget();
        }

        private void ShowFigurePlacingPopUp()
        {
            FigurePlacingDisplayableUI.ShowPopUpAsync().Forget();
        }
    }
}