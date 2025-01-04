#region

using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

#endregion

namespace Features.HandMenu
{
    [RequireComponent(typeof(UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor))]
    public class EnableDisableHandMenu : MonoBehaviour
    {
        [SerializeField] private GameObject handMenu;

        private UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor _interactor;

        private void Awake()
        {
            _interactor = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor>();
        }

        private void OnEnable()
        {
            _interactor.hoverEntered.AddListener(EnableMenu);
            _interactor.hoverExited.AddListener(DisableMenu);

            handMenu.SetActive(false);
        }

        private void OnDisable()
        {
            _interactor.hoverEntered.RemoveListener(EnableMenu);
            _interactor.hoverExited.RemoveListener(DisableMenu);

            handMenu.SetActive(false);
        }

        private void EnableMenu(HoverEnterEventArgs arg0)
        {
            handMenu.SetActive(true);
        }

        private void DisableMenu(HoverExitEventArgs arg0)
        {
            handMenu.SetActive(false);
        }
    }
}