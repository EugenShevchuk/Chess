using UnityEngine;

namespace Project.Infrastructure.BoardLayouts
{
    [CreateAssetMenu(menuName = "Layouts/Standard", fileName = "StandardLayout")]
    public sealed class StandardLayout : FigureLayout
    {
        [SerializeField] private TileData[] _tiles;
        
        public override TileData[] Tiles
        {
            get { return _tiles; }
        }
    }
}