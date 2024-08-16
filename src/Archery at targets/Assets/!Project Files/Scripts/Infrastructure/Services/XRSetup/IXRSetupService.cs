using Infrastructure.Services.XRSetup.AR;

namespace Infrastructure.Services.XRSetup
{
    public interface IXRSetupService
    {
        void EnableFeature(ARFeature feature, bool enable);
        bool IsFeatureEnabled(ARFeature feature);
    }
}