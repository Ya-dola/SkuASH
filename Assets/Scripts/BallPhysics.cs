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

    [SerializeField]
    private AnimationCurve ballPathCurve;

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
    private float mWallXPosOffset;

    [SerializeField]
    [Range(0f, 1f)]
    [Tooltip("0 = Regular Reflect | 1 = Reflect towards Direction")]
    private float dirBias = 0.4f;

    [Header("Raycast Details")]
    [SerializeField]
    private float rayLength = 30f;

    [SerializeField]
    private LayerMask rayLayerMask;

    // Private fields
    private Vector3 _lastFrameVelocity;
    private Rigidbody _rb;

    // Ball Flying on Hit 
    private Vector3 _collisionPos;
    private Vector3 _targetPos;

    private bool _ballHit = false;

    private void Update()
    {
        _lastFrameVelocity = _rb.velocity;
        if (!_ballHit)
            BounceBallFlight();
        else
        {
            HitBallFlight();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Wall
        if (collision.gameObject.CompareTag(TagsLayers.TagMainWall) ||
            collision.gameObject.CompareTag(TagsLayers.TagSideWall))
        {
            BallHitWall(collision);
            _ballHit = false;
        }

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

        ReflectBallBias(collision.GetContact(0).point,
            collision.GetContact(0).normal, GetPosOnMWall(), dirBias);

        AdjustBallCurveStartPos();
    }

    private void ReflectBall(Vector3 collisionNormal)
    {
        var reflectDir = Vector3.Reflect(_lastFrameVelocity.normalized, collisionNormal);

        _rb.velocity = reflectDir.normalized * Mathf.Max(_lastFrameVelocity.magnitude, reflectVel);
    }

    private void ReflectBallBias(
        Vector3 collisionPos, Vector3 collisionNormal, Vector3 targetPos, float targetBias)
    {
        var reflectDir = Vector3.Reflect(_lastFrameVelocity.normalized, collisionNormal);
        var position = transform.position;

        var dirToTargetPos = targetPos - position;

        var outDir = Vector3.Lerp(reflectDir, dirToTargetPos, targetBias);

        Physics.Raycast(collisionPos, outDir, out var hit, rayLength, rayLayerMask);

        // Debug.DrawRay(collisionPos, outDir, Color.black, 40f);

        // If Raycast Hits Main Wall
        if (hit.collider != null)
        {
            _collisionPos = collisionPos;
            _targetPos = hit.point;
            _ballHit = true;
        }

        // Debug.Log("Out Direction: " + direction + "Normal: " + direction.normalized);

        _rb.velocity = outDir.normalized * Mathf.Max(_lastFrameVelocity.magnitude, reflectVel);
    }

    private void BounceBallFlight()
    {
        // Abs used to Convert Negative Sine Wave to Positive - Stimulate Bounce upon reaching zero
        ballVisual.position = transform.position +
                              Vector3.up * (bounceHeight *
                                            Mathf.Abs(Mathf.Sin(Time.time * bounceSpeed)));
    }

    private void HitBallFlight()
    {
        var trans = transform;
        var position = trans.position;

        var ballPosZ = _collisionPos.z + (trans.localScale.z * 0.5f);

        var ballVisPosYCurve = ballPathCurve.Evaluate(
            Mathf.InverseLerp(ballPosZ, _targetPos.z, position.z));

        ballVisual.position = new Vector3(position.x, ballVisPosYCurve, position.z);
    }

    private void AdjustBallCurveStartPos()
    {
        // Adjust starting key of ballPathCurve to match height of ball on collision
        ballPathCurve.RemoveKey(0);
        ballPathCurve.AddKey(0f, ballVisual.position.y);
    }

    private Vector3 GetPosOnMWall()
    {
        return new Vector3(mWallXPosOffset.RandomRangePlusMin(),
            transform.position.y,
            mWallTransform.position.z);
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