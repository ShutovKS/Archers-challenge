using Zenject;

namespace Infrastructure.Factories.ARTrackingMode
{
    public interface IArTrackingModeFactory
    {
        IArTrackingMode Create<T>() where T : IArTrackingMode;
    }

    public class ArTrackingModeFactory : IArTrackingModeFactory
    {
        private readonly DiContainer _container;

        [Inject]
        public ArTrackingModeFactory(DiContainer container)
        {
            _container = container;
        }

        public IArTrackingMode Create<T>() where T : IArTrackingMode => _container.Instantiate<T>();
    }
}