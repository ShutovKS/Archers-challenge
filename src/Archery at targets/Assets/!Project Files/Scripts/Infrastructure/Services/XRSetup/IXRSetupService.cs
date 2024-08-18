using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine.XR.ARFoundation;

namespace Infrastructure.Services.XRSetup
{
    public interface IXRSetupService
    {
        void SetXRMode(XRMode mode);
        void AddXRComponent<T>(T service) where T : class;
        void RemoveXRComponent<T>() where T : class;
        T GetXRComponent<T>() where T : class;
    }

    [UsedImplicitly]
    public class XRSetupService : IXRSetupService
    {
        private readonly Dictionary<Type, object> _xrComponents = new();

        public void SetXRMode(XRMode mode)
        {
            var arSession = GetXRComponent<ARSession>();
            if (arSession)
            {
                arSession.enabled = mode == XRMode.MR;
            }
            else
            {
                throw new InvalidOperationException("ARSession component not found. Please ensure ARSession is " +
                                                    "created and added to XRSetupService.");
            }
        }

        public void AddXRComponent<T>(T component) where T : class
        {
            if (_xrComponents.ContainsKey(typeof(T)))
            {
                _xrComponents[typeof(T)] = component;
            }
            else
            {
                _xrComponents.Add(typeof(T), component);
            }
        }

        public void RemoveXRComponent<T>() where T : class
        {
            _xrComponents.Remove(typeof(T));
        }

        public T GetXRComponent<T>() where T : class
        {
            _xrComponents.TryGetValue(typeof(T), out var component);
            return component as T;
        }
    }

    public enum XRMode
    {
        None,
        VR,
        MR
    }
}