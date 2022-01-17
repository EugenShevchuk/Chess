using System;
using Project.Infrastructure.Enums;
using UnityEngine;

namespace Project.Infrastructure.BoardLayouts
{
    public abstract class FigureLayout : ScriptableObject
    {
        public abstract TileData[] Tiles { get; }

        [Serializable]
        public struct TileData
        {
            public Vector2Int Position;
            public Team Team;
            public FigureType FigureType;
        }
    }
}