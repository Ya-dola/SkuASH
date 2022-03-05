using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSysManager : MonoBehaviour
{
    public InputSys inputSys;

    public Vector2 input;
    public Vector3 mvmntVec3;
    public bool isMoving;

    // [Header("Debug")]
    // public float inputMag;

    private void Awake()
    {
        inputSys = new InputSys();

        // When Buttons are Pressed
        inputSys.PlayerControls.Move.performed += context => { MoveUpdate(context.ReadValue<Vector2>()); };
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void MoveUpdate(Vector2 direction)
    {
        // Getting Normalized Vec2 from WASD/Arrows
        input = direction;

        // Mapping Vec2 to Vec3
        mvmntVec3.x = input.x;
        mvmntVec3.z = input.y;

        // To check if Character is moving or not
        // inputMag = input.magnitude;
        if (input.magnitude == 0f)
            isMoving = false;
        else
            isMoving = true;
    }

    private void OnEnable()
    {
        inputSys.PlayerControls.Enable();
    }

    private void OnDisable()
    {
        inputSys.PlayerControls.Disable();
    }
}