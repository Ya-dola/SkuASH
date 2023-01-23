using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody))]
public class Ball : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField]
    private float bounceVelocity = 10f;

    [FormerlySerializedAs("bias")]
    [SerializeField]
    [Range(0f, 1f)]
    [Tooltip("0 = regular bounce ignoring player | 1 = direct to the player")]
    private float dirBias = 0.5f;

    [SerializeField]
    [Tooltip("Just for debugging, adds some velocity during OnEnable")]
    private Vector3 initialVelocity;

    [SerializeField]
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
    }

    private void OnCollisionEnter(Collision collision)
    {
        Bounce(collision.GetContact(0).normal);
    }

    private void Bounce(Vector3 collisionNormal)
    {
        var speed = _lastFrameVelocity.magnitude;

        var bounceDirection = Vector3.Reflect(_lastFrameVelocity.normalized, collisionNormal);
        var directionToPlayer = playerTransform.position - transform.position;

        var direction = Vector3.Lerp(bounceDirection, directionToPlayer, dirBias);

        // Debug.Log("Out Direction: " + direction + "Normal: " + direction.normalized);

        _rb.velocity = direction.normalized * Mathf.Max(speed, bounceVelocity);
    }
}