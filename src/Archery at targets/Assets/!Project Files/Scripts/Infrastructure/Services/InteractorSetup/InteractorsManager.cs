using UnityEngine;

namespace Infrastructure.Services.InteractorSetup
{
    public class InteractorsManager : MonoBehaviour
    {
        [SerializeField] private InteractorsContainer interactorsContainerLeft;
        [SerializeField] private InteractorsContainer interactorsContainerRight;

        public void SetInteractorLeft(InteractorType interactorType)
        {
            interactorsContainerLeft.SetInteractor(interactorType);
        }

        public void SetInteractorRight(InteractorType interactorType)
        {
            interactorsContainerRight.SetInteractor(interactorType);
        }
    }
}