using Edelweiss.Utils;
using UnityEngine;

namespace Edelweiss.Util.Debugging
{
    public class Debug : UnityEngine.Debug
    {
        public static void DrawCircle(Vector3 center, Vector3 axis, float radius, Color color) =>
            DrawCircle(center, axis, radius, color, 32);

        public static void DrawCircle(Vector3 center, Vector3 axis, float radius, Color color, int segments) =>
            DrawCircle(center, axis, radius, color, segments, 0f);

        public static void DrawCircle(Vector3 center, Vector3 axis, float radius, Color color, int segments,
                                      float   duration)
        {
            Vector3 normalizedZ = axis.normalized;
            Vector3 normalizedX = Vector3.Cross(Vector3.up, normalizedZ).normalized;
            if (normalizedZ == Vector3.up || normalizedZ == Vector3.down)
            {
                normalizedX = Vector3.Cross(Vector3.right, normalizedZ).normalized;
            }

            Vector3 normalizedY = Vector3.Cross(normalizedZ, normalizedX).normalized;

            float angleStep = 360f / segments;

            for (int i = 0; i < segments; i++)
            {
                float angle1 = Mathf.Deg2Rad * (i       * angleStep);
                float angle2 = Mathf.Deg2Rad * ((i + 1) * angleStep);

                Vector3 point1 = center + normalizedX * Mathf.Cos(angle1) * radius +
                                 normalizedY          * Mathf.Sin(angle1) * radius;
                Vector3 point2 = center + normalizedX * Mathf.Cos(angle2) * radius +
                                 normalizedY          * Mathf.Sin(angle2) * radius;

                DrawLine(point1, point2, color, duration);
            }
        }

        public static void DrawWireSphere(Vector3 position, float radius, Color color, int circleSegments,
                                          float   duration)
        {
            DrawCircle(position, Vector3.up,      radius, color, circleSegments, duration);
            DrawCircle(position, Vector3.right,   radius, color, circleSegments, duration);
            DrawCircle(position, Vector3.forward, radius, color, circleSegments, duration);
        }

        public static void DrawSphere(Vector3 position, float radius, Color color) =>
            DrawSphere(position, radius, 8, color, 0);

        public static void DrawSphere(Vector3 position, float radius, int circleSegments, Color color) =>
            DrawSphere(position, radius, circleSegments, color, 0f);

        public static void DrawSphere(Vector3 position, float radius, Color color,
                                      float   duration) =>
            DrawSphere(position, radius, 8, color, duration);

        public static void DrawSphere(Vector3 position, float radius, int circleSegments,
                                      Color   color,
                                      float   duration)
        {
            float latitudeStep = 180f / circleSegments;

            Vector3 axis = Vector3.forward;
            for (int i = 0; i <= circleSegments; i++)
            {
                float rotationAngle = i * latitudeStep;

                Vector3 rotatedAxis = Quaternion.Euler(0, rotationAngle, 0)     * axis;
                DrawCircle(position, rotatedAxis, radius, color, circleSegments * 2, duration);
            }

            Vector3 longtitudeAxis = Vector3.up;
            float   angle          = -Mathf.PI / 2f;

            for (int i = 0; i <= circleSegments; i++)
            {
                float y = position.y + Mathf.Sin(angle) * radius;

                Vector3 longtitudeCenter = position.With(y: y);
                float   currentRadius    = Mathf.Cos(angle) * radius;
                if (currentRadius >= .001f)
                {
                    DrawCircle(longtitudeCenter, longtitudeAxis, currentRadius, color, circleSegments * 2, duration);
                }

                angle += Mathf.PI / circleSegments;
            }
        }


        public static void DrawArrow(Vector3 start, Vector3 end, Color color) =>
            DrawArrow(start, end, color, 0f);

        public static void DrawArrow(Vector3 start, Vector3 end, Color color, float duration) =>
            DrawArrow(start, end, 0.15f, color, duration);

        public static void DrawArrow(Vector3 start, Vector3 end, float headScale, Color color) =>
            DrawArrow(start, end, headScale, color, 0f);

        public static void DrawArrow(Vector3 start, Vector3 end, float headScale, Color color, float duration)
        {
            Vector3 direction = (end - start).normalized;
            float   distance  = Vector3.Distance(start, end);

            DrawLine(start, end, color, duration);

            Vector3 upAxis = Vector3.Cross(direction, Vector3.right).normalized;
            if (upAxis == Vector3.zero)
            {
                upAxis = Vector3.Cross(direction, Vector3.up).normalized;
            }

            Vector3 rightAxis = Vector3.Cross(direction, upAxis).normalized;

            Vector3 headBase   = end      - direction * headScale;
            Vector3 headLeft   = headBase + rightAxis * headScale;
            Vector3 headRight  = headBase - rightAxis * headScale;
            Vector3 headTop    = headBase + upAxis    * headScale;
            Vector3 headBottom = headBase - upAxis    * headScale;

            DrawLine(end,       headLeft,   color, duration);
            DrawLine(end,       headRight,  color, duration);
            DrawLine(headLeft,  headTop,    color, duration);
            DrawLine(headLeft,  headBottom, color, duration);
            DrawLine(headRight, headTop,    color, duration);
            DrawLine(headRight, headBottom, color, duration);
            DrawLine(headTop,   headBottom, color, duration);
        }
    }
}