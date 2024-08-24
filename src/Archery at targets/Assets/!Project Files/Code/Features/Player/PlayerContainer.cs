using UnityEngine;

namespace Features.Player
{
    public class PlayerContainer : MonoBehaviour
    {
        [field: SerializeField] public GameObject Player { get; private set; }
        
        [field: SerializeField] public Camera Camera { get; private set; }
        public GameObject CameraGameObject => Camera.gameObject;
        
        [field: SerializeField] public Transform HandMenuSpawnPoint { get; private set; }
    }
}