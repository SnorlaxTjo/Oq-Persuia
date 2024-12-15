using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float moveSpeed = 5f;

    [Header("Rotation")]
    [SerializeField] float smoothTime = 0.1f;

    [Header("Jumping")]
    [SerializeField] float jumpHeight = 1f;
    [SerializeField] float maxJumpTime = 0.5f;
    [SerializeField] float coyoteTime = 0.2f;
    [SerializeField] float gravity = -19.84f;
    [SerializeField] float groundDistance = 1.1f;
    [SerializeField] LayerMask groundMask;

    bool isMoving;
    Vector3 velocity;

    bool moveBlock;
    bool completeMoveBlock;

    bool isGrounded;
    bool canJump;
    float coyoteTimeLeft;
    float jumpTimeLeft;
    bool isGroundPounding;

    float lastRotation;
    float currentVelocity;
    Vector3 moveDirection;

    Vector3 forward = new Vector3(0, 0, 1);
    Vector3 right = new Vector3(1, 0, 0);

    CharacterController controller;
    GroundPound groundPound;
    Animator playerAnimator;

    public bool MoveBlock { get { return moveBlock; } set {  moveBlock = value; } }
    public bool CompleteMoveBlock { get { return completeMoveBlock; } set { completeMoveBlock = value; } }
    public bool IsGrounded { get { return isGrounded; } }
    public bool IsGroundPounding { get { return isGroundPounding; } set { isGroundPounding = value; } }

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        groundPound = GetComponent<GroundPound>();
        playerAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        SetMoveAnimation();

        if (completeMoveBlock) { return; }

        Jump();

        if (moveBlock) { return; }

        GroundCheck();
        Move();
        RotatePlayer();
    }

    void GroundCheck()
    {
        isGrounded = Physics.Raycast(transform.position, -transform.up, groundDistance, groundMask);
    }

    private void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        isMoving = Mathf.Abs(x) > Mathf.Epsilon || Mathf.Abs(z) > Mathf.Epsilon;

        moveDirection = right * x + forward * z;
        moveDirection.Normalize();

        controller.Move(moveSpeed * Time.deltaTime * moveDirection);    
    }

    void SetMoveAnimation()
    {
        if (completeMoveBlock || moveBlock)
        {
            playerAnimator.SetBool("IsWalking", false);
            return;
        }

        if (playerAnimator.GetBool("IsWalking") != isMoving)
        {
            playerAnimator.SetBool("IsWalking", isMoving);
            return;
        }        
    }

    void RotatePlayer()
    {
        if (isMoving)
        {
            lastRotation = transform.localEulerAngles.y;

            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentVelocity, smoothTime);

            transform.rotation = Quaternion.Euler(0, angle, 0);
        }
        else
        {
            transform.localEulerAngles = new Vector3(0, lastRotation, 0);
        }
    }

    void Jump()
    {
        if ((isGrounded || coyoteTimeLeft > 0) && velocity.y < 0 && !isGroundPounding)
        {
            velocity.y = -2f;
        }

        if (isGroundPounding)
        {
            velocity.y = -20f;

            if (Physics.Raycast(transform.position, -transform.up, groundDistance, groundMask))
            {
                groundPound.DoGroundPoundAttack();
            }
        }

        if (!isGrounded)
        {
            coyoteTimeLeft -= Time.deltaTime;

            if (coyoteTimeLeft <= 0)
            {
                canJump = false;
            }
        }
        else
        {
            coyoteTimeLeft = coyoteTime;
            canJump = true;
        }

        if (canJump)
        {
            jumpTimeLeft = maxJumpTime;
        }

        if (Input.GetButton("Jump") && canJump && !moveBlock)
        {
            jumpTimeLeft -= Time.deltaTime;

            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            if (jumpTimeLeft <= 0 || Input.GetKeyUp(KeyCode.Space))
            {
                canJump = false;
            }
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }

        controller.Move(velocity * Time.deltaTime);
    }

    public void Teleport(Vector3 placeToTeleportTo)
    {
        controller.enabled = false;

        transform.position = placeToTeleportTo;

        controller.enabled = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - groundDistance, transform.position.z));
    }
}