#region

using Zenject;

#endregion

namespace Infrastructure.Installers
{
    public class GameBootstrapInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<GamingBootloader>().AsSingle().NonLazy();

            Container.Bind<IInitializable>().To<GamingBootloader>().FromResolve();
        }
    }
}