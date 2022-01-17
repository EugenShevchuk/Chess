using UnityEngine;

namespace Project.Infrastructure
{
    [CreateAssetMenu(menuName = "Configuration/UI")]
    public sealed class UIConfiguration : ScriptableObject
    {
        [Header("Pop Up")] 
        [Tooltip("How far pop up size will overshoot while showing up, before scaling back to zero vector")]
        [Range(1, 2)] public float OvershootPopUpSize;
        public float ShowingPopUpDuration;
        public AnimationCurve ShowingPopUpCurve;
        public float HidingPopUpDuration;
        public AnimationCurve HidingPopUpCurve;
    }
}