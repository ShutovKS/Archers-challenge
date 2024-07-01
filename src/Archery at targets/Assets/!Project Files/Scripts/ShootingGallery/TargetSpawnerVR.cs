using UnityEngine;

namespace ShootingGallery
{
    public class TargetSpawnerVR : TargetSpawner
    {
        [SerializeField] private Vector3 pointLimitationMin;
        [SerializeField] private Vector3 pointLimitationMax;
        [SerializeField] private GameObject targetPrefab;

        private void Start()
        {
            SpawnTarget();
        }

        protected override void SpawnTarget()
        {
            var instantiate = Instantiate(targetPrefab, GetRandomPosition(), Quaternion.identity);

            var target = instantiate.GetComponent<Target>();
            target.OnHit += base.OnTargetHit;
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