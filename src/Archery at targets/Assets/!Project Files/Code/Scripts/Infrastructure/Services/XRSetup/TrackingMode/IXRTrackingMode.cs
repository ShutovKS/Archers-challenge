#region

using UnityEngine;

#endregion

namespace Infrastructure.Services.XRSetup.TrackingMode
{
    public interface IXRTrackingMode
    {
        void ConfigureComponents(params Behaviour[] components);
    }
}