using System.Threading.Tasks;
using Infrastructure.Services.XRSetup.TrackingMode;

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