using UnityEngine;

namespace ShootingGallery
{
    public class TargetSpawnerMR : TargetSpawner
    {
        [SerializeField] private GameObject targetPrefab;
        [SerializeField] private Transform playerTransform;

        private void Start()
        {
            SpawnTarget();
        }

        protected override void SpawnTarget()
        {
            var instantiate = Instantiate(targetPrefab, GetRandomPosition(), Quaternion.identity);
            instantiate.transform.LookAt(playerTransform.position + Vector3.up);

            var target = instantiate.GetComponent<Target>();
            target.OnHit += base.OnTargetHit;
        }

        private Vector3 GetRandomPosition()
        {
            var x = Random.Range(-1.5f, 1.5f);
            var y = Random.Range(0.5f, 1.5f);
            var z = Random.Range(2.5f, 3.5f);
            return playerTransform.position + new Vector3(x, y, z);
        }
    }
}