#region

using UnityEngine;

#endregion

namespace Extension
{
    public static class GameObjectSetPositionAndRotation
    {
        public static void SetPositionAndRotation(this GameObject gameObject, Vector3 position, Quaternion rotation) =>
            gameObject.transform.SetPositionAndRotation(position, rotation);

        public static void SetPositionAndRotation(this GameObject gameObject, Vector3 position, Vector3 rotation) =>
            gameObject.transform.SetPositionAndRotation(position, rotation.ToQuaternion());

        public static void SetPositionAndRotation(this GameObject gameObject, Transform spawnPoint) =>
            gameObject.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
    }
}