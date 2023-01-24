using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallPhysics : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField]
    private float reflectVel = 10f;

    [SerializeField]
    private float bounceHeight = 4f;

    [SerializeField]
    private float bounceSpeed = 3f;

    [Header("Required Objects")]
    [SerializeField]
    private Transform ballVisual;

    [SerializeField]
    [Range(0f, 1f)]
    [Tooltip("0 = regular bounce ignoring player | 1 = direct to the player")]
    private float dirBias = 0.4f;

    [SerializeField]
    [Tooltip("Just for debugging, adds some velocity during OnEnable")]
    // TODO - Adjust so Players start the movement towards wall 
    private Vector3 initialVelocity;

    [SerializeField]
    // TODO - Change to Position that can be set through method
    private Transform playerTransform;

    private Vector3 _lastFrameVelocity;
    private Rigidbody _rb;

    private void OnEnable()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.velocity = initialVelocity;
    }

    private void Update()
    {
        _lastFrameVelocity = _rb.velocity;

        BounceBall();
    }

    private void OnCollisionEnter(Collision collision)
    {
        ReflectBall(collision.GetContact(0).normal);
    }

    private void ReflectBall(Vector3 collisionNormal)
    {
        var lastSpeed = _lastFrameVelocity.magnitude;

        var reflectDir = Vector3.Reflect(_lastFrameVelocity.normalized, collisionNormal);
        var dirToTargetPos = playerTransform.position - transform.position;

        var outDir = Vector3.Lerp(reflectDir, dirToTargetPos, dirBias);

        // Debug.Log("Out Direction: " + direction + "Normal: " + direction.normalized);

        _rb.velocity = outDir.normalized * Mathf.Max(lastSpeed, reflectVel);
    }

    private void BounceBall()
    {
        // Abs used to Convert Negative Sine Wave to Positive - Stimulate Bounce
        ballVisual.position = transform.position +
                              Vector3.up * (bounceHeight *
                                            Mathf.Abs(Mathf.Sin(Time.time * bounceSpeed)));
    }
}