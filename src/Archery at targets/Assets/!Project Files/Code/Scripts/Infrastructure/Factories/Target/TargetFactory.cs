#region

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Paths;
using Features.ColliderTools;
using Infrastructure.Factories.GameObjects;
using UnityEngine;
using Object = UnityEngine.Object;

#endregion

namespace Infrastructure.Factories.Target
{
    public class TargetFactory : ITargetFactory
    {
        public event Action<GameObject> TargetHit;

        private readonly IGameObjectFactory _gameObjectFactory;
        private readonly List<GameObject> _targets = new();

        private const string TARGET_PREFAB_PATH = AddressablesPaths.TARGET_PREFAB;

        public TargetFactory(IGameObjectFactory gameObjectFactory)
        {
            _gameObjectFactory = gameObjectFactory;
        }

        public async Task<GameObject> Instantiate(Vector3 position, Quaternion rotation, Transform parent = null)
        {
            var isHit = false;
            var instance = await _gameObjectFactory.InstantiateAsync(TARGET_PREFAB_PATH, position, rotation, parent);

            var colliderInteractionEnterTrigger = instance.AddComponent<ColliderInteractionEnterTrigger>();
            colliderInteractionEnterTrigger.OnTriggered += OnHit;

            _targets.Add(instance);

            return instance;

            void OnHit(Collider collider)
            {
                if (!isHit && CheckHit(collider))
                {
                    isHit = true;
                    TargetHit?.Invoke(instance);
                }
            }

            bool CheckHit(Collider collider)
            {
                return collider.TryGetComponent<Features.Projectile.Arrow>(out _);
            }
        }

        public void Destroy(GameObject target)
        {
            if (_targets.Remove(target))
            {
                Object.Destroy(target);
            }
        }

        public void DestroyAll()
        {
            foreach (var target in _targets)
            {
                Object.Destroy(target);
            }

            _targets.Clear();
        }
    }
}