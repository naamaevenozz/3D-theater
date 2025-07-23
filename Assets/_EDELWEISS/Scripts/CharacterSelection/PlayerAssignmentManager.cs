// using System;
// using Edelweiss.Core.Player;
// using UnityEngine;
// using UnityEngine.InputSystem;
//
// public class PlayerAssignments : MonoBehaviour
// {
//     public static PlayerAssignments Instance;
//
//     public PlayerInput playerInput1;
//     public PlayerInput playerInput2;
//
//     public GameObject character1;
//     public GameObject character2;
//
//     private PlayerInput inputOfChar1;
//     private PlayerInput inputOfChar2;
//
//     private void Awake()
//     {
//         if (Instance != null && Instance != this)
//         {
//             Destroy(gameObject);
//             return;
//         }
//
//         Instance = this;
//         DontDestroyOnLoad(gameObject);
//     }
//
//     private void Start()
//     {
//         inputOfChar1 = playerInput1;
//         inputOfChar2 = playerInput2;
//
//         AssignInputs();
//     }
//
//     private void AssignInputs()
//     {
//         var controller1 = character1.GetComponent<CSRootController>();
//         var controller2 = character2.GetComponent<CSRootController>();
//
//         if (controller1 != null)
//             controller1.SetPlayerInput(inputOfChar1);
//
//         if (controller2 != null)
//             controller2.SetPlayerInput(inputOfChar2);
//     }
//
//     public void SwapControl()
//     {
//         Debug.Log("SwapControl called");
//
//         var temp = inputOfChar1;
//         inputOfChar1 = inputOfChar2;
//         inputOfChar2 = temp;
//
//         AssignInputs();
//
//         Debug.Log("Swap done: char1 now has " + inputOfChar1.name + ", char2 has " + inputOfChar2.name);
//     }
// }
