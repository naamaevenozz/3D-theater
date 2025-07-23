using UnityEngine;

namespace Edelweiss.Utils
{
    public static class AngleExtensions
    {
        public static Angle ToAngle(this Vector2 vector)
        {
            return Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
        }

        public static Vector2 ToVector2(this Angle angle)
        {
            float x = Mathf.Cos(angle);
            float y = Mathf.Sin(angle);

            return new Vector2(x, y).normalized;
        }

        public static Vector3 ToVector3(this Angle angle)
        {
            float x = Mathf.Cos(angle);
            float z = Mathf.Sin(angle);

            return new Vector3(x, 0f, z).normalized;
        }
    }
}