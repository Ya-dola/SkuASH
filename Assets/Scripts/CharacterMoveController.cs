using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMoveController : MonoBehaviour
{
    public PlayerInput playerInput;

    public Vector2 input;
    public Vector3 currentMovement;
    public bool isMovementPressed;

    private void Awake()
    {
        playerInput = new PlayerInput();

        playerInput.CharacterControls.Move.started += context =>
        {
            // Getting Normalized Vec2 from WASD/Arrows
            input = context.ReadValue<Vector2>();

            // Mapping Vec2 to Vec3
            currentMovement.x = input.x;
            currentMovement.z = input.y;

            isMovementPressed = input.x != 0 || input.y != 0;
        };
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnEnable()
    {
        playerInput.CharacterControls.Enable();
    }

    private void OnDisable()
    {
        playerInput.CharacterControls.Disable();
    }
}