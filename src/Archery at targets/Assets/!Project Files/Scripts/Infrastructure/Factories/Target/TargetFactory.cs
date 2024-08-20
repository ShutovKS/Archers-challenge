using System;
using System.Collections.Generic;
using Extension;
using Features.BowAndArrows;
using Features.ColliderTools;
using Infrastructure.Factories.GameObjects;
using Infrastructure.Services.AssetsAddressables;
using Tools;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Infrastructure.Factories.Target
{
    public class TargetFactory : ITargetFactory
    {
        public event Action<string> TargetHit;

        private readonly IGameObjectFactory _gameObjectFactory;
        private readonly Dictionary<string, GameObject> _idToTarget = new();

        private const string TARGET_PREFAB_PATH = AssetsAddressableConstants.TARGET_PREFAB;

        public TargetFactory(IGameObjectFactory gameObjectFactory)
        {
            _gameObjectFactory = gameObjectFactory;
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
            var instance = await _gameObjectFactory.Instance(TARGET_PREFAB_PATH);
            instance.SetPositionAndRotation(position, rotation);

            var colliderInteractionEnterTrigger = instance.AddComponent<ColliderInteractionEnterTrigger>();
            colliderInteractionEnterTrigger.OnTriggered += OnHit;

            _idToTarget.Add(id, instance);

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