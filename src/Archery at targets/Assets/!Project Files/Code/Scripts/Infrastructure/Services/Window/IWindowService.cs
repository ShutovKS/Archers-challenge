#region

using System.Threading.Tasks;
using UnityEngine;

#endregion

namespace Infrastructure.Services.Window
{
    public interface IWindowService
    {
        Task OpenInWorld(WindowID windowID, Vector3 position, Quaternion rotation, Transform transform);

        Task<T> OpenInWorldAndGet<T>(WindowID windowID, Vector3 position, Quaternion rotation,
            Transform transform = null) where T : Component;

        T Get<T>(WindowID windowID) where T : Component;
        void Close(WindowID windowID);
    }
}