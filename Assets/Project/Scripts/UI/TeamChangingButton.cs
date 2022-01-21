using Project.Infrastructure.Enums;
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
        
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
            SetTeam();
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
            
            SetTeam();
        }

        private void SetTeam()
        {
            foreach (var emitter in _emitters)
                emitter.ChangeTeam(_team);
        }
    }
}