using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.HandMenu
{
    public class HandMenuUI : MonoBehaviour
    {
        public event Action OnExitButtonClicked;

        [SerializeField] private Button exitButton;

        private void OnEnable()
        {
            exitButton.onClick.AddListener(ExitButtonClicked);
        }

        private void OnDisable()
        {
            exitButton.onClick.RemoveListener(ExitButtonClicked);
        }

        private void ExitButtonClicked()
        {
            OnExitButtonClicked?.Invoke();
        }
    }
}