using UnityEngine;

public class InputSysMan : MonoBehaviour
{
    [SerializeField]
    private Vector2 input;

    [SerializeField]
    private Vector3 mvmntVec3;

    [SerializeField]
    private bool isMoving;

    private InputSys _inputSys;

    // [Header("Debug")]
    // public float inputMag;

    private void Awake()
    {
        _inputSys = new InputSys();

        // When Buttons are Pressed
        _inputSys.PlayerControls.Move.performed +=
            context => { MoveUpdate(context.ReadValue<Vector2>()); };
    }

    private void MoveUpdate(Vector2 direction)
    {
        // Getting Normalized Vec2 from WASD/Arrows
        input = direction;

        // Mapping Vec2 to Vec3
        mvmntVec3.x = input.x;
        mvmntVec3.z = input.y;

        // To check if Character is moving or not
        // inputMag = input.magnitude;
        isMoving = input.magnitude != 0f;
    }

    public void SetMvmntVec3Y(float yVal)
    {
        mvmntVec3.y = yVal;
    }

    public void IncrMvmntVec3Y(float yVal)
    {
        mvmntVec3.y += yVal;
    }

    private void OnEnable()
    {
        _inputSys.PlayerControls.Enable();
    }

    private void OnDisable()
    {
        _inputSys.PlayerControls.Disable();
    }

    // Getters and Setters
    public Vector2 Input => input;

    public Vector3 MvmntVec3 => mvmntVec3;

    public bool IsMoving => isMoving;
}