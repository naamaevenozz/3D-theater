using System;
using Edelweiss.Core;
using Edelweiss.PhysicsSystem;
using Edelweiss.Utils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.XR;

namespace Edelweiss.Core.Player
{
    public class RootController : EdelMono
    {
        [Header("Input")]
        [SerializeField]
        private PlayerInput input;

        [Header("Root")]
        [SerializeField]
        private Rigidbody rootRg;

        // [SerializeField]
        //private CharacterController cc;
    
        [Header("Parameters")]
        [SerializeField]
        private float moveSpeed = 5f;

        [SerializeField]
        private float baseVel = 5f;

        [SerializeField]
        private Transform rootTransform;

        [FormerlySerializedAs("rootPosTreshold")]
        [SerializeField]
        private float rootPosThreshold = 0.8f;

        [SerializeField]
        private ShockwaveReceiver shockwaveReceiver;

        private Vector3 targetDirection;
        private Vector2 moveInput;

        private void Awake()
        {
            this.ComponentScan()
                .IncludeChildren()
                .SetSingle(ref shockwaveReceiver);
        }

        private void Update()
        {
            HandleInput();
        }

        private void FixedUpdate()
        {
            if (shockwaveReceiver is { IsRecovering: true })
            {
                return;
            }

            //MoveRoot();
            //Vector3 targetVel = targetDirection * moveSpeed;

            Vector3 targetVel = new Vector3(moveInput.x, moveInput.y, 0) * moveSpeed;
            //Vector3 velChange = targetVel == Vector3.zero ?  new Vector3(0,baseVel,0) : targetVel - rootRg.linearVelocity;
            Vector3 velChange;
            if (targetVel.magnitude <= 0.1f && rootTransform.position.y <= rootPosThreshold)
            {
                velChange = new Vector3(0f, baseVel, 0f);
            }
            else
            {
                velChange = targetVel - rootRg.linearVelocity;
            }

            /*print(targetVel.magnitude);
            print(velChange);*/
            /*print(rootTransform.position.y);
            print(rootTransform.position.y <= rootPosTreshold);*/
            rootRg.AddForce(velChange, ForceMode.VelocityChange);
        }

        private void MoveRoot()
        {
            Vector3 desiredPosition = rootRg.position + targetDirection * (moveSpeed * Time.fixedDeltaTime);
            //cc.Move( targetDirection * (moveSpeed * Time.fixedDeltaTime));
            //rootRg.MovePosition(cc.transform.position);

            rootRg.MovePosition(desiredPosition);
        }

        private void HandleInput()
        {
            Vector2 rawInput = input.actions["MoveBodyJoyStick"].ReadValue<Vector2>();
            float   deadZone = 0.1f;
            targetDirection = rawInput.magnitude < deadZone
                                  ? Vector3.zero
                                  : new Vector3(rawInput.x, rawInput.y, 0).normalized;
            //print(targetDirection);
            moveInput = rawInput.magnitude < deadZone ? Vector2.zero : rawInput;
        }
    }
}