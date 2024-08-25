using Infrastructure.Services.XRSetup.TrackingMode;

namespace Infrastructure.Services.XRSetup.TrackingModeHandler
{
    public interface IXRTrackingModeHandler
    {
        void Disable();
        void Enable(IXRTrackingMode trackingMode);
    }
}