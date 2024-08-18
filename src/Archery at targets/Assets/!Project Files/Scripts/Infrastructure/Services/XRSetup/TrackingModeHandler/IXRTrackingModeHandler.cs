using System.Threading.Tasks;
using Infrastructure.Services.XRSetup.TrackingMode;

namespace Infrastructure.Services.XRSetup.TrackingModeHandler
{
    public interface IXRTrackingModeHandler
    {
        void Disable();
        Task Enable(IXRTrackingMode trackingMode);
    }
}