using System;
using Edelweiss.Core;
using UnityEngine;

namespace _EDELWEISS.Scripts.Core.Player
{
    public class StiffenShoulder : EdelMono
    {
        [SerializeField] private ConfigurableJoint leftJoint;
        [SerializeField] private ConfigurableJoint rightJoint;

        private void Start()
        {
            DoStiffenShoulder(leftJoint);
            DoStiffenShoulder(rightJoint);
        }

        private void DoStiffenShoulder(ConfigurableJoint shoulder)
        {
            // Lock translation entirely
            shoulder.xMotion = ConfigurableJointMotion.Locked;
            shoulder.yMotion = ConfigurableJointMotion.Locked;
            shoulder.zMotion = ConfigurableJointMotion.Locked;

            // Allow limited rotation (so you can still punch)
            shoulder.angularXMotion = ConfigurableJointMotion.Limited;
            shoulder.angularYMotion = ConfigurableJointMotion.Limited;
            shoulder.angularZMotion = ConfigurableJointMotion.Limited;

            // Small angular limits so only your hand-drive moves it far
            shoulder.lowAngularXLimit  = new SoftJointLimit { limit = -30f };
            shoulder.highAngularXLimit = new SoftJointLimit { limit =  100f };
            shoulder.angularYLimit     = new SoftJointLimit { limit = 30f };
            shoulder.angularZLimit     = new SoftJointLimit { limit = 180f };

            // Really strong drive to hold it there
            var drive = new JointDrive
            {
                positionSpring = 2000f,
                positionDamper = 300f,
                maximumForce   = Mathf.Infinity
            };
            shoulder.angularXDrive  = drive;
            shoulder.angularYZDrive = drive;

            // Projection keeps it from popping
            shoulder.projectionMode = JointProjectionMode.PositionAndRotation;
        }
    }
}