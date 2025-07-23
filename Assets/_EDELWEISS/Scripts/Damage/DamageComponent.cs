using Edelweiss.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Edelweiss.Damage
{
    public class DamageComponent : EdelMono
    {
        [SerializeField]
        private float damageScale = 1f;

        [SerializeField]
        private float damageAddition = 0f;

        [SerializeField]
        private int priority = 10;

        [SerializeField]
        private Rigidbody rootRigidbody;

        public float DamageScale    => damageScale;
        public float DamageAddition => damageAddition;
        public int   Priority       => priority;

        public Rigidbody RootRigidbody => rootRigidbody;
    }
}