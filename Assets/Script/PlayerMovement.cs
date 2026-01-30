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

    public float jumpForce;
    public float maxJumpHoldTime = 0.25f;       
    public float extraJumpForce = 20f;          
    public float maxJumpVelocity = 10f;         
    private bool isHoldingJump = false;
    private float jumpHoldTimer = 0f;

    public float minJumpHeight = 1f;

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

        
        if (isHoldingJump)
        {
            
            if (jumpHoldTimer < maxJumpHoldTime)
            {
                
                float impulseThisStep = extraJumpForce * Time.fixedDeltaTime;
                rigidBody.AddForce(Vector2.up * impulseThisStep, ForceMode2D.Impulse);

                jumpHoldTimer += Time.fixedDeltaTime;

                if (maxJumpVelocity > 0f && rigidBody.linearVelocity.y > maxJumpVelocity)
                {
                    rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, maxJumpVelocity);

                    isHoldingJump = false;
                }
            }
            else
            {
                
                isHoldingJump = false;
            }
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

    private void JumpStarted(InputAction.CallbackContext obj)
    {
        if (isOnGround)
        {
           
            rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

    
            float effectiveGravity = -Physics2D.gravity.y * rigidBody.gravityScale;
            if (effectiveGravity <= 0f)
            {
                
                effectiveGravity = 9.81f;
            }

            float minJumpVelocity = Mathf.Sqrt(2f * effectiveGravity * Mathf.Max(0f, minJumpHeight));

            
            if (rigidBody.linearVelocity.y < minJumpVelocity)
            {
                rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, minJumpVelocity);
            }

            isHoldingJump = true;
            jumpHoldTimer = 0f;
        }
    }
    private void JumpEnded(InputAction.CallbackContext obj)
    {
        isHoldingJump = false;
    }

    private void OnEnable()
    {
        moveAction?.Enable();
        jumpAction?.Enable();
        if (jumpAction != null)
        {
            jumpAction.started += JumpStarted;
            jumpAction.canceled += JumpEnded;
        }    
    }

    private void OnDisable()
    {
        moveAction?.Disable();
        jumpAction?.Disable();
        if (jumpAction != null)
        {
            jumpAction.started -= JumpStarted;
            jumpAction.canceled -= JumpEnded;
        }  
    }
}
