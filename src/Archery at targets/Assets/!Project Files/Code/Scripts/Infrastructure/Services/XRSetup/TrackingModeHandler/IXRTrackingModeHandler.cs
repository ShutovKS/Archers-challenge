#region

using Infrastructure.Services.XRSetup.TrackingMode;

#endregion

namespace Infrastructure.Services.XRSetup.TrackingModeHandler
{
    public interface IXRTrackingModeHandler
    {
        void Disable();
        void Enable(IXRTrackingMode trackingMode);
    }
}