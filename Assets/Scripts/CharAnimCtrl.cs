using UnityEngine;
using UnityEngine.Serialization;

public class CharAnimCtrl : MonoBehaviour
{
    [Header("Attributes")]
    public bool animIsMoving;

    public float rotationSpeed;

    [Header("Managers")]
    public GameObject managers;

    [Header("Private")]
    public Animator animator;

    [FormerlySerializedAs("inputSysManager")]
    public InputSysMan inputSysMan;

    private Vector3 posToLookAt;
    private Quaternion currentRot;
    private Quaternion targetRot;

    private void Awake()
    {
        inputSysMan = managers.GetComponentInChildren<InputSysMan>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ApplyAnimations();

        // To Ensure The Character Model is always at Vec3.zero
        ResetLocalPos();

        // Stops Execution here if not Moving
        if (!inputSysMan.IsMoving)
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

        animator.SetBool("isMoving", inputSysMan.IsMoving);

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
        posToLookAt.x = inputSysMan.MvmntVec3.x;
        posToLookAt.y = 0f;
        posToLookAt.z = inputSysMan.MvmntVec3.z;

        currentRot = transform.rotation;
        targetRot = Quaternion.LookRotation(posToLookAt);

        transform.rotation = Quaternion.Slerp(currentRot, targetRot, Time.fixedDeltaTime * rotationSpeed);
    }
}