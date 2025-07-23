using System;
using Edelweiss.Utils;
using Sirenix.OdinInspector;
using UnityEngine;
using Debug = Edelweiss.Util.Debugging.Debug;

namespace Edelweiss.PhysicsSystem
{
    [Serializable]
    public class Shockwave
    {
        public float Radius = 5f;
        public float Force  = 10f;

        [Range(0, 5f)]
        public float DecayRate = 0f;

        public LayerMask LayerMask;

        public Shockwave(float radius, float force, LayerMask layerMask)
        {
            Radius    = radius;
            Force     = force;
            LayerMask = layerMask;
        }

        [Button]
        public void Trigger(Vector3 position)
        {
            Collider[] colliders = Physics.OverlapSphere(position, Radius, LayerMask);

            foreach (Collider collider in colliders)
            {
                var receiver = collider.GetComponent<ShockwaveReceiver>();

                if (receiver == null) continue;

                ApplyShockwaveOnRigidbody(position, receiver);
            }

            Debug.DrawSphere(position, Radius, Color.red, 1f);
        }

        private void ApplyShockwaveOnRigidbody(Vector3 position, ShockwaveReceiver receiver)
        {
            Vector3 receiverPos = receiver.transform.position;
            float   distance    = Vector3.Distance(position, receiverPos);
            float   normalized  = distance / Radius;

            if (normalized > 1f) return;

            float forceMagnitude = Force * Mathf.Pow(1f - normalized, DecayRate);

            Vector3 direction   = (receiverPos - position).With(z: 0).normalized;
            Vector3 forceVector = AdjustForce(direction * forceMagnitude);

            // receiver.ApplyShockwave(forceVector);
            receiver.ApplyShockwaveExplosion(position, forceVector.magnitude, Radius);

            float f         = .25f + (normalized * .75f);
            Color lineColor = new Color(f, 0, 0, f);

            // Debug.DrawLine(position, receiverPos, lineColor, 1f);
        }

        private Vector3 AdjustForce(Vector3 force)
        {
            float magnitude = force.magnitude;

            float signX = Mathf.Sign(force.x);
            float signY = Mathf.Sign(force.y);

            float x1 = signX * force.normalized.x + 2;
            float y1 = signY * force.normalized.y + 1;

            float adjustedX = signX * magnitude * Mathf.Pow(x1, y1 / x1);
            float adjustedY = signY * magnitude * Mathf.Log(y1);

            return new Vector3(adjustedX, adjustedY, 0f);
        }
    }
}