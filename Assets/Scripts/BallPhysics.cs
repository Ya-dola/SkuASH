using System;
using UnityEngine;
using UnityEngine.Serialization;

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
    [Tooltip("Just for debugging, adds some velocity during OnEnable")]
    // TODO - Adjust so Players start the balls movement towards wall 
    private Vector3 initialVelocity;

    [Header("Main Wall Details")]
    [SerializeField]
    private Transform mWallTransform;

    [SerializeField]
    private float mWallZPosOffset;

    [SerializeField]
    [Range(0f, 1f)]
    [Tooltip("0 = Regular Reflect | 1 = Reflect towards Direction")]
    private float dirBias = 0.4f;

    private Vector3 _lastFrameVelocity;
    private Rigidbody _rb;

    private void Update()
    {
        _lastFrameVelocity = _rb.velocity;

        BounceBall();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Wall
        if (collision.gameObject.CompareTag(TagsLayers.TagMainWall) ||
            collision.gameObject.CompareTag(TagsLayers.TagSideWall))
            BallHitWall(collision);

        // Player
        if (!collision.gameObject.CompareTag(TagsLayers.TagPlayer)) return;
        BallHitPlayer(collision);
    }

    private void BallHitWall(Collision collision)
    {
        if (!CheckMatchingGoLayer(TagsLayers.LayerDefault))
            ChangeGoLayer(TagsLayers.LayerDefault);

        ReflectBall(collision.GetContact(0).normal);
    }

    private void BallHitPlayer(Collision collision)
    {
        if (!CheckMatchingGoLayer(TagsLayers.LayerBallNoPlayer))
            ChangeGoLayer(TagsLayers.LayerBallNoPlayer);

        ReflectBallBias(collision.GetContact(0).normal, GetPosOnMWall(), dirBias);
    }

    private void ReflectBall(Vector3 collisionNormal)
    {
        var reflectDir = Vector3.Reflect(_lastFrameVelocity.normalized, collisionNormal);

        _rb.velocity = reflectDir.normalized * Mathf.Max(_lastFrameVelocity.magnitude, reflectVel);
    }

    private void ReflectBallBias(Vector3 collisionNormal, Vector3 targetPos, float targetBias)
    {
        var reflectDir = Vector3.Reflect(_lastFrameVelocity.normalized, collisionNormal);
        var dirToTargetPos = targetPos - transform.position;

        var outDir = Vector3.Lerp(reflectDir, dirToTargetPos, targetBias);

        // Debug.Log("Out Direction: " + direction + "Normal: " + direction.normalized);

        _rb.velocity = outDir.normalized * Mathf.Max(_lastFrameVelocity.magnitude, reflectVel);
    }

    private void BounceBall()
    {
        // Abs used to Convert Negative Sine Wave to Positive - Stimulate Bounce upon reaching zero
        ballVisual.position = transform.position +
                              Vector3.up * (bounceHeight *
                                            Mathf.Abs(Mathf.Sin(Time.time * bounceSpeed)));
    }

    private Vector3 GetPosOnMWall()
    {
        return new Vector3(mWallTransform.position.x,
            transform.position.y,
            mWallZPosOffset.RandomRangePlusMin());
    }

    private bool CheckMatchingGoLayer(string layerName)
    {
        return gameObject.layer == LayerMask.NameToLayer(layerName);
    }

    private void ChangeGoLayer(string layerName)
    {
        gameObject.layer = LayerMask.NameToLayer(layerName);
    }

    private void OnEnable()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.velocity = initialVelocity;
    }
}