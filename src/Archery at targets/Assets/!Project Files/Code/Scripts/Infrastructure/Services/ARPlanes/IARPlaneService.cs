using System;
using System.Collections.ObjectModel;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace Infrastructure.Services.ARPlanes
{
    public interface IARPlanesService
    {
        event Action OnPlaneDetected;

        bool IsPlaneDetected { get; }

        void SetArPlaneManager(ARPlaneManager planeManager);

        ReadOnlyCollection<ARPlane> GetPlanes(PlaneClassifications classification);
    }
}