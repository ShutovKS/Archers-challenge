using System.Threading.Tasks;
using Infrastructure.Services.XRSetup.TrackingMode;

namespace Infrastructure.Services.XRSetup.TrackingModeHandler
{
    public class NoneTrackingModeHandler : IXRTrackingModeHandler
    {
        public void Disable()
        {
        }

        public Task Enable(IXRTrackingMode trackingMode) => Task.CompletedTask;
    }
}