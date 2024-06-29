using UnityEngine;

namespace BowAndArrow.BowScripts
{
    public class PrefabLauncher : MonoBehaviour
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private float forceMultiplier = 500f;

        private Rigidbody _bodyToLaunch;
        private Transform _trans;

        private void Start()
        {
            _trans = transform;
        }

        public void InstantiatePrefab()
        {
            var instantiate = Instantiate(prefab, _trans.position, _trans.rotation, _trans);
            _bodyToLaunch = instantiate.GetComponent<Rigidbody>();

            if (_bodyToLaunch == null)
            {
                Debug.Log("У префаба нет жесткой конструкции, так что добавим ее");
                _bodyToLaunch = instantiate.AddComponent<Rigidbody>();
            }
        }

        public void LaunchPrefab(float forceAmount)
        {
            _bodyToLaunch.isKinematic = false;
            _bodyToLaunch.transform.parent = null;
            var force = _trans.forward * (forceAmount * forceMultiplier);
            _bodyToLaunch.AddForce(force);
        }
    }
}