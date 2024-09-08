using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Infrastructure.Services.Camera
{
    [UsedImplicitly]
    public class CameraService : ICameraService
    {
        public GameObject Camera { get; private set; }
        public Vector3 CameraPosition => Camera.transform.position;
        private UnityEngine.Camera _camera;

        public void SetCamera(UnityEngine.Camera camera)
        {
            Camera = camera.gameObject;
            _camera = camera;
        }

        public void SetBackgroundType(CameraBackgroundType backgroundType, object parameter = null)
        {
            CheckCamera();

            switch (backgroundType)
            {
                case CameraBackgroundType.Skybox:
                    SetSkyboxBackground();
                    break;
                case CameraBackgroundType.SolidColor:
                    var backgroundColor = parameter == null ? Color.black : (Color)parameter;
                    SetSolidColorBackground(backgroundColor);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(backgroundType), backgroundType,
                        "Unsupported background type.");
            }
        }

        private void SetSkyboxBackground()
        {
            _camera.clearFlags = CameraClearFlags.Skybox;
        }

        private void SetSolidColorBackground(Color backgroundColor)
        {
            _camera.clearFlags = CameraClearFlags.SolidColor;
            _camera.backgroundColor = backgroundColor;
        }

        private void CheckCamera()
        {
            if (_camera == null)
            {
                Debug.LogError("Camera is not set. Please call SetCamera() before setting background.");
            }
        }
    }
}