// using _EDELWEISS.Scripts.Core.Player;
// using Edelweiss.Player;
// using UnityEngine;
// using UnityEngine.InputSystem;
//
// public class CSCharacterSide : MonoBehaviour
// {
//     [Header("References")]
//     [SerializeField] private GameObject characterObject;
//
//     private Gamepad controllingGamepad;
//
//     public bool CanBeControlled()
//     {
//         return controllingGamepad == null;
//     }
//
//     public void AssignControl(Gamepad gamepad)
//     {
//         if (controllingGamepad != null) return;
//
//         controllingGamepad = gamepad;
//
//         var playerInput = characterObject.GetComponent<PlayerInput>();
//         if (playerInput != null)
//             playerInput.enabled = true;
//
//         var rootController = characterObject.GetComponentInChildren<CSRootController>();
//         if (rootController != null)
//         {
//             rootController.enabled = true;
//             rootController.SetInputDevice(gamepad);
//         }
//
//         var armsController = characterObject.GetComponent<CSArmsController>();
//         if (armsController != null)
//         {
//             armsController.enabled = true;
//             armsController.SetInputDevice(gamepad);
//         }
//     }
//
//     public void RemoveCandidate(Gamepad gamepad)
//     {
//         if (controllingGamepad != gamepad) return;
//
//         controllingGamepad = null;
//
//         var playerInput = characterObject.GetComponent<PlayerInput>();
//         if (playerInput != null)
//             playerInput.enabled = false;
//
//         var rootController = characterObject.GetComponentInChildren<CSRootController>();
//         if (rootController != null)
//         {
//             rootController.SetInputDevice(null);
//             rootController.enabled = false;
//         }
//
//         var armsController = characterObject.GetComponent<CSArmsController>();
//         if (armsController != null)
//         {
//             armsController.SetInputDevice(null);
//             armsController.enabled = false;
//         }
//     }
//     public Gamepad GetControllingGamepad()
//     {
//         return controllingGamepad;
//     }
// }