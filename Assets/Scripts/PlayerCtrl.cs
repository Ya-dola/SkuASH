using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerCtrl : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField]
    private float speed = 6f;

    [SerializeField]
    private float turnSpeed = 360f;

    [Header("Required Objects")]
    [SerializeField]
    private InputSysMan inputSysMan;

    private Rigidbody _rb;
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
        var relativeDir = (position + inputSysMan.MvmntVec3) - position;
        var lookRot = Quaternion.LookRotation(relativeDir, Vector3.up);

        // Sharp Turns - 8 Directional
        // transform.rotation = lookRot;

        // Smooth Turns - Actual Rotation 
        transform.rotation = Quaternion.RotateTowards(transform.rotation,
            lookRot, turnSpeed * Time.deltaTime);
    }

    private void MoveInDir()
    {
        var trans = transform;
        _rb.MovePosition(trans.position + trans.forward *
            (inputSysMan.MvmntVec3.magnitude * (speed * Time.deltaTime)));
    }

    private void OnEnable()
    {
        _rb = GetComponent<Rigidbody>();
    }
}