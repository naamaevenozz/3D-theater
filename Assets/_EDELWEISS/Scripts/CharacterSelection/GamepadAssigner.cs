// using _EDELWEISS.Scripts.Core.Player;
// using UnityEngine;
// using UnityEngine.InputSystem;
// using Edelweiss.Core.Player;
// using Edelweiss.Player;
//
// public class GamepadAssigner : MonoBehaviour
// {
//     public enum PlayerSide { Left, Right }
//     [SerializeField] private PlayerSide side;
//
//     private void Start()
//     {
//         Gamepad gamepad = side == PlayerSide.Left
//             ? PlayerAssignments.Instance.LeftPlayerGamepad
//             : PlayerAssignments.Instance.RightPlayerGamepad;
//
//         if (gamepad == null)
//         {
//             Debug.LogError($"[GamepadAssigner] No gamepad assigned for {side} side!");
//             return;
//         }
//
//         var oldRoot = GetComponent<CSRootController>();
//         if (oldRoot != null) oldRoot.enabled = false;
//
//         var oldArms = GetComponent<CSArmsController>();
//         if (oldArms != null) oldArms.enabled = false;
//
//         var newRoot = GetComponentInChildren<RootController>(true);
//         if (newRoot != null) EnablePlayerInput(newRoot.GetComponent<PlayerInput>(), gamepad);
//
//         var newArms = GetComponent<ArmsController>();
//         if (newArms != null) EnablePlayerInput(newArms.GetComponent<PlayerInput>(), gamepad);
//     }
//
//     private void EnablePlayerInput(PlayerInput playerInput, Gamepad gamepad)
//     {
//         if (playerInput == null || gamepad == null) return;
//
//         playerInput.SwitchCurrentControlScheme("Gamepad", gamepad);
//         playerInput.enabled = true;
//     }
//
// }