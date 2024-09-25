using Features.TargetsInLevelManager;

namespace Infrastructure.Providers.Contexts.TargetsInLevelManage
{
    public interface ITargetsInLevelManagerContextProvider
    {
        ITargetsInLevelManager Get();

        void Register(ITargetsInLevelManager manager);
        void Unregister(ITargetsInLevelManager manager);
    }

    public class TargetsInLevelManagerContextProvider : ITargetsInLevelManagerContextProvider
    {
        private ITargetsInLevelManager _targetsInLevelManager;

        public ITargetsInLevelManager Get() => _targetsInLevelManager;

        public void Register(ITargetsInLevelManager manager) => _targetsInLevelManager = manager;

        public void Unregister(ITargetsInLevelManager manager)
        {
            if (_targetsInLevelManager == manager) _targetsInLevelManager = null;
        }
    }
}