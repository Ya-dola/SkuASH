using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnityCharCtrlMovement : MonoBehaviour
{
    [Header("Attributes")]
    public float moveSpeed;

    [Header("Physics")]
    public float groundedGravity = -0.05f;

    public float gravity = -9.81f;

    [Header("Managers")]
    public GameObject managers;

    [Header("Private")]
    public CharacterController charCtrl;

    public InputSysManager inputSysManager;

    private void Awake()
    {
        inputSysManager = managers.GetComponentInChildren<InputSysManager>();
        charCtrl = GetComponent<CharacterController>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        ApplyGravity();

        // Stops Execution here if not Moving
        if (!inputSysManager.isMoving)
            return;

        Move();
    }

    private void Move()
    {
        charCtrl.Move(inputSysManager.mvmntVec3 * Time.fixedDeltaTime * moveSpeed);
    }

    // Applying Proper Gravity to Character Controller to keep it grounded
    private void ApplyGravity()
    {
        if (charCtrl.isGrounded)
            inputSysManager.mvmntVec3.y = groundedGravity;
        else
            inputSysManager.mvmntVec3.y += (gravity * Time.fixedDeltaTime);
    }
}