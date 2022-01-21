using System;
using Project.Infrastructure.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Infrastructure.BoardLayouts
{
    [Serializable]
    public abstract class LayoutData
    {
        public Image Image;
        public string Name;
        
        public Element[] Elements;

        [Serializable]
        public struct Element
        {
            public Vector2Int Position;
            public Team Team;
            public FigureType FigureType;
        }
    }
}