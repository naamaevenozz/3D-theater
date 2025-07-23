using System;
using System.Collections;
using Edelweiss.Core.Player;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerAssignments : MonoBehaviour
{
    public static PlayerAssignments Instance;

    [Header("Player Inputs")]
    public PlayerInput playerInput1;
    public PlayerInput playerInput2;

    [Header("Characters (must contain CSRootController)")]
    public GameObject character1;
    public GameObject character2;

    private PlayerInput inputOfChar1;
    private PlayerInput inputOfChar2;

    private CSRootController controller1;
    private CSRootController controller2;
    
    private bool confirm1 = false;
    private bool confirm2 = false;
    
    [Header("Scene Transition")]
    public string nextSceneName = "NextSceneName"; 
    public Animator transitionAnimator; 


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        inputOfChar1 = playerInput1;
        inputOfChar2 = playerInput2;

        controller1 = character1.GetComponent<CSRootController>();
        controller2 = character2.GetComponent<CSRootController>();

        if (controller1 == null || controller2 == null)
        {
            Debug.LogError("Missing CSRootController on one of the characters! Please assign character1 and character2 to the objects with CSRootController directly.");
            return;
        }

        AssignInputs();
    }

    private void AssignInputs()
    {
        controller1.SetPlayerInput(inputOfChar1);
        controller2.SetPlayerInput(inputOfChar2);
    }

    public void SwapControl()
    {
        Debug.Log("SwapControl called");

        var temp = inputOfChar1;
        inputOfChar1 = inputOfChar2;
        inputOfChar2 = temp;

        AssignInputs();

        Debug.Log("Swap done!");
    }
    
    public void Confirm(PlayerInput input)
    {
        if (input == inputOfChar1 && !confirm1)
        {
            confirm1 = true;
            Debug.Log("Player 1 confirmed.");
        }
        else if (input == inputOfChar2 && !confirm2)
        {
            confirm2 = true;
            Debug.Log("Player 2 confirmed.");
        }
        else
        {
            return; 
        }

        if (confirm1 && confirm2)
        {
            Debug.Log("Both players confirmed. Starting scene transition...");
            StartCoroutine(TransitionThenLoadScene());
        }
    }

    private IEnumerator TransitionThenLoadScene()
    {
        if (transitionAnimator != null)
        {
            transitionAnimator.SetTrigger("Transition");

            yield return null; 
            var stateInfo = transitionAnimator.GetCurrentAnimatorStateInfo(0);
            yield return new WaitForSeconds(stateInfo.length);
        }
        else
        {
            Debug.LogWarning("No transition animator assigned. Loading scene immediately.");
        }

        SceneManager.LoadScene(nextSceneName);
    }



    public void ResetConfirmations()
    {
        confirm1 = false;
        confirm2 = false;
    }
}