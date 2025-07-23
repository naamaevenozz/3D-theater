using Edelweiss.Core;
using ImprovedTimers;
using UnityEngine;
using Debug = Edelweiss.Util.Debugging.Debug;

namespace Edelweiss.PhysicsSystem
{
    public class ShockwaveReceiver : EdelMono
    {
        [SerializeField]
        private Rigidbody rb;

        private Rigidbody[] _childrenRigidbodies;

        private CountdownTimer _recoveryTimer;

        public bool IsRecovering => _recoveryTimer is { IsRunning: true };

        private void Awake()
        {
            Reset();

            _childrenRigidbodies = GetComponentsInChildren<Rigidbody>();

            _recoveryTimer = new(Config.ShockwaveRecoveryTime);
        }

        private void Reset()
        {
            ValidateComponent(ref rb);
        }

        public void ApplyShockwave(Vector3 force)
        {
            if (rb == null || IsRecovering) return;

            foreach (Rigidbody childRigidbody in _childrenRigidbodies)
            {
                childRigidbody.linearVelocity = Vector3.zero;
            }

            rb.AddForce(force, ForceMode.Impulse);

            Debug.DrawArrow(rb.position, rb.position + force, Color.yellow, 1f);

            _recoveryTimer.Reset(Config.ShockwaveRecoveryTime);
            _recoveryTimer.Start();
        }

        public void ApplyShockwaveExplosion(Vector3 position, float force, float radius)
        {
            if (rb == null || IsRecovering) return;

            foreach (Rigidbody childRigidbody in _childrenRigidbodies)
            {
                childRigidbody.linearVelocity = Vector3.zero;
            }

            rb.AddExplosionForce(force, position, radius, 1f, ForceMode.Impulse);

            Debug.DrawArrow(rb.position, rb.position + rb.linearVelocity, Color.yellow, 1f);

            _recoveryTimer.Reset(Config.ShockwaveRecoveryTime);
            _recoveryTimer.Start();
        }
    }
}