using UnityEngine;
using UnityEngine.Serialization;

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

    [FormerlySerializedAs("inputSysManager")]
    public InputSysMan inputSysMan;

    private void Awake()
    {
        inputSysMan = managers.GetComponentInChildren<InputSysMan>();
        charCtrl = GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {
        ApplyGravity();

        // Stops Execution here if not Moving
        if (!inputSysMan.IsMoving)
            return;

        Move();
    }

    private void Move()
    {
        charCtrl.Move(inputSysMan.MvmntVec3 * (Time.fixedDeltaTime * moveSpeed));
    }

    // Applying Proper Gravity to Character Controller to keep it grounded
    private void ApplyGravity()
    {
        if (charCtrl.isGrounded)
            inputSysMan.SetMvmntVec3Y(groundedGravity);
        else
            inputSysMan.IncrMvmntVec3Y(gravity * Time.fixedDeltaTime);
    }
}