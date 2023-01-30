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
    public event Action<Transform> ActVehicleHitBall;

    public void EvtVehicleHitBall(Transform trans)
    {
        ActVehicleHitBall?.Invoke(trans);
    }
}