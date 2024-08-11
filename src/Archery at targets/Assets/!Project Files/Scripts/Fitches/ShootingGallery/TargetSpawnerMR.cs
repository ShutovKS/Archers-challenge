using UnityEngine;

namespace Fitches.ShootingGallery
{
    public class TargetSpawnerMR : TargetSpawner
    {
        [SerializeField] private GameObject targetPrefab;
        [SerializeField] private Transform playerTransform;

        public override void SpawnTarget()
        {
            var instantiate = Instantiate(targetPrefab, GetRandomPosition(), Quaternion.identity);
            instantiate.transform.LookAt(playerTransform.position + Vector3.up);

            var target = instantiate.GetComponent<Target>();
            target.OnHit += OnTargetHit;
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