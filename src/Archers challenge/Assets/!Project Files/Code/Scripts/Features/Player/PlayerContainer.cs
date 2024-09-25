using UnityEngine;

namespace Features.Player
{
    public class PlayerContainer : MonoBehaviour
    {
        [field: SerializeField] public Transform HandMenuSpawnPoint { get; private set; }
    }
}