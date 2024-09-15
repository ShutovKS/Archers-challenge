using UnityEngine;

namespace Infrastructure.Services.Camera
{
    public interface ICameraService
    {
        Vector3 CameraPosition { get; }
        GameObject Camera { get; }

        void SetCamera(UnityEngine.Camera camera);
        void SetBackgroundType(CameraBackgroundType backgroundType, object parameter = null);
    }

    public enum CameraBackgroundType
    {
        Skybox,
        SolidColor
    }
}