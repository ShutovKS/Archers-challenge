using UnityEngine;

namespace DemoScripts
{
    /// <summary>
    /// makes physics layer have 2 layers not interact
    /// made for bow and arrow script so bow and arrow dont see eachother.
    /// /// </summary>
    public class FixPhysicsLayers : MonoBehaviour
    {
        public int layer1 = 6;
        public int layer2 = 7;

        private void Start()
        {
            Physics.IgnoreLayerCollision(layer1, layer2, true);
        }


        private void Update()
        {
        
        }
    }
}
