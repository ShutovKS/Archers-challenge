using System;
using UnityEngine;

namespace Infrastructure.Services.XRSetup.TrackingMode
{
    public class NoneTrackingMode : IXRTrackingMode
    {
        public void ConfigureComponents(params Behaviour[] components)
        {
            // Do nothing
        }
    }
}