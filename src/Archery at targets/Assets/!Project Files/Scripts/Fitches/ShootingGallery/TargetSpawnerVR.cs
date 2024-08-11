using UnityEngine;

namespace Fitches.ShootingGallery
{
    public class TargetSpawnerVR : TargetSpawner
    {
        [SerializeField] private Vector3 pointLimitationMin;
        [SerializeField] private Vector3 pointLimitationMax;
        [SerializeField] private GameObject targetPrefab;

        public override void SpawnTarget()
        {
            var instantiate = Instantiate(targetPrefab, GetRandomPosition(), Quaternion.identity);

            var target = instantiate.GetComponent<Target>();
            target.OnHit += OnTargetHit;
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