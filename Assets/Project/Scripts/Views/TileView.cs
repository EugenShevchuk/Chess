using Leopotam.EcsLite;
using UnityEngine;

namespace Project.Views
{
    public sealed class TileView : MonoBehaviour, IObjectView
    {
        public EcsPackedEntity Entity;
    }
}