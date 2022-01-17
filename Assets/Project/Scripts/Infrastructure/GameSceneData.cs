using UnityEngine;

namespace Project.Infrastructure
{
    internal sealed class GameSceneData : MonoBehaviour
    {
        [Header("Figure data")] 
        public Transform WhiteTeamSpawnPoint;
        public Transform BlackTeamSpawnPoint;
        
        [Header("Board data")]
        public Transform Board;
    }
}