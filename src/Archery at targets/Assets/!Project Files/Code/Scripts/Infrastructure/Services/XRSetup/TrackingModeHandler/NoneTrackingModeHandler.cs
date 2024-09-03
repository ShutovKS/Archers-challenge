#region

using Infrastructure.Services.XRSetup.TrackingMode;

#endregion

namespace Infrastructure.Services.XRSetup.TrackingModeHandler
{
    public class NoneTrackingModeHandler : IXRTrackingModeHandler
    {
        public void Disable()
        {
        }

        public void Enable(IXRTrackingMode trackingMode)
        {
        }
    }
}