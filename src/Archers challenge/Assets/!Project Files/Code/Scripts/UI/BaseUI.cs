using UnityEngine;

namespace UI
{
    public abstract class BaseUI : MonoBehaviour
    {
        [SerializeField] protected Canvas canvas;

        public virtual void Show() => canvas.enabled = true;

        public virtual void Hide() => canvas.enabled = false;
    }
}