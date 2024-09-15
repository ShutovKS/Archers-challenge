#region

using UnityEngine;

#endregion

namespace Features.PositionsContainer
{
    public class RandomPositionsFromRegion : PositionsContainer
    {
        public override bool InfinitePositions => true;

        [SerializeField] private Transform minPositionTransform, maxPositionTransform;
        [SerializeField] private Quaternion rotation;
        private Vector3 _minPosition, _maxPosition;

        private void Awake()
        {
            _minPosition = minPositionTransform.position;
            _maxPosition = maxPositionTransform.position;
        }

        public override (Vector3 position, Quaternion rotation) GetTargetPosition()
        {
            var position = new Vector3(
                Random.Range(_minPosition.x, _maxPosition.x),
                Random.Range(_minPosition.y, _maxPosition.y),
                Random.Range(_minPosition.z, _maxPosition.z)
            );

            return (position, rotation);
        }
    }
}