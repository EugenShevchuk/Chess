using Project.Components;
using Project.Infrastructure.Enums;
using UnityEngine;

namespace Project.UI
{
    public sealed class SelectFigureRequestEmitter : ButtonEcsEmitter<SelectFigureRequest>
    {
        [SerializeField] private FigureType _type;
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
            
        }
    }
}