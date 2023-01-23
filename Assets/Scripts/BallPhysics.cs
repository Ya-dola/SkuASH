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
    private float _initialHeight;

    private void OnEnable()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.velocity = initialVelocity;
        _initialHeight = transform.position.y;
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

        var bounceDirection = Vector3.Reflect(_lastFrameVelocity.normalized, collisionNormal);
        var directionToPlayer = playerTransform.position - transform.position;

        var direction = Vector3.Lerp(bounceDirection, directionToPlayer, dirBias);

        // Debug.Log("Out Direction: " + direction + "Normal: " + direction.normalized);

        _rb.velocity = direction.normalized * Mathf.Max(lastSpeed, reflectVel);
    }

    private void BounceBall()
    {
        var position = transform.position;

        position = new Vector3(position.x, _initialHeight, position.z) +
                   Vector3.up * Mathf.Abs(Mathf.Sin(Time.deltaTime * bounceSpeed));

        transform.position = position;
    }
}