using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField]
    private float speed = 6f;

    [SerializeField]
    private float turnSpeed = 360f;

    [Header("Required Objects")]
    [SerializeField]
    private Rigidbody rb;

    [SerializeField]
    private InputSysManager inputSysMan;

    private Vector3 _inputVec;

    // Update is called once per frame
    void Update()
    {
        LookInDir();
    }

    private void FixedUpdate()
    {
        MoveInDir();
    }

    private void LookInDir()
    {
        if (inputSysMan.MvmntVec3 == Vector3.zero) return;

        var position = transform.position;
        var relativeDir = (position + inputSysMan.MvmntVec3.Vec3ToIso()) - position;
        var lookRot = Quaternion.LookRotation(relativeDir, Vector3.up);

        // Sharp Turns - 8 Directional
        // transform.rotation = lookRot;

        // Smooth Turns - Actual Rotation 
        transform.rotation = Quaternion.RotateTowards(transform.rotation,
            lookRot, turnSpeed * Time.deltaTime);
    }

    private void MoveInDir()
    {
        var transform1 = transform;
        rb.MovePosition(transform1.position +
                        transform1.forward * (inputSysMan.MvmntVec3.magnitude * (speed * Time.deltaTime)));
    }
}