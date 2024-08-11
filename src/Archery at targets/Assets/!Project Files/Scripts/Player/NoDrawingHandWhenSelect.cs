using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace Player
{
    [RequireComponent(typeof(XRBaseInteractor))]
    public class NoDrawingHandWhenSelect : MonoBehaviour
    {
        [SerializeField] private GameObject drawingHand;

        private XRBaseInteractor _interactor;

        private void Awake()
        {
            _interactor = GetComponent<XRBaseInteractor>();
        }

        private void OnEnable()
        {
            _interactor.selectEntered.AddListener(OnSelectEntered);
            _interactor.selectExited.AddListener(OnSelectExited);
        }

        private void OnDisable()
        {
            _interactor.selectEntered.RemoveListener(OnSelectEntered);
            _interactor.selectExited.RemoveListener(OnSelectExited);
        }

        private void OnSelectEntered(SelectEnterEventArgs args) => drawingHand.SetActive(false);
        private void OnSelectExited(SelectExitEventArgs args) => drawingHand.SetActive(true);
    }
}