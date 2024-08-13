using System.Threading.Tasks;
using UnityEngine;

namespace Infrastructure.Services.Window
{
    public interface IWindowService
    {
        Task Open(WindowID windowID);
        Task<T> OpenAndGetComponent<T>(WindowID windowID) where T : Component;
        void Close(WindowID windowID);
    }
}