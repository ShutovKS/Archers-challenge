using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace Features.HandMenu
{
    [RequireComponent(typeof(XRBaseInteractor))]
    public class EnableDisableHandMenu : MonoBehaviour
    {
        [SerializeField] private GameObject handMenu;

        private XRBaseInteractor _interactor;

        private void Awake()
        {
            _interactor = GetComponent<XRBaseInteractor>();
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