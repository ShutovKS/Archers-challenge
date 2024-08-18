using System.Threading.Tasks;
using Infrastructure.Services.XRSetup.TrackingMode;
using UnityEngine;

namespace Infrastructure.Services.XRSetup
{
    public interface IXRSetupService
    {
        Task SetXRMode(XRMode mode);
        Task SetXRTrackingMode(IXRTrackingMode xrTrackingMode);
        Task SetAnchorManagerState(bool isAnchorManagerEnabled, GameObject anchorPrefab = null);
    }
}