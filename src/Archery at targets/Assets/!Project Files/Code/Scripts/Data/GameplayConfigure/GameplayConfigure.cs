using Infrastructure.Services.InteractorSetup;
using Infrastructure.Services.XRSetup;
using Infrastructure.Services.XRSetup.TrackingMode;

namespace Data.GameplayConfigure
{
    public class GameplayConfigure
    {
        public XRMode XRMode { get; set; }
        public GameplayType GameplayType { get; set; }
        public IXRTrackingMode XRTrackingMode { get; set; }
        public InteractorType InteractorType { get; set; }
    }
}