using System;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;

    [SerializeField]
    private float speed = 6f;

    [SerializeField]
    private float turnSpeed = 360f;

    [FormerlySerializedAs("inputSysManager")]
    [SerializeField]
    private InputSysManager inputSysMan;

    private Vector3 _inputVec;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Look();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Look()
    {
        if (inputSysMan.MvmntVec3 == Vector3.zero) return;

        var position = transform.position;
        var relativeDir = (position + inputSysMan.MvmntVec3) - position;
        var lookRot = Quaternion.LookRotation(relativeDir, Vector3.up);

        // Sharp Turns - 8 Directional
        // transform.rotation = lookRot;

        transform.rotation = Quaternion.RotateTowards(transform.rotation,
            lookRot, turnSpeed * Time.deltaTime);
    }

    private void Move()
    {
        var transform1 = transform;
        rb.MovePosition(transform1.position +
                        transform1.forward * (inputSysMan.MvmntVec3.magnitude * (speed * Time.deltaTime)));
    }
}