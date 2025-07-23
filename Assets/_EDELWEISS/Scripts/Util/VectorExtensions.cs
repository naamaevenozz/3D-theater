using UnityEngine;

namespace Edelweiss.Utils
{
    public static class VectorExtensions
    {
        public static Vector3 With(this Vector3 vector, float? x = null, float? y = null, float? z = null)
        {
            return new Vector3(x ?? vector.x, y ?? vector.y, z ?? vector.z);
        }

        public static Vector2 XY(this Vector3 vector) => new(vector.x, vector.y);
        public static Vector2 YZ(this Vector3 vector) => new(vector.y, vector.z);
        public static Vector2 XZ(this Vector3 vector) => new(vector.x, vector.z);
        public static Vector2 YX(this Vector3 vector) => new(vector.y, vector.x);
        public static Vector2 ZX(this Vector3 vector) => new(vector.z, vector.x);
        public static Vector2 ZY(this Vector3 vector) => new(vector.z, vector.y);
    }
}