using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ShootingGallery
{
    public class TargetSpawner : MonoBehaviour
    {
        public event Action<int> OnTargetHitCountChanged;
        public int TargetCount { get; private set; }

        [SerializeField] private Vector3 pointLimitationMin;
        [SerializeField] private Vector3 pointLimitationMax;
        [SerializeField] private GameObject targetPrefab;

        private void Start()
        {
            SpawnTarget();
        }

        private void SpawnTarget()
        {
            var instantiate = Instantiate(targetPrefab, GetRandomPosition(), Quaternion.identity);

            var target = instantiate.GetComponent<Target>();
            target.OnHit += OnTargetHit;
        }

        private void OnTargetHit()
        {
            TargetCount++;
            OnTargetHitCountChanged?.Invoke(TargetCount);

            Debug.Log("Target hit: " + TargetCount);
            SpawnTarget();
        }

        private Vector3 GetRandomPosition()
        {
            var x = Random.Range(pointLimitationMin.x, pointLimitationMax.x);
            var y = Random.Range(pointLimitationMin.y, pointLimitationMax.y);
            var z = Random.Range(pointLimitationMin.z, pointLimitationMax.z);
            return new Vector3(x, y, z);
        }
    }
}