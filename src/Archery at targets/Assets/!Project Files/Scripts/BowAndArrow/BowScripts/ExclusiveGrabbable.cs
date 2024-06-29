using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace BowAndArrow.BowScripts
{
    public class ExclusiveGrabbable : MonoBehaviour
    {
        [SerializeField] private XRBaseInteractable interactable;
        [SerializeField] private XRBaseInteractor interactor;

        [SerializeField] private InteractionLayerMask previousAbleLayers = ~0;

        [SerializeField] private InteractionLayerMask previousActorLayers = ~0;
        [SerializeField] private InteractionLayerMask exclusiveLayerMask = 1 << 10;

        public void Exclusive(SelectEnterEventArgs args)
        {
            previousAbleLayers = interactable.interactionLayers;
            interactable.interactionLayers = exclusiveLayerMask;

            interactor = args.interactorObject.transform.GetComponent<XRBaseInteractor>();
            previousActorLayers = interactor.interactionLayers;
            interactor.interactionLayers = exclusiveLayerMask;
        }

        public void Nonexclusive(SelectExitEventArgs args)
        {
            interactable.interactionLayers = previousAbleLayers;

            interactor = args.interactorObject.transform.GetComponent<XRBaseInteractor>();
            interactor.interactionLayers = previousActorLayers;
        }
    }
}