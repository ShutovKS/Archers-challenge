using Zenject;

namespace Infrastructure.Factories.ARTrackingMode
{
    public interface IARTrackingModeFactory
    {
        IARTrackingMode Create<T>() where T : IARTrackingMode;
    }

    public class ARTrackingModeFactory : IARTrackingModeFactory
    {
        private readonly DiContainer _container;

        [Inject]
        public ARTrackingModeFactory(DiContainer container)
        {
            _container = container;
        }

        public IARTrackingMode Create<T>() where T : IARTrackingMode => _container.Instantiate<T>();
    }
}