#region

using System.Threading.Tasks;
using UnityEngine;

#endregion

namespace Infrastructure.Services.Window
{
    public interface IWindowService
    {
        Task Open(WindowID windowID, Vector3? position = null, Quaternion? rotation = null, Transform parent = null);

        Task<T> OpenAndGet<T>(WindowID windowID, Vector3? position = null, Quaternion? rotation = null, Transform parent = null)
            where T : Component;

        T Get<T>(WindowID windowID) where T : Component;
        void Close(WindowID windowID);
    }
}