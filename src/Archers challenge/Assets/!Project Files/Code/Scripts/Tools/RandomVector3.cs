#region

using UnityEngine;

#endregion

namespace Tools
{
    public static class RandomVector3
    {
        /// <summary>
        /// Возвращает случайный Vector3 в диапазоне от min до max.
        /// </summary>
        /// <param name="min">Минимальное значение диапазона.</param>
        /// <param name="max">Максимальное значение диапазона.</param>
        /// <returns>Случайный Vector3 в диапазоне от min до max.</returns>
        public static Vector3 Range(Vector3 min, Vector3 max)
        {
            var x = Random.Range(min.x, max.x);
            var y = Random.Range(min.y, max.y);
            var z = Random.Range(min.z, max.z);

            return new Vector3(x, y, z);
        }
    }
}