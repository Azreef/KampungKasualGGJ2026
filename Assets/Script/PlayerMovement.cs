using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Player Setup")]
    private Rigidbody2D rigidBody;

    [Header("Input Setup")]
    private InputAction moveAction;
    private InputAction jumpAction;
    private PlayerInput playerInput;

    [Header("Movement")]
    public float characterSpeed;
    public float characterAcceleration;
    public float characterDeceleration;
    public float velocityPower;
    public float jumpForce;

    private Vector2 _moveDirection;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();

        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions.FindAction("Move");
        jumpAction = playerInput.actions.FindAction("Jump");

        moveAction.Enable();
        jumpAction.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMove();
    }

    void PlayerMove()
    {
        _moveDirection = moveAction.ReadValue<Vector2>();

        float _targetSpeed = _moveDirection.x * characterSpeed;
        float _speedDifferences = _targetSpeed - rigidBody.linearVelocityX;
        float _accelerationRate = (Mathf.Abs(_targetSpeed) > 0.01f) ? characterAcceleration : characterDeceleration;
        float _movement = Mathf.Pow(Mathf.Abs(_speedDifferences) * _accelerationRate, velocityPower) * Mathf.Sign(_speedDifferences);

        rigidBody.AddForce(_movement * Vector2.right);

    }
}
