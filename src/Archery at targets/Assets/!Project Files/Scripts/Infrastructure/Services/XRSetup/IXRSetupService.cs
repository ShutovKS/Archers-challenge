using System;
using System.Collections.Generic;
using Infrastructure.Factories.Player;
using JetBrains.Annotations;
using UnityEngine.XR.ARFoundation;

namespace Infrastructure.Services.XRSetup
{
    public interface IXRSetupService
    {
        void SetXRMode(XRMode mode);

        void AddXRComponent<T>(T service);
        void RemoveXRComponent<T>();
        T GetXRComponent<T>();
    }

    [UsedImplicitly]
    public class XRSetupService : IXRSetupService
    {
        private readonly Dictionary<Type, object> _xrServices = new();

        public void SetXRMode(XRMode mode)
        {
            GetXRComponent<ARSession>().enabled = mode switch
            {
                XRMode.MR => true,
                XRMode.None or XRMode.VR => false,
                _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
            };
        }

        public void AddXRComponent<T>(T service)
        {
            if (_xrServices.ContainsKey(typeof(T)))
            {
                _xrServices[typeof(T)] = service;
            }
            else
            {
                _xrServices.Add(typeof(T), service);
            }
        }

        public void RemoveXRComponent<T>()
        {
            _xrServices.Remove(typeof(T));
        }

        public T GetXRComponent<T>() => (T)_xrServices[typeof(T)];
    }

    public enum XRMode
    {
        None,
        VR,
        MR
    }
}