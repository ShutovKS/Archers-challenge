using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Extension;
using Features.ColliderTools;
using Infrastructure.Factories.GameObjects;
using Infrastructure.Services.AssetsAddressables;
using JetBrains.Annotations;
using Tools;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Infrastructure.Factories.Target
{
    [UsedImplicitly]
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

        public async Task<string> Instantiate(Vector3 position, Quaternion rotation)
        {
            var isHit = false;
            var id = UniqueIDGenerator.Generate();
            var instance = await _gameObjectFactory.Instantiate(TARGET_PREFAB_PATH);
            instance.SetPositionAndRotation(position, rotation);

            var colliderInteractionEnterTrigger = instance.AddComponent<ColliderInteractionEnterTrigger>();
            colliderInteractionEnterTrigger.OnTriggered += OnHit;

            _idToTarget.Add(id, instance);

            return id;

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
                return collider.TryGetComponent<Features.Projectile.Arrow>(out _);
            }
        }

        public void Destroy(string targetId)
        {
            if (_idToTarget.Remove(targetId, out var gameObject))
            {
                Object.Destroy(gameObject);
            }
        }

        public void DestroyAll()
        {
            foreach (var (_, gameObject) in _idToTarget)
            {
                Object.Destroy(gameObject);
            }

            _idToTarget.Clear();
        }
    }
}