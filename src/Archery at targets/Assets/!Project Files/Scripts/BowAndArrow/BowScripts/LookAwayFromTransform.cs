using UnityEngine;

namespace BowAndArrow.BowScripts
{
    public class LookAwayFromTransform : MonoBehaviour
    {
        [SerializeField] private Transform targetTransform;
        [SerializeField] private Transform upTransform;
        
        private Transform _transform;
        private Quaternion _resetRotation;

        private void Start()
        {
            _transform = transform;
            _resetRotation = _transform.localRotation;
        }

        public void LookAway()
        {
            var targetDirection = targetTransform.position - _transform.position;
            _transform.rotation = Quaternion.LookRotation(-targetDirection, upTransform.up);
        }

        public void ResetLook()
        {
            _transform.localRotation = _resetRotation;
        }
    }
}