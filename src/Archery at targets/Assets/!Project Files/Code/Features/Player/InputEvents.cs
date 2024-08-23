using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.Player
{
    public class InputEvents : MonoBehaviour
    {
        public Shortcut exitShortcut;

        private void OnEnable()
        {
            exitShortcut.Enable();
        }

        private void OnDisable()
        {
            exitShortcut.Disable();
        }

        private void OnDestroy()
        {
            exitShortcut.Disable();
        }
    }

    [Serializable]
    public class Shortcut
    {
        public event Action OnTriggered;

        [SerializeField] private InputActionReference[] actions;

        private bool _isEnabled;

        public void Enable()
        {
            if (_isEnabled) return;

            foreach (var actionReference in actions)
            {
                actionReference.action.performed += CheckTriggered;
            }

            _isEnabled = true;
        }

        public void Disable()
        {
            if (!_isEnabled) return;

            foreach (var actionReference in actions)
            {
                actionReference.action.performed -= CheckTriggered;
            }

            _isEnabled = false;
        }

        private void CheckTriggered(InputAction.CallbackContext callbackContext)
        {
            if (actions.All(actionReference => actionReference.action.triggered))
            {
                OnTriggered?.Invoke();
            }
        }
    }
}