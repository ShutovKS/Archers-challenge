using UI.HandMenu;
using UnityEngine;

namespace Features.Player
{
    public class PlayerContainer : MonoBehaviour
    {
        [field: SerializeField] public HandMenuUI HandMenuUI { get; private set; }
    }
}