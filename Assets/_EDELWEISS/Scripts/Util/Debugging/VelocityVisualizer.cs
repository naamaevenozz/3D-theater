using System;
using Edelweiss.Core;
using Edelweiss.Damage;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Edelweiss.Utils.Debuggging
{
    public class VelocityVisualizer : EdelMono
    {
        [Required]
        [SerializeField]
        private Rigidbody rb;

        [Required]
        [SerializeField]
        private LineRenderer lineRenderer;

        private DamageComponent dc;

        private void Awake()
        {
            Reset();
            ValidateComponent(ref dc, optional: true);
        }

        private void Update()
        {
            Vector3 startPos = transform.position;
            Vector3 velocity = rb.linearVelocity.With(z: 0) * Config.ImpulseScale;

            if (dc) velocity -= dc.RootRigidbody.linearVelocity.With(z: 0) * Config.ImpulseScale;
            Vector3 endPos   = startPos + velocity;

            lineRenderer.SetPosition(0, startPos);
            lineRenderer.SetPosition(1, endPos);
        }

        private void Reset()
        {
            ValidateComponent(ref rb);
        }

        private void OnValidate()
        {
            ValidateComponent(ref lineRenderer);

            if (lineRenderer == null) lineRenderer = gameObject.AddComponent<LineRenderer>();

            lineRenderer.SetPosition(0, Vector3.zero);
            lineRenderer.SetPosition(1, Vector3.zero);

            lineRenderer.startWidth = lineRenderer.endWidth = .06f;

            lineRenderer.startColor = Color.yellow;
            lineRenderer.endColor   = Color.red;
        }

        private void OnDestroy()
        {
            Destroy(lineRenderer);
        }
    }
}