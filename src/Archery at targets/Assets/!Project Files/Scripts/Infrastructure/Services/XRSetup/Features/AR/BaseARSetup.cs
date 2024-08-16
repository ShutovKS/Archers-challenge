using System;
using UnityEngine;

namespace Infrastructure.Services.XRSetup.Features.AR
{
    public abstract class BaseARSetup<TManager> : IARSetup where TManager : Behaviour
    {
        protected readonly TManager Manager;

        protected BaseARSetup(TManager manager)
        {
            Manager = manager ?? throw new ArgumentNullException(nameof(manager));
        }

        public abstract ARFeature Feature { get; }

        public virtual void Enable(bool enable)
        {
            Manager.enabled = enable;
        }

        public bool IsEnabled()
        {
            return Manager.enabled;
        }
    }
}