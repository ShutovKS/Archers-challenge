using System.Collections.Generic;
using System.Linq;
using Infrastructure.Services.XRSetup.AR;
using Zenject;

namespace Infrastructure.Services.XRSetup
{
    public class XRSetupService : IXRSetupService
    {
        private readonly Dictionary<ARFeature, IARSetup> _arSetups;

        [Inject]
        public XRSetupService(List<IARSetup> arSetups)
        {
            _arSetups = arSetups.ToDictionary(setup => setup.Feature);
        }

        public void EnableFeature(ARFeature feature, bool enable)
        {
            if (_arSetups.TryGetValue(feature, out var arSetup))
            {
                arSetup.Enable(enable);
            }
        }

        public bool IsFeatureEnabled(ARFeature feature)
        {
            return _arSetups.TryGetValue(feature, out var arSetup) && arSetup.IsEnabled();
        }
    }
}