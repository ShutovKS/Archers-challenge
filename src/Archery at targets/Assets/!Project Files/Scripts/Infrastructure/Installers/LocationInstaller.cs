using Features.ShootingGallery;
using UI;
using UnityEngine;
using Zenject;

namespace Infrastructure.Installers
{
    public class LocationInstaller : MonoInstaller
    {
        [SerializeField] private InformationDeskUI informationDeskUI;
        [SerializeField] private TargetSpawner targetSpawner;

        public override void InstallBindings()
        {
            Container.Bind<TargetSpawner>().FromInstance(targetSpawner).AsSingle();
            Container.Bind<InformationDeskUI>().FromInstance(informationDeskUI).AsSingle();
        }
    }
}