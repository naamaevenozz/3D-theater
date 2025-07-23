using System;
using Edelweiss.Core;
using Edelweiss.PhysicsSystem;
using Edelweiss.Utils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;
using Debug = Edelweiss.Util.Debugging.Debug;

namespace Edelweiss.Core.Player
{
    public class CSRootController : EdelMono
    {
        [Header("Root")]
        [SerializeField]
        private Rigidbody rootRg;

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

        private PlayerInput input;

        private Vector3 targetDirection;
        private Vector2 moveInput;
        
        
        

        private void Awake()
        {
            this.ComponentScan()
                .IncludeChildren()
                .SetSingle(ref shockwaveReceiver);
        }

        private void OnEnable()
        {
            if (input != null)
            {
                input.actions["Swap"].performed += OnSwapPressed;
                input.actions["Confirm"].performed += OnConfirmPressed;
                
            }
        }

        private void OnDisable()
        {
            if (input != null)
            {
                input.actions["Swap"].performed -= OnSwapPressed;
                input.actions["Confirm"].performed -= OnConfirmPressed;
            }
        }

        private void Update()
        {
            if (input == null)
                return;

            HandleInput();
        }

        private void FixedUpdate()
        {
            if (shockwaveReceiver is { IsRecovering: true })
                return;

            Vector3 targetVel = new Vector3(moveInput.x, moveInput.y, 0) * moveSpeed;
            Vector3 velChange;

            if (targetVel.magnitude <= 0.1f && rootTransform.position.y <= rootPosThreshold)
            {
                velChange = new Vector3(0f, baseVel, 0f);
            }
            else
            {
                velChange = targetVel - rootRg.linearVelocity;
            }

            rootRg.AddForce(velChange, ForceMode.VelocityChange);
        }

        private void HandleInput()
        {
            Vector2 rawInput = input.actions["MoveBodyJoyStick"].ReadValue<Vector2>();
            float deadZone = 0.1f;
            targetDirection = rawInput.magnitude < deadZone
                ? Vector3.zero
                : new Vector3(rawInput.x, rawInput.y, 0).normalized;

            moveInput = rawInput.magnitude < deadZone ? Vector2.zero : rawInput;
        }

        public void SetPlayerInput(PlayerInput newInput)
        {
            if (input != null)
            {
                input.actions["Swap"].performed -= OnSwapPressed;
                input.actions["Confirm"].performed -= OnConfirmPressed;
            }

            input = newInput;

            if (input != null)
            {
                input.actions["Swap"].performed += OnSwapPressed;
                input.actions["Confirm"].performed += OnConfirmPressed;
            }
        }

        private void OnSwapPressed(InputAction.CallbackContext ctx)
        {
            Debug.Log("Swap Pressed");
            PlayerAssignments.Instance?.SwapControl();
        }

        private void OnConfirmPressed(InputAction.CallbackContext ctx)
        {
            Debug.Log("Confirm Pressed");
            PlayerAssignments.Instance?.Confirm(input);

        }
    }
}