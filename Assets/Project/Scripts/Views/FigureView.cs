using Leopotam.EcsLite;
using Project.Infrastructure.Enums;
using UnityEngine;

namespace Project.Views
{
    public sealed class FigureView : MonoBehaviour, IObjectView
    {
        public EcsPackedEntity Entity;
        public FigureType Type;
    }
}