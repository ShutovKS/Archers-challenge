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

        void AddXRService<T>(T service);
        T GetXRService<T>();
    }

    [UsedImplicitly]
    public class XRSetupService : IXRSetupService
    {
        private readonly IPlayerFactory _playerFactory;
        private readonly Dictionary<Type, object> _xrServices = new();

        public XRSetupService(IPlayerFactory playerFactory)
        {
            _playerFactory = playerFactory;
        }

        public void SetXRMode(XRMode mode)
        {
            GetXRService<ARSession>().enabled = mode switch
            {
                XRMode.AR => true,
                XRMode.None or XRMode.VR => false,
                _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
            };
        }

        public void AddXRService<T>(T service)
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

        public T GetXRService<T>() => (T)_xrServices[typeof(T)];
    }

    public enum XRMode
    {
        None,
        VR,
        AR
    }
}