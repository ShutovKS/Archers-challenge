using System;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace Infrastructure.Services.XRSetup.TrackingMode
{
    public class MeshTrackingMode : IXRTrackingMode
    {
        public MeshTrackingMode(MeshFilter meshPrefab = null)
        {
            _meshPrefab = meshPrefab;
        }

        private readonly MeshFilter _meshPrefab;

        public void ConfigureComponents(params Behaviour[] components)
        {
            foreach (var component in components)
            {
                if (component is ARMeshManager arMeshManager)
                {
                    arMeshManager.meshPrefab = _meshPrefab;
                }
            }
        }
    }
}