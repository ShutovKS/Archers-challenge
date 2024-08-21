using System;
using UnityEngine;

namespace Infrastructure.Services.InteractorSetup
{
    [Serializable]
    public class InteractorsContainer
    {
        [SerializeField] private GameObject directInteractor;
        [SerializeField] private GameObject gazeInteractor;
        [SerializeField] private GameObject nearFarInteractor;
        [SerializeField] private GameObject pokeInteractor;
        [SerializeField] private GameObject rayInteractor;
        [SerializeField] private GameObject socketInteractor;

        public void SetInteractor(InteractorType interactorType)
        {
            if (directInteractor) directInteractor.SetActive(interactorType.HasFlag(InteractorType.Direct));
            if (gazeInteractor) gazeInteractor.SetActive(interactorType.HasFlag(InteractorType.Gaze));
            if (nearFarInteractor) nearFarInteractor.SetActive(interactorType.HasFlag(InteractorType.NearFar));
            if (pokeInteractor) pokeInteractor.SetActive(interactorType.HasFlag(InteractorType.Poke));
            if (rayInteractor) rayInteractor.SetActive(interactorType.HasFlag(InteractorType.Ray));
            if (socketInteractor) socketInteractor.SetActive(interactorType.HasFlag(InteractorType.Socket));
        }
    }
}