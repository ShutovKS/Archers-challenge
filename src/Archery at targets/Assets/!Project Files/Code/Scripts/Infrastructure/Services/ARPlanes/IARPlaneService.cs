using System;
using System.Collections.ObjectModel;
using UnityEngine.XR.ARFoundation;

namespace Infrastructure.Services.ARPlanes
{
    public interface IARPlanesService
    {
        event Action OnPlaneDetected;

        bool IsPlaneDetected { get; }

        void SetArPlaneManager(ARPlaneManager planeManager);

        ReadOnlyCollection<UnityEngine.XR.ARFoundation.ARPlane> Planes { get; }
    }
}