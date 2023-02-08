using UnityEngine;

public class GameMan : MonoBehaviour
{
    // Singleton Setup
    public static GameMan Instance { get; private set; }

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

    private BallStateEnum _ballState;
}