using UnityEngine;

namespace Utils
{
    public static class Helpers
    {
        public static bool IsInRange(Vector3 a, Vector3 b, float range)
        {
            bool result = false;

            Vector3 distance = Direction(a, b);
            result = distance.sqrMagnitude <= (range * range);

            return result;
        }

        public static bool IsInRange(Vector3 distance, float range)
        {
            bool result = false;

            result = distance.sqrMagnitude <= (range * range);

            return result;
        }

        public static Vector3 Direction(Vector3 from, Vector3 to)
        {
            Vector3 direction = to - from;
            return direction;
        }
    }
}
