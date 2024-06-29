using UnityEngine;

namespace BowAndArrow.BowScripts
{
    public class FollowTransformOnRail : MonoBehaviour
    {
        [SerializeField] private Transform targetTransform;

        [SerializeField] private float railMin = -0.7f;
        [SerializeField] private float railMax;

        private Transform _trans;
        private Vector3 _resetPosition;

        private void Start()
        {
            _trans = transform;
            _resetPosition = _trans.localPosition;
        }

        public void FollowOnRail()
        {
            _trans.position = targetTransform.position;
            _trans.localPosition = new Vector3(0, 0, Mathf.Clamp(_trans.localPosition.z, railMin, railMax));
        }

        public void ResetPosition()
        {
            _trans.localPosition = _resetPosition;
        }
    }
}