using System.Collections;
using Edelweiss.Core;
using Edelweiss.Damage;
using UnityEngine;

namespace Edelweiss.Core.Player
{
    public class PlayerKo : EdelMono
    {
        [Header("Hook & Chest")]
        [SerializeField]
        private Rigidbody hookRb;

        [SerializeField]
        private GameObject chest;

        [SerializeField]
        private Transform chestBone;

        [Header("KO Settings")]
        [Tooltip("Damage threshold to trigger KO")]
        [SerializeField]
        private float damageThreshold = 10f;

        [Tooltip("Seconds to stay ragdoll")]
        [SerializeField]
        private float koDuration = 3f;

        [Tooltip("Extra wait before re‐attach for physics to settle")]
        [SerializeField]
        private float reattachDelay = 0.1f;

        [Header("Joint Settings")]
        [Tooltip("Force that would break the joint (unused when breaking manually)")]
        [SerializeField]
        private float breakForce = 200f;

        [SerializeField]
        private float breakTorque = 200f;

        [Tooltip("Angular spring & damper for the joint when reattached")]
        [SerializeField]
        private float angularSpring = 80f;

        [SerializeField]
        private float angularDamper = 20f;

        [Tooltip("Angular limits for X (twist) axis")]
        [SerializeField]
        private float lowTwistLimit = -10f;

        [SerializeField]
        private float highTwistLimit = 10f;

        [Tooltip("Swing cone limits Y/Z")]
        [SerializeField]
        private float swingYLimit = 15f;

        [SerializeField]
        private float swingZLimit = 15f;

        [Header("Optional Controllers to Disable During KO")]
        [SerializeField]
        private MonoBehaviour[] disableDuringKO;

        // The actual joint instance
        [SerializeField]
        private ConfigurableJoint _joint;

        private bool _isKO = false;

        private void OnEnable()
        {
            GameEvents.HitRegistered += HandleKO;
        }

        private void OnDisable()
        {
            GameEvents.HitRegistered -= HandleKO;
        }

        private void HandleKO(HitContext hitContext)
        {
            print("damage is: " + hitContext.Damage);
            if (_isKO) return;                       // already KO’d
            if (hitContext.Damage < damageThreshold) // not enough damage
                return;

            _isKO = true;
            print("deleting joint");
            DeleteJoint();
            StartCoroutine(KnockOutPlayer());
        }

        private void DeleteJoint()
        {
            if (IsNull(_joint)) return;

            print("joint deleted");
            Destroy(_joint);
            _joint = null;
        }

        private IEnumerator KnockOutPlayer()
        {
            // 1) Disable other controllers (movement/arms/etc.)
            foreach (var mb in disableDuringKO)
                if (mb != null)
                    mb.enabled = false;

            // 2) Wait ragdoll time
            yield return new WaitForSeconds(koDuration);

            // 3) Snap hook to wherever the chest ended up
            hookRb.position          = chestBone.position;
            chest.transform.position = chestBone.position;

            // 4) Wait for physics to settle
            yield return new WaitForSeconds(reattachDelay);

            // 5) Reattach joint
            AttachJoint();

            // 6) Re-enable controllers
            foreach (var mb in disableDuringKO)
                if (mb != null)
                    mb.enabled = true;

            _isKO = false;
        }

        private void AttachJoint()
        {
            // Create new ConfigurableJoint on the chest bone
            _joint               = chest.gameObject.AddComponent<ConfigurableJoint>();
            _joint.connectedBody = hookRb;

            // // (Optional) if you ever want to use automatic break:
            // _joint.breakForce  = breakForce;
            // _joint.breakTorque = breakTorque;

            // Lock translation
            _joint.xMotion = ConfigurableJointMotion.Locked;
            _joint.yMotion = ConfigurableJointMotion.Locked;
            _joint.zMotion = ConfigurableJointMotion.Locked;

            // Limit rotation
            _joint.angularXMotion = ConfigurableJointMotion.Limited;
            _joint.angularYMotion = ConfigurableJointMotion.Limited;
            _joint.angularZMotion = ConfigurableJointMotion.Limited;

            // Set twist limits
            _joint.lowAngularXLimit  = new SoftJointLimit { limit = lowTwistLimit };
            _joint.highAngularXLimit = new SoftJointLimit { limit = highTwistLimit };
            // Set swing limits
            _joint.angularYLimit = new SoftJointLimit { limit = swingYLimit };
            _joint.angularZLimit = new SoftJointLimit { limit = swingZLimit };

            // Set angular drives (spring + damper)
            var xDrive = new JointDrive
                         {
                             positionSpring = angularSpring,
                             positionDamper = angularDamper,
                             maximumForce   = Mathf.Infinity
                         };
            var yzDrive = xDrive; // same for both Y and Z
            _joint.angularXDrive  = xDrive;
            _joint.angularYZDrive = yzDrive;

            // Projection helps keep the joint from “popping” under loads
            _joint.projectionMode = JointProjectionMode.PositionAndRotation;
        }

        // (Optional) you can still catch OnJointBreak if you set breakForce/breakTorque
        private void OnJointBreak(float brokenForceValue)
        {
            // If you wanted to auto-KO on physics impulses, you could:
            // if (!_isKO && brokenForceValue >= breakForce)
            //     HandleKO(new HitContext { Damage = damageThreshold });
        }
    }
}