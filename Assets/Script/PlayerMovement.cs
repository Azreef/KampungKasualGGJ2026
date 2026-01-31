using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.LowLevelPhysics2D.PhysicsShape;

public class PlayerController : MonoBehaviour
{
    [Header("Player Setup")]
    public Collider2D groundCheckCollider;
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
    

    private Vector2 _moveDirection;

    public float jumpForce = 10.0f;       
    private bool isHoldingJump = false;

    private float hangTimeCounter = 0.0f;
    public float maxHangTime = 0.2f;

    private float jumpBufferCounter = 0.0f;
    public float maxJumpBufferTime = 0.2f;

    //Ground Check
    private ContactFilter2D groundContactFilter;            
    private Collider2D[] groundContactResult = new Collider2D[5]; 
    private bool isOnGround = false;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput?.actions.FindAction("Move");
        jumpAction = playerInput?.actions.FindAction("Jump");

        groundContactFilter = new ContactFilter2D();
        groundContactFilter.useTriggers = false;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _moveDirection = moveAction.ReadValue<Vector2>();
        isOnGround = CheckIsOnGround();
    }

    void FixedUpdate()
    {
        PlayerMove();
        EvaluateJump();
    }

    void EvaluateJump()
    {
        if (isOnGround)
        {
            hangTimeCounter = maxHangTime;
        }
        else
        {
            hangTimeCounter -= Time.deltaTime;
        }

        if (!isHoldingJump)
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        bool isJumping = isHoldingJump && (isOnGround || jumpBufferCounter > 0 || hangTimeCounter > 0);

        if (isJumping)
        {
            rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, jumpForce);
            hangTimeCounter = 0f;
            jumpBufferCounter = 0f;
        }

        if(!isHoldingJump && rigidBody.linearVelocity.y > 0)
        {
            rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, rigidBody.linearVelocity.y * 0.25f);
        }
    }

    void PlayerMove()
    {
        float _targetSpeed = _moveDirection.x * characterSpeed;
        float _speedDifferences = _targetSpeed - rigidBody.linearVelocity.x; 
        float _accelerationRate = (Mathf.Abs(_targetSpeed) > 0.01f) ? characterAcceleration : characterDeceleration;
        float _movement = Mathf.Pow(Mathf.Abs(_speedDifferences) * _accelerationRate, velocityPower) * Mathf.Sign(_speedDifferences);
     
        rigidBody.AddForce(new Vector2(_movement, 0f), ForceMode2D.Force);
    }

    bool CheckIsOnGround()
    {
        if (groundCheckCollider == null)
            return false;

        if (Physics2D.OverlapCollider(groundCheckCollider, groundContactFilter, groundContactResult) >= 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void JumpPressed(InputAction.CallbackContext obj)
    {
        if(isOnGround)
        {
            jumpBufferCounter = maxJumpBufferTime;
        }
        
        isHoldingJump = true;
    }

    private void JumpReleased(InputAction.CallbackContext obj)
    {
        isHoldingJump = false;
    }

    private void OnEnable()
    {
        moveAction?.Enable();
        jumpAction?.Enable();
        if (jumpAction != null)
        {
            jumpAction.started += JumpPressed;
            jumpAction.canceled += JumpReleased;
        }    
    }

    private void OnDisable()
    {
        moveAction?.Disable();
        jumpAction?.Disable();
        if (jumpAction != null)
        {
            jumpAction.started -= JumpPressed;
            jumpAction.canceled -= JumpReleased;
        }  
    }
}
