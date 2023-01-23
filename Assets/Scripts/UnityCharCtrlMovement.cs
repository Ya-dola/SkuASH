using UnityEngine;

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

    private void FixedUpdate()
    {
        ApplyGravity();

        // Stops Execution here if not Moving
        if (!inputSysManager.IsMoving)
            return;

        Move();
    }

    private void Move()
    {
        charCtrl.Move(inputSysManager.MvmntVec3 * (Time.fixedDeltaTime * moveSpeed));
    }

    // Applying Proper Gravity to Character Controller to keep it grounded
    private void ApplyGravity()
    {
        if (charCtrl.isGrounded)
            inputSysManager.SetMvmntVec3Y(groundedGravity);
        else
            inputSysManager.IncrMvmntVec3Y(gravity * Time.fixedDeltaTime);
    }
}