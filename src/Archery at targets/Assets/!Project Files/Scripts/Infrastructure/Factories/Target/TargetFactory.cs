using System;
using System.Collections.Generic;
using Features.BowAndArrows;
using Features.ColliderTools;
using Infrastructure.Services.AssetsAddressables;
using Tools;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Infrastructure.Factories.Target
{
    public class TargetFactory : ITargetFactory
    {
        public event Action<string> TargetHit;

        private readonly DiContainer _container;
        private readonly AssetsAddressablesProvider _assetsAddressablesProvider;
        private readonly Dictionary<string, GameObject> _idToTarget = new();

        private const string TARGET_PREFAB_PATH = AssetsAddressableConstants.TARGET_PREFAB;

        public TargetFactory(DiContainer container, AssetsAddressablesProvider assetsAddressablesProvider)
        {
            _container = container;
            _assetsAddressablesProvider = assetsAddressablesProvider;
        }

        public void SpawnTargets(int count, Vector3 pointLimitationMin, Vector3 pointLimitationMax,
            Quaternion rotation)
        {
            for (var i = 0; i < count; i++)
            {
                var position = RandomVector3.Range(pointLimitationMin, pointLimitationMax);

                InstanceTarget(position, rotation);
            }
        }

        public void Destroy(string targetId)
        {
            if (_idToTarget.Remove(targetId, out var gameObject))
            {
                Object.Destroy(gameObject);
            }
        }

        private async void InstanceTarget(Vector3 position, Quaternion rotation)
        {
            var isHit = false;
            var id = UniqueIDGenerator.Generate();
            var prefab = await _assetsAddressablesProvider.GetAsset<GameObject>(TARGET_PREFAB_PATH);

            var instantiate = _container.InstantiatePrefab(prefab, position, rotation, null);

            var colliderInteractionEnterTrigger = instantiate.AddComponent<ColliderInteractionEnterTrigger>();
            colliderInteractionEnterTrigger.OnTriggered += OnHit;

            _idToTarget.Add(id, instantiate);

            return;

            void OnHit(Collider collider)
            {
                if (!isHit && CheckHit(collider))
                {
                    isHit = true;
                    TargetHit?.Invoke(id);
                }
            }

            bool CheckHit(Collider collider)
            {
                return collider.TryGetComponent<Arrow>(out _);
            }
        }
    }
}