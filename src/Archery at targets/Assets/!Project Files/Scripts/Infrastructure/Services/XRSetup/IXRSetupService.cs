using System.Threading.Tasks;
using Infrastructure.Services.XRSetup.TrackingMode;
using UnityEngine;

namespace Infrastructure.Services.XRSetup
{
    public interface IXRSetupService
    {
        void Initialize();
        void SetXRMode(XRMode mode);
        void SetXRTrackingMode(IXRTrackingMode xrTrackingMode);
        void SetAnchorManagerState(bool isAnchorManagerEnabled, GameObject anchorPrefab = null);
    }
}