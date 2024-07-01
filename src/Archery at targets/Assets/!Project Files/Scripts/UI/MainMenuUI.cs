using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] private Button vrGameButton;
        [SerializeField] private Button mrGameButton;
        [SerializeField] private Button exitButton;

        public event Action OnVrGameButtonClicked;
        public event Action OnMrGameButtonClicked;
        public event Action OnExitButtonClicked;

        private void OnEnable()
        {
            vrGameButton.onClick.AddListener(() => OnVrGameButtonClicked?.Invoke());
            mrGameButton.onClick.AddListener(() => OnMrGameButtonClicked?.Invoke());
            exitButton.onClick.AddListener(() => OnExitButtonClicked?.Invoke());
        }

        private void OnDisable()
        {
            vrGameButton.onClick.RemoveAllListeners();
            mrGameButton.onClick.RemoveAllListeners();
            exitButton.onClick.RemoveAllListeners();
        }

        private void Awake()
        {
            OnVrGameButtonClicked += () => Debug.Log("VR Game Button Clicked");
            OnMrGameButtonClicked += () => Debug.Log("MR Game Button Clicked");
            OnExitButtonClicked += () => Debug.Log("Exit Button Clicked");
        }
    }
}