using Leopotam.EcsLite;
using Project.Infrastructure.Enums;
using UnityEngine;

namespace Project.Views
{
    public sealed class FigureView : MonoBehaviour, IObjectView
    {
        [SerializeField] private SpriteRenderer _figureImage;
        [SerializeField] private Sprite _whiteFigureSprite;
        [SerializeField] private Sprite _blackFigureSprite;
        
        public EcsPackedEntity Entity;
        public FigureType Type;

        public void SetFigureImage(Team team)
        {
            _figureImage.sprite = team == Team.White ? _whiteFigureSprite : _blackFigureSprite;
        }
    }
}