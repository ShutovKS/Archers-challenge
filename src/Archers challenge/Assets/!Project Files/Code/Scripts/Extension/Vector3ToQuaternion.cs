#region

using UnityEngine;

#endregion

namespace Extension
{
    public static class Vector3ToQuaternion
    {
        public static Quaternion ToQuaternion(this Vector3 vector3) =>
            Quaternion.Euler(vector3.x, vector3.y, vector3.z);
    }
}