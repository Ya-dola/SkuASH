using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class CharAnimCtrl : MonoBehaviour
{
    [Header("Attributes")]
    public bool animIsMoving;

    public float rotationSpeed;

    [Header("Managers")]
    public GameObject managers;

    [Header("Private")]
    public Animator animator;

    public InputSysManager inputSysManager;

    private Vector3 posToLookAt;
    private Quaternion currentRot;
    private Quaternion targetRot;

    private void Awake()
    {
        inputSysManager = managers.GetComponentInChildren<InputSysManager>();
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ApplyAnimations();

        // To Ensure The Character Model is always at Vec3.zero
        ResetLocalPos();

        // Stops Execution here if not Moving
        if (!inputSysManager.isMoving)
            return;

        Rotate();
    }

    private void ResetLocalPos()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, 1f);
    }

    private void ApplyAnimations()
    {
        animIsMoving = animator.GetBool("isMoving");

        animator.SetBool("isMoving", inputSysManager.isMoving);

        // if (inputSysManager.isMoving)
        // {
        //     animator.SetBool("isMoving", true);
        // }
        // else
        // {
        //     animator.SetBool("isMoving", false);
        // }
    }

    private void Rotate()
    {
        posToLookAt.x = inputSysManager.mvmntVec3.x;
        posToLookAt.y = 0f;
        posToLookAt.z = inputSysManager.mvmntVec3.z;

        currentRot = transform.rotation;
        targetRot = Quaternion.LookRotation(posToLookAt);

        transform.rotation = Quaternion.Slerp(currentRot, targetRot, Time.fixedDeltaTime * rotationSpeed);
    }
}