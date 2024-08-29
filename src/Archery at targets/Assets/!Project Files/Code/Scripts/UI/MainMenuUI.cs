using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MainMenuUI : MonoBehaviour
    {
        public event Action OnInfiniteVRClicked, OnInfiniteMRClicked, OnExitClicked;

        [SerializeField] private Button infiniteVR, infiniteMR, exit;
        
        private void Awake()
        {
            infiniteVR.onClick.AddListener(() => OnInfiniteVRClicked?.Invoke());
            infiniteMR.onClick.AddListener(() => OnInfiniteMRClicked?.Invoke());
            exit.onClick.AddListener(() => OnExitClicked?.Invoke());
        }
        
        private void OnDestroy()
        {
            infiniteVR.onClick.RemoveAllListeners();
            infiniteMR.onClick.RemoveAllListeners();
            exit.onClick.RemoveAllListeners();
        }
    }
}