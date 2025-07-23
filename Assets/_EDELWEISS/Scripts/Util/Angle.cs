using UnityEngine;

namespace Edelweiss.Utils
{
    [System.Serializable]
    public struct Angle
    {
        [SerializeField]
        private float _raw;

        private float Raw
        {
            get => (_raw % 360f + 360f) % 360f;
            set => _raw = (value % 360f + 360f) % 360f;
        }

        private Angle(float value)
        {
            _raw = 0;
            Raw  = value;
        }

        public static implicit operator Angle(float value) => new Angle(value);
        public static implicit operator float(Angle angle) => angle.Raw;


        public static Angle operator +(Angle a, Angle b) => new(a.Raw + b.Raw);
        public static Angle operator -(Angle a, Angle b) => new(a.Raw - b.Raw);
        public static Angle operator *(Angle a, Angle b) => new(a.Raw * b.Raw);
        public static Angle operator /(Angle a, Angle b) => new(a.Raw / b.Raw);
        public static Angle operator -(Angle a)          => new(-a.Raw);
        public static Angle operator %(Angle a, Angle b) => new(a.Raw % b.Raw);

        public static float operator +(Angle a, float b) => (Angle)(a.Raw + b);
        public static float operator -(Angle a, float b) => (Angle)(a.Raw - b);
        public static float operator *(Angle a, float b) => (Angle)(a.Raw * b);
        public static float operator /(Angle a, float b) => (Angle)(a.Raw / b);
        public static float operator %(Angle a, float b) => (Angle)(a.Raw % b);
        public static float operator +(float a, Angle b) => (Angle)(a + b.Raw);
        public static float operator -(float a, Angle b) => (Angle)(a - b.Raw);
        public static float operator *(float a, Angle b) => (Angle)(a * b.Raw);
        public static float operator /(float a, Angle b) => (Angle)(a / b.Raw);
        public static float operator %(float a, Angle b) => (Angle)(a % b.Raw);

        public static float operator +(Angle a, int   b) => (Angle)(a.Raw + b);
        public static float operator -(Angle a, int   b) => (Angle)(a.Raw - b);
        public static float operator *(Angle a, int   b) => (Angle)(a.Raw * b);
        public static float operator /(Angle a, int   b) => (Angle)(a.Raw / b);
        public static float operator %(Angle a, int   b) => (Angle)(a.Raw % b);
        public static float operator +(int   a, Angle b) => (Angle)(a + b.Raw);
        public static float operator -(int   a, Angle b) => (Angle)(a - b.Raw);
        public static float operator *(int   a, Angle b) => (Angle)(a * b.Raw);
        public static float operator /(int   a, Angle b) => (Angle)(a / b.Raw);
        public static float operator %(int   a, Angle b) => (Angle)(a % b.Raw);

        public static float ShortestDistance(Angle a, Angle b)
        {
            float delta = (b.Raw - a.Raw) % 360f;
            if (delta > 180f)
                delta -= 360f;
            else if (delta < -180f)
                delta += 360f;

            return delta;
        }

        public static float ShortestDistanceAbs(Angle a, Angle b) => Mathf.Abs(ShortestDistance(a, b));

        public static Angle Lerp(Angle a, Angle b, float t)
        {
            t = Mathf.Clamp01(t);

            float delta = ShortestDistance(a, b);

            float result = a.Raw + delta * t;
            return new Angle(result);
        }

        public bool IsInBetween(Angle a, Angle b)
        {
            if (a > b)
            {
                return (this >= a && this <= 360f) || (this >= 0f && this <= b);
            }
            else
            {
                return this >= a && this <= b;
            }
        }

        public override string ToString()
        {
            return Raw.ToString();
        }
    }
}