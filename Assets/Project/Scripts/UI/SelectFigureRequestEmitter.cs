using Project.Components;
using Project.Infrastructure.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Button))]
    [RequireComponent(typeof(Image))]
    public sealed class SelectFigureRequestEmitter : ButtonEcsEmitter<SelectFigureRequest>
    {
        [SerializeField] private Image _figureImage;
        [SerializeField] private FigureType _type;
        [SerializeField] private Sprite _whiteFigureSprite;
        [SerializeField] private Sprite _blackFigureSprite;

        private Team _team;

        public void ChangeTeam(Team team)
        {
            _team = team;
            UpdateView(team);
        }

        protected override void OnButtonClick()
        {
            ref var evt = ref CreateEvent();
            evt.Team = _team;
            evt.Type = _type;
        }

        private void UpdateView(Team team)
        {
            _figureImage.sprite = team == Team.White ? _whiteFigureSprite : _blackFigureSprite;
        }
    }
}