using System;
using Edelweiss.Core;
using Edelweiss.PhysicsSystem;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class RootMovement : EdelMono
{
    [SerializeField]
    private float moveSpeed = 5f;

    //[SerializeField] private float maxDistanceFromStart = 3f; // טווח תנועה מקסימלי
    [SerializeField]
    private CharacterController cc;

    [SerializeField]
    private KeyCode moveUp;

    [SerializeField]
    private KeyCode moveDown;

    [SerializeField]
    private KeyCode moveLeft;

    [SerializeField]
    private KeyCode moveRight;

    [SerializeField]
    private PlayerInput input;

    private Vector3 startPosition;
    private Vector3 targetDirection;

    void Awake()
    {
        if (cc == null) cc = GetComponent<CharacterController>();
        startPosition = transform.position;
    }

    private void Update()
    {
        HandleInput();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (targetDirection.magnitude < .05f) return;

        Vector3 motion      = targetDirection * (moveSpeed * Time.deltaTime);
        Vector3 newPosition = cc.transform.position + motion;

        //print($"Old Position: {cc.transform.position}\tNew Position: {newPosition}\tMotion: {motion}");

        cc.Move(motion);


        /*if (Vector3.Distance(startPosition, newPosition) <= maxDistanceFromStart)
        {
            cc.Move(motion);
        }*/
    }

    private void HandleInput()
    {
        Vector2 rawInput = input.actions["MoveBody"].ReadValue<Vector2>().normalized;
        targetDirection = new(rawInput.x, rawInput.y, 0);
    }
}