using UnityEngine;

public class PlayerLocomotionManager : CharacterLocomotionManager
{
    [Header("Movement")]
    [SerializeField] float moveSpeed = 50;
    public float groundDrag, airDrag,
    jumpForce, jumpCooldown, airMultiplier;
    bool readyToJump = true;


    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    float startYscale;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;

    [Header("Inputs")]
    public float verticalMovement;
    public float horizontalMovement;
    [SerializeField] bool pressingCrouch;
    [SerializeField] bool pressingJump;
    private Vector3 moveDirection;
    Rigidbody rb;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        startYscale = transform.localScale.y;
    }

    public void HandleAllMovement()
    {
        HandleMovement();
        HandleGroundCheck();
        HandleDrag();
        HandleSpeedLimit();
        HandeJump();
        HandleCrouch();
    }

    private void HandleCrouch()
    {
        pressingCrouch = PlayerInputManager.instance.crouchInput;
        if (pressingCrouch)
        {
            //Crouch
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
        }
        if (!pressingCrouch)
        {
            //Crouchn't
            transform.localScale = new Vector3(transform.localScale.x, startYscale, transform.localScale.z);
        }
    }
    private void HandeJump()
    {
        pressingJump = PlayerInputManager.instance.jumpInput;
        if (pressingJump && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void ResetJump()
    {
        readyToJump = true;
    }
    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void HandleSpeedLimit()
    {
        if (OnSlope())
        {
            if (rb.velocity.magnitude > moveSpeed)
                rb.velocity = rb.velocity.normalized * moveSpeed;
        }
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }

    private void HandleDrag()
    {
        if (grounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = airDrag;
        }
    }

    private void HandleGroundCheck()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f, whatIsGround);
    }

    private void HandleMovement()
    {
        verticalMovement = PlayerInputManager.instance.verticalInput;
        horizontalMovement = PlayerInputManager.instance.horizontalInput;
        moveDirection = PlayerCamera.instance.transform.forward * verticalMovement + PlayerCamera.instance.transform.right * horizontalMovement;
        moveDirection.y = 0f;
        moveDirection.Normalize();

        if (OnSlope())
        {
            moveDirection = GetSlopeMoveDirection();
        }

        //on ground crouching
        if (grounded && pressingCrouch)
        {
            rb.AddForce(moveDirection * crouchSpeed * 10f * Time.deltaTime, ForceMode.Force);
        }
        else if (grounded)
        {
            //on ground
            rb.AddForce(moveDirection * moveSpeed * 10f * Time.deltaTime, ForceMode.Force);
        }
        //in air
        else if (!grounded)
        {
            rb.AddForce(moveDirection * moveSpeed * 10f * airMultiplier * Time.deltaTime, ForceMode.Force);
        }

        rb.useGravity = !OnSlope();
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        return false;
    }
    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }
}
