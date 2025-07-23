using UnityEngine;
using RootMotion.FinalIK;

namespace RootMotion.Demos {

    /// <summary>
    /// Making a character hang from its back (pelvis) and swing about it while maintaining animation.
    /// </summary>
    public class PendulumExample : MonoBehaviour {

        [Tooltip("The master weight of this script.")]
        [Range(0f, 1f)] public float weight = 1f;

        [Tooltip("Multiplier for the distance of the root to the target.")]
        public float hangingDistanceMlp = 1.3f;

        [Tooltip("Where does the root of the character land when weight is blended out?")]
        [HideInInspector] public Vector3 rootTargetPosition;

        [Tooltip("How is the root of the character rotated when weight is blended out?")]
        [HideInInspector] public Quaternion rootTargetRotation;

        [Tooltip("The fixed point in world space that the back will hang from.")]
        public Transform target;

        [Tooltip("Your ragdoll pelvis (back) bone Rigidbody.")]
        public Transform pelvisTarget;

        // These can stay if you still want to drive your arms/legs from the ragdoll bones:
        public Transform leftHandTarget;
        public Transform rightHandTarget;
        public Transform leftFootTarget;
        public Transform rightFootTarget;
        public Transform bodyTarget;
        public Transform headTarget;

        /// <summary>
        /// The “down” direction in pelvis-local space (usually Vector3.right for a side-view).
        /// </summary>
        public Vector3 pelvisDownAxis = Vector3.right;

        private FullBodyBipedIK ik;
        private Quaternion   rootRelativeToPelvis;
        private Vector3      pelvisToRoot;
        private float        lastWeight;

        void Start() {
            ik = GetComponent<FullBodyBipedIK>();

            // ——— Hang from the pelvis rigidbody, not the left hand ———
            var joint = target.gameObject.AddComponent<FixedJoint>();
            joint.connectedBody = pelvisTarget.GetComponent<Rigidbody>();

            // Cache the offsets for blending the root back to animation
            rootRelativeToPelvis = Quaternion.Inverse(pelvisTarget.rotation) * transform.rotation;
            pelvisToRoot          = Quaternion.Inverse(ik.references.pelvis.rotation)
                                  * (transform.position - ik.references.pelvis.position);

            rootTargetPosition = transform.position;
            rootTargetRotation = transform.rotation;
            lastWeight         = weight;
        }

        void LateUpdate() {
            // If we’re not hanging, just update our “rest” pose
            if (weight <= 0f) {
                rootTargetPosition = transform.position;
                rootTargetRotation = transform.rotation;
                lastWeight = weight;
                return;
            }

            lastWeight = weight;

            // ——— Blend the overall root transform based on the ragdoll pelvis ———
            Vector3 desiredRootPos = pelvisTarget.position + 
                                     pelvisTarget.rotation * pelvisToRoot * hangingDistanceMlp;
            transform.position = Vector3.Lerp(rootTargetPosition, desiredRootPos, weight);

            Quaternion desiredRootRot = pelvisTarget.rotation * rootRelativeToPelvis;
            transform.rotation = Quaternion.Lerp(rootTargetRotation, desiredRootRot, weight);

            // ——— Compute a “hanging down” direction in world space ———
            Vector3 downDir = ik.references.pelvis.rotation * pelvisDownAxis;

            // ——— Swing both arms to match the ragdoll targets ———
            // Left arm
            var leftArmRot = Quaternion.FromToRotation(
                downDir, 
                leftHandTarget.position - headTarget.position
            );
            ik.references.leftUpperArm.rotation =
                Quaternion.Lerp(Quaternion.identity, leftArmRot, weight) 
                * ik.references.leftUpperArm.rotation;

            // Right arm
            var rightArmRot = Quaternion.FromToRotation(
                downDir,
                rightHandTarget.position - headTarget.position
            );
            ik.references.rightUpperArm.rotation =
                Quaternion.Lerp(Quaternion.identity, rightArmRot, weight)
                * ik.references.rightUpperArm.rotation;

            // ——— Legs as before ———
            var leftLegRot = Quaternion.FromToRotation(
                downDir, 
                leftFootTarget.position - bodyTarget.position
            );
            ik.references.leftThigh.rotation =
                Quaternion.Lerp(Quaternion.identity, leftLegRot, weight)
                * ik.references.leftThigh.rotation;

            var rightLegRot = Quaternion.FromToRotation(
                downDir,
                rightFootTarget.position - bodyTarget.position
            );
            ik.references.rightThigh.rotation =
                Quaternion.Lerp(Quaternion.identity, rightLegRot, weight)
                * ik.references.rightThigh.rotation;
        }
    }
}
