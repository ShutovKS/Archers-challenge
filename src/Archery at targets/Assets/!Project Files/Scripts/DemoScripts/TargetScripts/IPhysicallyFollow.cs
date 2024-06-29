using UnityEngine;

namespace DemoScripts.TargetScripts
{
    [RequireComponent(typeof(Rigidbody))]
    public class IPhysicallyFollow : MonoBehaviour
    {
        public Transform followTargetTransform;

        public bool followRotation = true;
        public float rotateSpeed = 1;

        public bool followTranslation = true;
        public float translateSpeed = 1;

        private Transform _transform;
        private Rigidbody _rigbody;

        private void Start()
        {
            _transform = transform;
            _rigbody = GetComponent<Rigidbody>();

            if(followTargetTransform == null)
            {
                CreateDummyTarget();
            }

            _rigbody.position = followTargetTransform.position;
            _rigbody.rotation = followTargetTransform.rotation;
        }

        private void CreateDummyTarget()
        {
            followTargetTransform = new GameObject("Target").transform;
            followTargetTransform.transform.position = _transform.position;
            followTargetTransform.transform.rotation = _transform.rotation;
        }

        private void Update()
        {
            PhysicallyFollow();
        }

        private void PhysicallyFollow()
        {
            if (followTranslation)
            {
                var distanceFromTarget = Vector3.Distance(followTargetTransform.position, _transform.position);
                var directionToTarget = followTargetTransform.position - _transform.position;
                _rigbody.linearVelocity = directionToTarget.normalized * (distanceFromTarget * translateSpeed);
            }

            if (followRotation)
            {
                var rotationalOffset = followTargetTransform.rotation * Quaternion.Inverse(_rigbody.rotation);
                rotationalOffset = ShortRotation(rotationalOffset);            
                rotationalOffset.ToAngleAxis(out var angle, out var axis);
                _rigbody.angularVelocity = axis * (angle * Mathf.Deg2Rad * rotateSpeed);
            }
        }

        private Quaternion ShortRotation(Quaternion q)
        {
            if (q.w < 0)
            {
                q.x = -q.x;
                q.y = -q.y;
                q.z = -q.z;
                q.w = -q.w;
            }
            return q;
        }
    }
}
