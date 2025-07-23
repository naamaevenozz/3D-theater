using Edelweiss.Core;
using Edelweiss.Utils;
using UnityEngine;
using Debug = Edelweiss.Util.Debugging.Debug;

namespace Edelweiss.Damage
{
    public class DamageablePart : EdelMono, IHittable
    {
        private HitAccumulator _hitAccumulator;

        [SerializeField]
        private float damageScale = 1f;

        [SerializeField]
        private float damageAddition = 0f;

        [SerializeField]
        private int priority = 10;

        public float          DamageScale    => damageScale;
        public float          DamageAddition => damageAddition;
        public int            Priority       => priority;
        public HitAccumulator HitAccumulator => _hitAccumulator;

        public void InjectHitAccumulator(HitAccumulator hitAccumulator)
        {
            _hitAccumulator = hitAccumulator;
        }

        private void OnCollisionEnter(Collision other)
        {
            other.ComponentScan()
                 .DoSingle((DamageComponent damager) => HandleDamagerEnter(damager, other));
        }

        private void HandleDamagerEnter(DamageComponent damager, Collision other)
        {
            if (damager.enabled == false) return;
            bool strongEnough = other.impulse.magnitude * Config.ImpulseScale >= Config.ImpulseThreshold;
            if (!strongEnough) return;

            _hitAccumulator?.TryRegisterHit(damager, this, other);

            Vector3 pos     = damager.transform.position;
            Vector3 vel     = damager.GetComponent<Rigidbody>().linearVelocity.With(z: 0);
            Vector3 impulse = other.impulse.With(z: 0);

            Debug.DrawArrow(pos, pos + impulse, Color.cyan,  1f);
            Debug.DrawArrow(pos, pos + vel,     Color.green, 1f);
        }
    }
}