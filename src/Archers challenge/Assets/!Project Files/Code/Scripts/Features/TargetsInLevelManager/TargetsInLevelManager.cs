using System;
using Infrastructure.Factories.Target;
using Infrastructure.Providers.Contexts.TargetsInLevelManage;
using UnityEngine;
using Zenject;

namespace Features.TargetsInLevelManager
{
    public abstract class TargetsInLevelManager : MonoBehaviour, ITargetsInLevelManager
    {
        public event Action OnTargetHit;

        public abstract void PrepareTargets();
        public abstract void StartTargets();
        public abstract void StopTargets();

        protected ITargetsInLevelManagerContextProvider TargetsInLevelManagerProvider;
        protected ITargetFactory TargetFactory;

        [Inject]
        public void ConstructBase(ITargetsInLevelManagerContextProvider targetsInLevelManagerContextProvider,
            ITargetFactory targetFactory)
        {
            TargetsInLevelManagerProvider = targetsInLevelManagerContextProvider;
            TargetsInLevelManagerProvider.Register(this);

            TargetFactory = targetFactory;
        }

        protected virtual void OnOnTargetHit(GameObject targetInstance) => OnTargetHit?.Invoke();

        protected virtual void OnDestroy() => TargetsInLevelManagerProvider.Unregister(this);
    }
}