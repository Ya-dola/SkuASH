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

    [Header("Private")]
    public GameObject managers;

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
        Move();
    }

    private void Move()
    {
        charCtrl.Move(inputSysManager.mvmntVec3 * moveSpeed * Time.fixedDeltaTime);
    }

    // Applying Proper Gravity to Character Controller
    private void ApplyGravity()
    {
        if (charCtrl.isGrounded)
            inputSysManager.mvmntVec3.y = groundedGravity;
        else
            inputSysManager.mvmntVec3.y += (gravity * Time.fixedDeltaTime);
    }
}