using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MainMenu
{
    public class MainMenuUI : MonoBehaviour
    {
        public event Action OnInfiniteVRClicked, OnInfiniteMRClicked, OnStoreClicked, OnExitClicked;

        [SerializeField] private Button infiniteVR, infiniteMR, store, exit;

        private void Awake()
        {
            infiniteVR.onClick.AddListener(() => OnInfiniteVRClicked?.Invoke());
            infiniteMR.onClick.AddListener(() => OnInfiniteMRClicked?.Invoke());
            store.onClick.AddListener(() => OnStoreClicked?.Invoke());
            exit.onClick.AddListener(() => OnExitClicked?.Invoke());
        }

        private void OnDestroy()
        {
            infiniteVR.onClick.RemoveAllListeners();
            infiniteMR.onClick.RemoveAllListeners();
            store.onClick.RemoveAllListeners();
            exit.onClick.RemoveAllListeners();
        }
    }
}