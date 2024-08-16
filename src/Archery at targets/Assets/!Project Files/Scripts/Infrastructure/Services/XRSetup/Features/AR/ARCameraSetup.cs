using System;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using Zenject;

namespace Infrastructure.Services.XRSetup.Features.AR
{
    public class ARCameraSetup : BaseARSetup<ARCameraManager>
    {
        private readonly Camera _camera;
        private readonly ARCameraBackground _arCameraBackground;

        private const float DEFAULT_NEAR_CLIP_PLANE = 0.1f;
        private const float DEFAULT_FAR_CLIP_PLANE = 1000f;
        private static readonly Color DefaultBackgroundColor = Color.black;

        [Inject]
        public ARCameraSetup(Camera camera, ARCameraManager arCameraManager, ARCameraBackground arCameraBackground)
            : base(arCameraManager)
        {
            _camera = camera ?? throw new ArgumentNullException(nameof(camera));
            _arCameraBackground = arCameraBackground ?? throw new ArgumentNullException(nameof(arCameraBackground));
        }

        public override ARFeature Feature => ARFeature.Camera;

        public override void Enable(bool enable)
        {
            SetupCamera(
                enable ? CameraClearFlags.SolidColor : CameraClearFlags.Skybox,
                DefaultBackgroundColor,
                DEFAULT_NEAR_CLIP_PLANE,
                DEFAULT_FAR_CLIP_PLANE
            );

            base.Enable(enable);
            _arCameraBackground.enabled = enable;
        }

        private void SetupCamera(CameraClearFlags clearFlags, Color backgroundColor, float nearClipPlane,
            float farClipPlane)
        {
            _camera.clearFlags = clearFlags;
            _camera.backgroundColor = backgroundColor;
            _camera.nearClipPlane = nearClipPlane;
            _camera.farClipPlane = farClipPlane;
        }
    }
}