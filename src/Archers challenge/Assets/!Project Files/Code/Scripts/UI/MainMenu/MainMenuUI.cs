#region

using System;
using UnityEngine;
using UnityEngine.UI;

#endregion

namespace UI.MainMenu
{
    public class MainMenuUI : BaseUI
    {
        public event Action OnInfiniteVRClicked, OnInfiniteMRClicked, OnLevelsClicked, OnExitClicked;

        [SerializeField] private Button infiniteVR, infiniteMR, levels, exit;

        private void Awake()
        {
            infiniteVR.onClick.AddListener(() => OnInfiniteVRClicked?.Invoke());
            infiniteMR.onClick.AddListener(() => OnInfiniteMRClicked?.Invoke());
            levels.onClick.AddListener(() => OnLevelsClicked?.Invoke());
            exit.onClick.AddListener(() => OnExitClicked?.Invoke());
        }

        private void Start()
        {
#if LEVEL_DISABLE
            levels.gameObject.SetActive(false);
#endif

#if MR_DISABLE
            infiniteMR.gameObject.SetActive(false);
#endif

#if VR_DISABLE
            infiniteVR.gameObject.SetActive(false);
#endif
        }

        private void OnDestroy()
        {
            infiniteVR.onClick.RemoveAllListeners();
            infiniteMR.onClick.RemoveAllListeners();
            levels.onClick.RemoveAllListeners();
            exit.onClick.RemoveAllListeners();
        }
    }
}