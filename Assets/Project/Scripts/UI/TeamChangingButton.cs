using Project.Infrastructure.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Button))]
    public sealed class TeamChangingButton : MonoBehaviour
    {
        [SerializeField] private Team _team;
        [SerializeField] private SelectFigureRequestEmitter[] _emitters;

        [SerializeField] private TextMeshProUGUI _teamNameText;
        [SerializeField] private TextMeshProUGUI _teamText;
        
        private Image _image;
        
        
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _image = GetComponent<Image>();
            
            SetTeam();
            UpdateView();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(ChangeTeam);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(ChangeTeam);
        }

        private void ChangeTeam()
        {
            _team = _team == Team.White ? Team.Black : Team.White;
            
            UpdateView();
            SetTeam();
        }

        private void SetTeam()
        {
            foreach (var emitter in _emitters)
                emitter.ChangeTeam(_team);
        }

        private void UpdateView()
        {
            if (_team == Team.White)
                SetViewToWhiteTeam();
            else
                SetViewToBlackTeam();
        }

        private void SetViewToBlackTeam()
        {
            _teamNameText.text = "BLACK";
            _teamNameText.color = Color.white;
            _teamText.color = Color.white;
            _image.color = Color.black;
        }

        private void SetViewToWhiteTeam()
        {
            _teamNameText.text = "WHITE";
            _teamNameText.color = Color.black;
            _teamText.color = Color.black;
            _image.color = Color.white;
        }
    }
}