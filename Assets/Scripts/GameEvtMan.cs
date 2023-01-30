using System;
using UnityEngine;

public class GameEvtMan : MonoBehaviour
{
    // Singleton Setup
    public static GameEvtMan Instance { get; private set; }

    private void Awake()
    {
        // Singleton Setup
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    // Event Actions
    public event Action<Transform, Vector3, Vector3> ActVehicleHitBallAnim;
    public event Action<Vector3, Vector3> ActVehicleHitBall;

    // Invoking Event Actions
    public void EvtVehicleHitBallAnim(Transform trans, Vector3 vecA, Vector3 vecB)
    {
        ActVehicleHitBallAnim?.Invoke(trans, vecA, vecB);
    }

    public void EvtVehicleHitBall(Vector3 vecA, Vector3 vecB)
    {
        ActVehicleHitBall?.Invoke(vecA, vecB);
    }
}