using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float moveSpeed = 8f;
    [SerializeField] float jumpForce = 16f;

    [Header("Double Jump")]
    [SerializeField] int maxJumps = 2;
    int jumpsLeft;

[Header("Wall Jump")]
[SerializeField] float wallJumpForceX = 8f;
[SerializeField] float wallJumpForceY = 14f;
[SerializeField] float wallJumpLockTime = 0.2f;
float wallJumpLockTimer;

    [Header("Wall Slide")]
    [SerializeField] float wallSlideSpeed = 1.5f;

    [Header("Wall Check")]
    [SerializeField] Transform wallCheck;
    [SerializeField] float wallCheckRadius = 0.1f;

    [Header("Ground Check")]
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckRadius = 0.15f;
    [SerializeField] LayerMask groundLayer;

    [Header("Coyote Time")]
    [SerializeField] float coyoteTime = 0.12f;
    float coyoteTimer;

    [Header("Jump Buffer")]
    [SerializeField] float jumpBufferTime = 0.1f;
    float jumpBufferTimer;
[Header("Death")]
[SerializeField] float respawnDelay = 1f;
Vector3 spawnPoint;
bool isDead;
bool inputEnabled = true;

    Rigidbody2D rb;
    Animator anim;
    bool isGrounded;
    bool isTouchingWall;
    bool facingRight = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
            spawnPoint = transform.position; // ← thêm

    }
    public void SetSpawnPoint(Vector3 pos) {
        spawnPoint = pos;
    }

    void Update()
    {
        if (isDead || !inputEnabled) return;
        CheckGround();
        CheckWall();
        HandleCoyoteTime();
        HandleJumpBuffer();
        HandleJump();
        HandleFlip();
        UpdateAnimations();
    }

    void FixedUpdate()
    {    
    if (isDead || !inputEnabled) return;
        HandleMovement();
        HandleWallSlide();
    }

    void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        if (isGrounded) jumpsLeft = maxJumps;
    }

    void CheckWall()
    {
        isTouchingWall = Physics2D.OverlapCircle(wallCheck.position, wallCheckRadius, groundLayer);
    }

    bool IsPressingIntoWall()
    {
        float x = Input.GetAxisRaw("Horizontal");
        return (facingRight && x > 0f) || (!facingRight && x < 0f);
    }

    bool IsWallSliding()
    {
        return isTouchingWall && !isGrounded && IsPressingIntoWall() && rb.linearVelocity.y <= 0f;
    }

    void HandleCoyoteTime()
    {
        if (isGrounded)
            coyoteTimer = coyoteTime;
        else
            coyoteTimer -= Time.deltaTime;
    }

    void HandleJumpBuffer()
    {
        if (Input.GetButtonDown("Jump"))
            jumpBufferTimer = jumpBufferTime;
        else
            jumpBufferTimer -= Time.deltaTime;
    }

   void HandleJump()
{
    // Wall jump → trigger DoubleJump
    if (isTouchingWall && !isGrounded && Input.GetButtonDown("Jump"))
    {
        float dir = facingRight ? -1f : 1f;
        rb.linearVelocity = new Vector2(wallJumpForceX * dir, wallJumpForceY);
        Flip();
        jumpsLeft = 0;
        jumpBufferTimer = 0f;
        wallJumpLockTimer = wallJumpLockTime;
        anim.SetTrigger("DoubleJump"); // ← thêm
        return;
    }

    // Normal + double jump
    if (jumpBufferTimer > 0f && (coyoteTimer > 0f || jumpsLeft > 0))
    {
        bool isDoubleJump = !isGrounded && coyoteTimer <= 0f; // ← thêm

        if (!isGrounded) jumpsLeft--;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        jumpBufferTimer = 0f;
        coyoteTimer = 0f;

        if (isDoubleJump) anim.SetTrigger("DoubleJump"); // ← thêm
    }

    if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0f)
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
}

    void HandleMovement()
{
    if (wallJumpLockTimer > 0f)
    {
        wallJumpLockTimer -= Time.fixedDeltaTime;
        return; // không nhận input ngang khi đang lockout
    }

    float x = Input.GetAxisRaw("Horizontal");
    rb.linearVelocity = new Vector2(x * moveSpeed, rb.linearVelocity.y);
}

    void HandleWallSlide()
    {
        // Force slide velocity để override friction khi bấm vào tường
        if (IsWallSliding())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -wallSlideSpeed);
        }
    }

    void HandleFlip()
    {
        float x = Input.GetAxisRaw("Horizontal");
        if (x > 0 && !facingRight) Flip();
        else if (x < 0 && facingRight) Flip();
    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(-transform.localScale.x, 1f, 1f);
    }

    void UpdateAnimations()
    {
        bool wallSliding = IsWallSliding();

        anim.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));
        anim.SetBool("IsGrounded", isGrounded);
        anim.SetBool("IsJumping", rb.linearVelocity.y > 0.1f && !isTouchingWall);
        anim.SetBool("IsFalling", rb.linearVelocity.y < -0.1f && !wallSliding);
        anim.SetBool("IsWall", wallSliding);
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
        if (wallCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(wallCheck.position, wallCheckRadius);
        }
    }
    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Trap" )&& !isDead)
        Die();
    }
void Die()
{
    isDead = true;
    anim.SetTrigger("Hit");
    
    // Death pop: bật người lên cao một chút
    rb.linearVelocity = new Vector2(0f, 8f);
    
    // Tắt collider để rớt xuyên qua đất
    GetComponent<CapsuleCollider2D>().enabled = false;
    
    Invoke(nameof(Respawn), respawnDelay);
}

void Respawn()
{
    transform.position = spawnPoint;
    rb.linearVelocity = Vector2.zero;
    GetComponent<CapsuleCollider2D>().enabled = true;
    isDead = false;
}
public void SetInputEnabled(bool enabled){
    inputEnabled = enabled;
    if(!enabled) rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
}
}