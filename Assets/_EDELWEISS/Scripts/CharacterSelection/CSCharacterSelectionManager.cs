// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.InputSystem;
// using UnityEngine.SceneManagement;
//
// public class CharacterSelectionManager : MonoBehaviour
// {
//     public CSCharacterSide leftSide;
//     public CSCharacterSide rightSide;
//
//     private Dictionary<Gamepad, SideChoice> playerAssignments = new();
//     private HashSet<Gamepad> confirmedGamepads = new();
//
//     private bool leftConfirmed = false;
//     private bool rightConfirmed = false;
//     private bool transitionStarted = false;
//
//     private void Update()
//     {
//         foreach (var gamepad in Gamepad.all)
//         {
//             if (!playerAssignments.ContainsKey(gamepad))
//                 playerAssignments[gamepad] = SideChoice.Unassigned;
//
//             if (gamepad.dpad.right.wasPressedThisFrame)
//                 TryAssignToSide(gamepad, SideChoice.Right);
//             else if (gamepad.dpad.left.wasPressedThisFrame)
//                 TryAssignToSide(gamepad, SideChoice.Left);
//
//             if (playerAssignments[gamepad] != SideChoice.Unassigned && gamepad.buttonSouth.wasPressedThisFrame) // A button
//             {
//                     ConfirmSelection(gamepad);
//             }
//         }
//     }
//
//     private void TryAssignToSide(Gamepad gamepad, SideChoice targetSide)
//     {
//         if (playerAssignments[gamepad] == targetSide)
//             return;
//
//         if (playerAssignments[gamepad] == SideChoice.Left)
//         {
//             leftSide.RemoveCandidate(gamepad);
//             leftConfirmed = false;
//         }
//         else if (playerAssignments[gamepad] == SideChoice.Right)
//         {
//             rightSide.RemoveCandidate(gamepad);
//             rightConfirmed = false;
//         }
//
//         CSCharacterSide target = (targetSide == SideChoice.Left) ? leftSide : rightSide;
//
//         if (target.CanBeControlled())
//         {
//             target.AssignControl(gamepad);
//             playerAssignments[gamepad] = targetSide;
//
//             Debug.Log($"{gamepad.displayName} selected side {targetSide}");
//         }
//         else
//         {
//             Debug.Log($"{gamepad.displayName} tried to select {targetSide}, but it's already taken.");
//         }
//     }
//
//     private void ConfirmSelection(Gamepad gamepad)
//     {
//         if (confirmedGamepads.Contains(gamepad))
//             return;
//
//         SideChoice choice = playerAssignments[gamepad];
//
//         if (choice == SideChoice.Left)
//         {
//             leftConfirmed = true;
//             PlayerAssignments.Instance.LeftPlayerGamepad = gamepad;
//         }
//         else if (choice == SideChoice.Right)
//         {
//             rightConfirmed = true;
//             PlayerAssignments.Instance.RightPlayerGamepad = gamepad;
//         }
//         else return;
//
//         confirmedGamepads.Add(gamepad);
//         Debug.Log($"{gamepad.displayName} confirmed selection for {choice}");
//
//         CheckStartGame();
//     }
//
//     private void CheckStartGame()
//     {
//         if (leftConfirmed && rightConfirmed && !transitionStarted)
//         {
//             transitionStarted = true;
//             Debug.Log("Both players confirmed! Loading next scene...");
//             SceneManager.LoadScene("GameWorld");
//         }
//     }
//
//     private enum SideChoice
//     {
//         Unassigned,
//         Left,
//         Right
//     }
// }
