using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace Infrastructure.Services.InteractorSetup
{
    [Serializable]
    public class InteractorsContainer
    {
        [SerializeField] private GameObject directInteractor;
        [SerializeField] private GameObject nearFarInteractor;
        [SerializeField] private GameObject pokeInteractor;
        [SerializeField] private GameObject rayInteractor;

        public void SetInteractor(InteractorType interactorType)
        {
            if (directInteractor) directInteractor.SetActive(interactorType.HasFlag(InteractorType.Direct));
            if (nearFarInteractor) nearFarInteractor.SetActive(interactorType.HasFlag(InteractorType.NearFar));
            if (pokeInteractor) pokeInteractor.SetActive(interactorType.HasFlag(InteractorType.Poke));
            if (rayInteractor) rayInteractor.SetActive(interactorType.HasFlag(InteractorType.Ray));
        }
    }
}