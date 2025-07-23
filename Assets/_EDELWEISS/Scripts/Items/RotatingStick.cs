using System;
using Edelweiss.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Edelweiss.Items
{
    public class RotatingStick : EdelMono
    {
        [Required]
        [SerializeField]
        private Rigidbody rb;

        [SerializeField]
        private float linearThreshold = 1;

        [SerializeField]
        private Vector3 rotationAxis = Vector3.up;

        [SerializeField]
        private float rotationScale = 1f;

        [SerializeField]
        private Collider collider;

        private void Awake()
        {
            ValidateComponent(ref collider);
        }

        public void OnGrab()
        {
            collider.excludeLayers = LayerMask.NameToLayer("Ground");
        }

        public void OnRelease()
        {
            collider.excludeLayers = default;
        }

        private void FixedUpdate()
        {
            float linearMagnitude = rb.linearVelocity.magnitude;

            if (linearMagnitude < linearThreshold) return;

            float   scaledMagnitude = linearMagnitude * rotationScale;
            Vector3 baseRotation    = transform.TransformVector(rotationAxis);
            Vector3 rotation        = baseRotation.normalized * scaledMagnitude;

            rb.AddTorque(rotation, ForceMode.Acceleration);

            LogInfo($"Angular Velocity: {rb.angularVelocity}, Torque applied: {rotation}, Linear Velocity: {rb.linearVelocity.magnitude}, Scaled Magnitude: {scaledMagnitude}, Base Rotation: {baseRotation}");
        }
    }
}