using System;
using Edelweiss.Core;
using Edelweiss.Utils;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Edelweiss.Player
{
    public class ArmsController : EdelMono
    {
        [Header("Target Transforms")]
        [SerializeField]
        private Transform rootTarget;

        [SerializeField]
        private Transform leftHandTarget;

        [SerializeField]
        private Transform rightHandTarget;

        [Header("Control Settings")]
        [SerializeField]
        private float maxHandDistance = 1f;

        [Header("Springs")]
        [SerializeField]
        private SpringJoint leftSpring;

        [SerializeField]
        private SpringJoint rightSpring;

        [SerializeField]
        private float handSpringForce = 100f;

        [SerializeField]
        private float handSpringDamper = 20f;

        private Vector3 restingDirection = Vector3.down;
        private float   restSpring       = 100f;
        private float   restDamper       = 20f;

        [Header("Input")]
        [SerializeField]
        private PlayerInput input;

        private Vector3 lastRootPos;
        private Vector3 leftAnchor;
        private Vector3 rightAnchor;

        private bool holdLeftTrigger  = false;
        private bool holdRightTrigger = false;

        private bool HoldLeftTrigger
        {
            get => holdLeftTrigger;
            set
            {
                if (value == holdLeftTrigger)
                    return;

                holdLeftTrigger = value;

                if (holdLeftTrigger)
                    OnLeftHandActivated.Invoke();
                else
                    OnLeftHandDeactivated.Invoke();
            }
        }

        private bool HoldRightTrigger
        {
            get => holdRightTrigger;
            set
            {
                if (value == holdRightTrigger)
                    return;
                holdRightTrigger = value;
                if (holdRightTrigger)
                    OnRightHandActivated.Invoke();
                else
                    OnRightHandDeactivated.Invoke();
            }
        }

        private Vector2 rotateInput;
        private Vector3 leftDir;
        private Vector3 rightDir;

        [SerializeField]
        private EdelEvent OnRightHandActivated = new();

        [SerializeField]
        private EdelEvent OnRightHandDeactivated = new();

        [SerializeField]
        private EdelEvent OnLeftHandActivated = new();

        [SerializeField]
        private EdelEvent OnLeftHandDeactivated = new();

        void Start()
        {
            lastRootPos = rootTarget.position;
            leftAnchor  = leftHandTarget.position;
            rightAnchor = rightHandTarget.position;
        }


        private void Update()
        {
            HandleInput();
        }

        void LateUpdate()
        {
            // 3) Move anchors with root
            Vector3 delta = rootTarget.position - lastRootPos;
            leftAnchor  += delta;
            rightAnchor += delta;
            lastRootPos =  rootTarget.position;

            // 4) Compute each arm’s direction
            leftDir  = ComputeStretchDir(HoldLeftTrigger);
            rightDir = ComputeStretchDir(HoldRightTrigger);

            /*// 5) Apply springs on only if that arm is active
            leftSpring.spring  = (leftDir  != Vector3.zero) ? handSpringForce : 0f;
            leftSpring.damper  = (leftDir  != Vector3.zero) ? handSpringDamper : 0f;
            rightSpring.spring = (rightDir != Vector3.zero) ? handSpringForce : 0f;
            rightSpring.damper = (rightDir != Vector3.zero) ? handSpringDamper : 0f;*/


            if (leftDir != Vector3.zero)
            {
                if (holdLeftTrigger)
                {
                    leftSpring.spring = handSpringForce;
                    leftSpring.damper = handSpringDamper;
                }
                else
                {
                    leftSpring.spring = restSpring;
                    leftSpring.damper = restDamper;
                }
            }
            else
            {
                leftSpring.spring = 0f;
                leftSpring.damper = 0f;
            }

            // RIGHT
            if (rightDir != Vector3.zero)
            {
                if (holdRightTrigger)
                {
                    rightSpring.spring = handSpringForce;
                    rightSpring.damper = handSpringDamper;
                }
                else
                {
                    rightSpring.spring = restSpring;
                    rightSpring.damper = restDamper;
                }
            }
            else
            {
                rightSpring.spring = 0f;
                rightSpring.damper = 0f;
            }

            // 6) Move the hooks toward their circle positions
            MoveHand(leftHandTarget,  leftAnchor,  leftDir);
            MoveHand(rightHandTarget, rightAnchor, rightDir);
        }

        /*private Vector3 ComputeStretchDir(bool isStretched)
        {
            if (!isStretched)
                return Vector3.zero;

                if (rotateInput.magnitude >= 0.2f)
                    return new Vector3(rotateInput.x, rotateInput.y, 0f).normalized;

            return Vector3.zero;
        }*/


        private Vector3 ComputeStretchDir(bool isStretched)
        {
            if (isStretched && rotateInput.magnitude >= 0.2f)
                return new Vector3(rotateInput.x, rotateInput.y, 0f).normalized;

            /*// 2) If punching but no stick input, hold up
            if (isStretched)
                return Vector3.up;*/

            // 3) Otherwise (idle), rest at your chosen direction
            return restingDirection.normalized;
        }

        private void MoveHand(Transform hand, Vector3 anchor, Vector3 dir)
        {
            if (dir == Vector3.zero)
                return;

            Vector3 desired = anchor + dir * maxHandDistance;

            hand.position = desired;
        }

        private void HandleInput()
        {
            Vector2 rawInput = input.actions["RotateHands"].ReadValue<Vector2>();
            float   deadZone = 0.2f;
            rotateInput = rawInput.magnitude < deadZone
                              ? Vector3.zero
                              : new Vector3(rawInput.x, rawInput.y, 0).normalized;

            HoldLeftTrigger  = input.actions["LeftPunch"].ReadValue<float>()  > 0.1f;
            HoldRightTrigger = input.actions["RightPunch"].ReadValue<float>() > 0.1f;
        }
    }
}