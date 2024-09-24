using System.Threading.Tasks;
using UnityEngine;

namespace Features.TargetsInLevelManager
{
    public class RandomTargetsFromRegionAndLookingAtTarget : TargetsInLevelManager
    {
        [SerializeField] private Transform minPositionTransform, maxPositionTransform;
        [SerializeField] private Transform targetTransform;
        private Vector3 _minPosition, _maxPosition;

        private void Awake()
        {
            _minPosition = minPositionTransform.position;
            _maxPosition = maxPositionTransform.position;
        }

        public override void PrepareTargets()
        {
        }

        public override async void StartTargets()
        {
            TargetFactory.TargetHit += OnOnTargetHit;

            await InstantiateTarget();
        }

        public override void StopTargets()
        {
            TargetFactory.TargetHit -= OnOnTargetHit;

            TargetFactory.DestroyAll();
        }

        protected override async void OnOnTargetHit(GameObject targetInstance)
        {
            TargetFactory.Destroy(targetInstance);

            await InstantiateTarget();

            base.OnOnTargetHit(targetInstance);
        }

        private async Task InstantiateTarget()
        {
            var position = GetTargetPosition();
            var rotation = GetTargetRotation(position);

            await TargetFactory.Instantiate(position, rotation);
        }

        private Vector3 GetTargetPosition() => new(
            Random.Range(_minPosition.x, _maxPosition.x),
            Random.Range(_minPosition.y, _maxPosition.y),
            Random.Range(_minPosition.z, _maxPosition.z)
        );

        private Quaternion GetTargetRotation(Vector3 position) =>
            Quaternion.LookRotation(targetTransform.position - position);
    }
}