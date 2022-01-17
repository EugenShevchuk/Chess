using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Project.Infrastructure.Enums;
using Project.Views;
using UnityEngine;

namespace Project.Infrastructure
{
    [CreateAssetMenu(menuName = "Configuration/Main")]
    internal sealed class Configuration : ScriptableObject
    {
        [Header("Board data")]
        public TileView TilePrefab;
        public float TileYSize;
        public float TimeToCreateTile;
        public int NumTilesCreatingAtOnce;
        public Ease TileEaseType;
        public Color WhiteTileColor;
        public Color BlackTileColor;

        [Header("Figures data")]
        public float FigureYOffset;
        public Color WhiteFigureColor;
        public Color BlackFigureColor;
        [SerializeField] private FigureView[] _figures;
        private Dictionary<FigureType, FigureView> _figureMap;
        public Dictionary<FigureType, FigureView> FigureMap 
        {
            get { return _figureMap ??= _figures.ToDictionary(view => view.Type);}
        }

        [Header("Figure Movement")] 
        public float Speed = 3f;
        public float StandardFlyUpHeight = .5f;
        public float StandardFlyUpTime = .5f;
        public float KnightFlyUpHeight = 2.5f;
        public float KnightFlyUpTime = 1.25f;
        public AnimationCurve FlyUpCurve;
        public AnimationCurve FlyCurve;
        public AnimationCurve FlyDownCurve;
    }
}