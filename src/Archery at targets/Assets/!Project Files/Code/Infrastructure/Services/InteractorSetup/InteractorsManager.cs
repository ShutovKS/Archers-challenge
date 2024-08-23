using UnityEngine;

namespace Infrastructure.Services.InteractorSetup
{
    public class InteractorsManager : MonoBehaviour
    {
        [SerializeField] private InteractorsContainer interactorsContainerLeft;
        [SerializeField] private InteractorsContainer interactorsContainerRight;
        [SerializeField] private GameObject gazeInteractor;
        
        public void SetGazeInteractor(bool isActive)
        {
            if (gazeInteractor) gazeInteractor.SetActive(isActive);
        }
        
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