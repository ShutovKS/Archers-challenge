using UnityEngine;

namespace Data.SceneContext
{
    public abstract class BaseSceneContextData : MonoBehaviour
    {
        [field: Header("Player")]
        [field: SerializeField] public Transform PlayerSpawnPoint { get; private set; }
    }
}