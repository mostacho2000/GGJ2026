using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    public PlayerMovementStats movementStats;
    [SerializeField] private Collider2D bodyCollider;
    [SerializeField] private Collider2D feetCollider;

    private Rigidbody2D rb;

    public float horizontalVelocity { get; private set; }
    
    private bool isFacingRight;

    private RaycastHit2D groundHit;
    private RaycastHit2D headHit;
    private RaycastHit2D wallHit;
    private RaycastHit2D lastWallHit;

    private bool isGrounded;
    private bool hitHead;
    private bool isTouchingWall;

    public float verticalVelocity { get; private set; }
    private bool isJumping;
    private bool isFastFalling;
    private bool isFalling;
    private float fastFallTime;
    private float fastFallReleaseSpeed;
    public int numberOfJumps;

    private float apexPoint;
    private float timePastApexThreshold;
    private bool isPastApexThreshold;

    private float jumpBufferTimer;
    private bool jumpReleasedDuringBuffer;

    private float coyoteTimer;

    private bool isWallSliding;
    private bool isWallSlideFalling;

    private bool useWallJumpMovementStats;
    public bool isWallJumping;
    private float wallJumpTime;
    private bool isWallJumpFastFalling;
    private bool isWallJumpFalling;
    private float wallJumpFastFallTime;
    private float wallJumpFastFallReleaseSpeed;

    private float wallJumpPostBufferTimer;

    private float wallJumpApexPoint;
    private float wallJumpTimePastApexThreshold;
    private bool isPastWallJumpApexThreshold;

    private bool isDashing;
    private bool isAirDashing;
    private float dashTimer;
    private float dashOnGroundTimer;
    private int numberOfDashesUsed;
    private Vector2 dashDirection;
    private bool isDashFastFalling;
    private float dashFastFallTime;
    private float dashFastFallReleaseSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        isFacingRight = true;
    }

    private void Update()
    {
        CountTimers();
        JumpChecks();
        LandCheck();
        WallJumpCheck();

        WallSlideCheck();
        DashCheck();
    }

    private void FixedUpdate()
    {
        CollisionChecks();
        Jump();
        Fall();
        WallSlide();
        WallJump();
        Dash();

        if (isGrounded) Move(movementStats.groundAcceleration, movementStats.groundDeceleration, InputManager.movement);
        else
        {
            if(useWallJumpMovementStats) Move(movementStats.wallJumpMoveAcceleration, movementStats.wallJumpMoveDeceleration, InputManager.movement);
            else Move(movementStats.airAcceleration, movementStats.airDeceleration, InputManager.movement);
        }

        ApplyVelocity();
    }

    private void ApplyVelocity()
    {
        if(!isDashing) verticalVelocity = Mathf.Clamp(verticalVelocity, -movementStats.maxFallSpeed, 50f);
        else verticalVelocity = Mathf.Clamp(verticalVelocity, -50f, 50f);
        rb.linearVelocity = new Vector2(horizontalVelocity, verticalVelocity);
    }

    #region Movement
    private void Move(float acceleration, float deceleration, Vector2 moveInput)
    {
        if(!isDashing)
        {
            if (Mathf.Abs(moveInput.x) >= movementStats.MoveThreshold)
            {
                TurnCheck(moveInput);

                float targetVelocity = 0f;
                if (InputManager.runHeld) targetVelocity = moveInput.x * movementStats.runSpeed;
                else targetVelocity = moveInput.x * movementStats.maxWalkSpeed;

                horizontalVelocity = Mathf.Lerp(horizontalVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
            }

            else if (Mathf.Abs(moveInput.x) < movementStats.MoveThreshold) horizontalVelocity = Mathf.Lerp(horizontalVelocity, 0f, deceleration * Time.fixedDeltaTime);
        }
    }

    private void TurnCheck(Vector2 moveInput)
    {
        if (isFacingRight && moveInput.x < 0) Turn(false);
        else if (!isFacingRight && moveInput.x > 0) Turn(true);
    }

    private void Turn(bool faceRight)
    {
        if (faceRight)
        {
            isFacingRight = true;
            transform.Rotate(0f, 180f, 0f);
        }
        else
        {
            isFacingRight = false;
            transform.Rotate(0f, -180f, 0f);
        }
    }

    #endregion

    #region Jumping

    private void ResetJumpValues()
    {
        isJumping = false;
        isFalling = false;
        isFastFalling = false;
        fastFallTime = 0f;
        isPastApexThreshold = false;
    }

    public void JumpChecks()
    {
        if (InputManager.jumpPressed)
        {
            if (isWallSlideFalling && wallJumpPostBufferTimer >= 0f) return;

            else if (isWallSliding || (isTouchingWall && !isGrounded)) return;

            jumpBufferTimer = movementStats.jumpBufferTime;
            jumpReleasedDuringBuffer = false;
        }

        if (InputManager.jumpReleased)
        {
            if (jumpBufferTimer > 0f) jumpReleasedDuringBuffer = true;

            if(isJumping && verticalVelocity > 0f)
            {
                if(isPastApexThreshold)
                {
                    isPastApexThreshold = false;
                    isFastFalling = true;
                    fastFallTime = movementStats.timeForUpwardsCancel;
                    verticalVelocity = 0f;
                }

                else
                {
                    isFastFalling = true;
                    fastFallReleaseSpeed = verticalVelocity;
                }
            }
        }

        if (jumpBufferTimer > 0f && !isJumping && (isGrounded || coyoteTimer > 0f))
        {
            InitiateJump(1);

            if (jumpReleasedDuringBuffer)
            {
                isFastFalling = true;
                fastFallReleaseSpeed = verticalVelocity;
            }
        }

        else if (jumpBufferTimer > 0f && (isJumping || isWallJumping || isWallSlideFalling || isAirDashing || isDashFastFalling) && !isTouchingWall && numberOfJumps < movementStats.maxJumpNumber)
        {
            InitiateJump(1);
            isFastFalling = false;

            if(isDashFastFalling) isDashFastFalling = false;
        }

        else if (jumpBufferTimer > 0f && isFalling && !isWallSlideFalling && numberOfJumps < movementStats.maxJumpNumber - 1)
        {
            InitiateJump(2);
            isFastFalling = false;
        }
    }

    private void InitiateJump(int numberOfJumpsUsed)
    {
        if (!isJumping) isJumping = true;

        ResetWallJumpValues();

        jumpBufferTimer = 0f;
        numberOfJumps += numberOfJumpsUsed;
        verticalVelocity = movementStats.initialJumpVelocity;
    }

    private void Jump()
    {
        if (isJumping)
        {
            Debug.Log("Jumping");
            if (hitHead) isFastFalling = true;

            if (verticalVelocity >= 0f)
            {
                apexPoint = Mathf.InverseLerp(movementStats.initialJumpVelocity, 0f, verticalVelocity);

                if (apexPoint > movementStats.apexThreshold)
                {
                    if (!isPastApexThreshold)
                    {
                        isPastApexThreshold = true;
                        timePastApexThreshold = 0f;
                    }

                    if (isPastApexThreshold)
                    {
                        timePastApexThreshold += Time.fixedDeltaTime;
                        if (timePastApexThreshold < movementStats.apexHangTime) verticalVelocity = 0f;
                        else verticalVelocity = -0.01f;
                    }
                }

                else if (!isFastFalling)
                {
                    verticalVelocity += movementStats.gravity * Time.fixedDeltaTime;
                    if (isPastApexThreshold) isPastApexThreshold = false;
                }
            }

            else if (!isFastFalling) verticalVelocity += movementStats.gravity * movementStats.gravityOnReleaseMultiplier * Time.fixedDeltaTime;
            else if (verticalVelocity < 0f)
            {
                if(!isFalling) isFalling = true;
            }
        }

        if(isFastFalling)
        {
            if (fastFallTime >= movementStats.timeForUpwardsCancel) verticalVelocity += movementStats.gravity * movementStats.gravityOnReleaseMultiplier * Time.fixedDeltaTime;
            else if (fastFallTime < movementStats.timeForUpwardsCancel) verticalVelocity = Mathf.Lerp(fastFallReleaseSpeed, 0f, fastFallTime / movementStats.timeForUpwardsCancel);
            fastFallTime += Time.fixedDeltaTime;
        }
    }

    #endregion

    #region Wall Sliding

    private void WallSlideCheck()
    {
        if(isTouchingWall && !isGrounded && !isDashing)
        {
            ResetJumpValues();
            ResetWallJumpValues();
            ResetDashValues();

            if(movementStats.resetDashOnWallSlide) ResetDashes();

            if (verticalVelocity < 0f && !isWallSliding)
            {
                isWallSliding = true;
                isWallSlideFalling = false;

                if (movementStats.resetJumpsOnWallSlide) numberOfJumps = 0;
            }
        }

        else if (isWallSliding && !isTouchingWall && !isGrounded && !isWallSlideFalling)
        {
            isWallSlideFalling = true;
            StopWallSlide();
        }

        else StopWallSlide();
    }

    private void StopWallSlide()
    {
        if (isWallSliding)
        {
            numberOfJumps++;
            isWallSliding = false;
        }
    }

    private void WallSlide()
    {
        if (isWallSliding) verticalVelocity = Mathf.Lerp(verticalVelocity, -movementStats.wallSlideSpeed, movementStats.wallSlideDeceleration * Time.fixedDeltaTime);
    }

    #endregion

    #region Wall Jumping

    private void WallJumpCheck()
    {
        if (ShouldApplyPostWallJumpBuffer()) wallJumpPostBufferTimer = movementStats.wallJumpPostBufferTime;

        if(InputManager.jumpReleased && !isWallSliding && !isTouchingWall && isWallJumping)
        {
            if (verticalVelocity > 0f)
            {
                if (isPastWallJumpApexThreshold)
                {
                    isPastWallJumpApexThreshold = false;
                    isWallJumpFastFalling = true;
                    wallJumpFastFallTime = movementStats.timeForUpwardsCancel;
                    verticalVelocity = 0f;
                }

                else
                {
                    isWallJumpFastFalling = true;
                    wallJumpFastFallReleaseSpeed = verticalVelocity;
                }
            }
        }

        if(InputManager.jumpPressed && wallJumpPostBufferTimer > 0f) InitiateWallJump();
    }

    private void InitiateWallJump()
    {
        Debug.Log("Initiate Wall Jump");

        if (!isWallJumping)
        {
            isWallJumping = true;
            useWallJumpMovementStats = true;
        }

        StopWallSlide();
        ResetJumpValues();
        wallJumpTime = 0f;

        verticalVelocity = movementStats.initialWallJumpVelocity;

        int directionMultiplier = 0;
        Vector2 hitPoint = lastWallHit.collider.ClosestPoint(bodyCollider.bounds.center);

        if (hitPoint.x > transform.position.x) directionMultiplier = -1;
        else directionMultiplier = 1;

        horizontalVelocity = Mathf.Abs(movementStats.wallJumpDirection.x) * directionMultiplier;
    }

    private void WallJump()
    {

        if (isWallJumping)
        {
            Debug.Log("Wall Jumping");

            wallJumpTime += Time.fixedDeltaTime;
            if(wallJumpTime >= movementStats.timeToJumpApex) useWallJumpMovementStats = false;

            if(hitHead)
            {
                isWallJumpFastFalling = true;
                useWallJumpMovementStats = false;
            }

            if (verticalVelocity >= 0f)
            {
                wallJumpApexPoint = Mathf.InverseLerp(movementStats.wallJumpDirection.y, 0f, verticalVelocity);

                if (wallJumpApexPoint > movementStats.apexThreshold)
                {
                    if (!isPastWallJumpApexThreshold)
                    {
                        isPastWallJumpApexThreshold = true;
                        wallJumpTimePastApexThreshold = 0f;
                    }

                    if (isPastWallJumpApexThreshold)
                    {
                        wallJumpTimePastApexThreshold += Time.fixedDeltaTime;
                        if (wallJumpTimePastApexThreshold < movementStats.apexHangTime) verticalVelocity = 0f;
                        else verticalVelocity = -0.01f;
                    }
                }

                else if (!isWallJumpFastFalling)
                {
                    verticalVelocity += movementStats.wallJumpGravity * Time.fixedDeltaTime;
                    if (isPastWallJumpApexThreshold) isPastWallJumpApexThreshold = false;
                }
            }

            else if (!isWallJumpFastFalling) verticalVelocity += movementStats.wallJumpGravity * Time.fixedDeltaTime;

            else if (verticalVelocity < 0f)
            {
                if(!isWallJumpFalling) isWallJumpFalling = true;
            }
        }

        if (isWallJumpFastFalling)
        {
            Debug.Log("Wall Jump Fast Falling");

            if (wallJumpFastFallTime >= movementStats.timeForUpwardsCancel) verticalVelocity += movementStats.wallJumpGravity * movementStats.wallJumpGravityOnReleaseMultiplier * Time.fixedDeltaTime;
            else if (wallJumpFastFallTime < movementStats.timeForUpwardsCancel) verticalVelocity = Mathf.Lerp(wallJumpFastFallReleaseSpeed, 0f, wallJumpFastFallTime / movementStats.timeForUpwardsCancel);
            wallJumpFastFallTime += Time.fixedDeltaTime;
        }
    }

    private bool ShouldApplyPostWallJumpBuffer()
    {
        if(!isGrounded && (isTouchingWall || isWallSliding)) return true;
        else return false;
    }

    private void ResetWallJumpValues()
    {
        isWallSlideFalling = false;
        useWallJumpMovementStats = false;
        isWallJumping = false;
        isWallJumpFastFalling = false;
        isWallJumpFalling = false;
        isPastWallJumpApexThreshold = false;

        wallJumpFastFallTime = 0f;
        wallJumpTime = 0f;
    }

    #endregion

    #region Dashing

    private void ResetDashValues()
    {
        isDashFastFalling = false;
        dashOnGroundTimer = -0.01f;
    }

    private void ResetDashes()
    {
        numberOfDashesUsed = 0;
    }

    public void DashCheck()
    {
        if (InputManager.dashPressed)
        {
            if(isGrounded && dashOnGroundTimer < 0 && !isDashing && numberOfDashesUsed < movementStats.numberOfDashes) InitiateDash();

            else if (!isGrounded && numberOfDashesUsed < movementStats.numberOfDashes && !isDashing)
            {
                isAirDashing = true;
                InitiateDash();

                if(wallJumpPostBufferTimer > 0f)
                {
                    numberOfJumps--;
                    if(numberOfJumps < 0) numberOfJumps = 0;
                }
            }
        }
    }

    private void InitiateDash()
    {
        dashDirection = InputManager.movement;

        Vector2 closestDirection = Vector2.zero;
        float minimumDistance = Vector2.Distance(dashDirection, movementStats.dashDirections[0]);

        for (int i = 0; i < movementStats.dashDirections.Length; i++)
        {
            if(dashDirection == movementStats.dashDirections[i])
            {
                closestDirection = dashDirection;
                break;
            }

            float distance = Vector2.Distance(dashDirection, movementStats.dashDirections[i]);

            bool isDiagonal = Mathf.Abs(movementStats.dashDirections[i].x) == 1f && Mathf.Abs(movementStats.dashDirections[i].y) == 1f;
            if (isDiagonal) distance -= movementStats.dashDiagonallyBias;

            else if (distance < minimumDistance)
            {
                minimumDistance = distance;
                closestDirection = movementStats.dashDirections[i];
            }
        }

        if(closestDirection == Vector2.zero)
        {
            if(isFacingRight) closestDirection = Vector2.right;
            else closestDirection = Vector2.left;
        }

        dashDirection = closestDirection;
        numberOfDashesUsed++;
        isDashing = true;
        dashTimer = 0f;
        dashOnGroundTimer = movementStats.timeBetweenDashesOnGround;

        ResetDashes();
        ResetWallJumpValues();
        StopWallSlide();
    }

    private void Dash()
    {
        if(isDashing)
        {
            dashTimer += Time.fixedDeltaTime;
            if(dashTimer >= movementStats.dashDuration)
            {
                if (isGrounded) ResetDashes();

                isAirDashing = false;
                isDashing = false;

                if(!isJumping && !isWallJumping)
                {
                    dashFastFallTime = 0f;
                    dashFastFallReleaseSpeed = verticalVelocity;

                    if (!isGrounded) isDashFastFalling = true;
                }

                return;
            }

            horizontalVelocity = movementStats.DashSpeed * dashDirection.x;

            if (dashDirection.y != 0f || isAirDashing) verticalVelocity = movementStats.DashSpeed * dashDirection.y;
        }

        else if (isDashFastFalling)
        {
            if (verticalVelocity > 0f)
            {
                if (dashFastFallTime < movementStats.dashTimeForUpwardsCancel) verticalVelocity = Mathf.Lerp(dashFastFallReleaseSpeed, 0f, (dashFastFallTime / movementStats.dashTimeForUpwardsCancel));
                else if (dashFastFallTime >= movementStats.dashTimeForUpwardsCancel) verticalVelocity += movementStats.gravity * movementStats.dashGravityOnReleaseMultiplier * Time.fixedDeltaTime;

                dashFastFallTime += Time.fixedDeltaTime;
            }

            else verticalVelocity += movementStats.gravity * movementStats.dashGravityOnReleaseMultiplier * Time.fixedDeltaTime;
        }
    }

    #endregion

    #region Landing/Falling

    private void LandCheck()
    {
        if ((isJumping || isFalling || isWallJumpFalling || isWallJumping || isWallSlideFalling || isWallSliding || isDashFastFalling) && isGrounded && verticalVelocity <= 0f)
        {
            ResetJumpValues();
            StopWallSlide();
            ResetWallJumpValues();
            ResetDashes();

            numberOfJumps = 0;

            verticalVelocity = Physics2D.gravity.y;

            if(isDashFastFalling && isGrounded)
            {
                ResetDashValues();
                return;
            }

            ResetDashValues();
        }
    }

    private void Fall()
    {
        if (!isGrounded && !isJumping && !isWallSliding && !isWallJumping && !isDashing && !isDashFastFalling)
        {
            if (!isFalling) isFalling = true;

            verticalVelocity += movementStats.gravity * Time.fixedDeltaTime;
        }
    }

    #endregion

    #region Collision Checks
    private void IsGrounded()
    {
        Vector2 boxCastOrigin = new Vector2(feetCollider.bounds.center.x, feetCollider.bounds.min.y);
        Vector2 boxCastSize = new Vector2(feetCollider.bounds.size.x, movementStats.groundCheckDistance);

        groundHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.down, movementStats.groundCheckDistance, movementStats.groundLayer);
        if (groundHit.collider != null) isGrounded = true;
        else isGrounded = false;

        // Debug to draw box

    }

    private void IsHittingHead()
    {
        Vector2 boxCastOrigin = new Vector2(bodyCollider.bounds.center.x, bodyCollider.bounds.max.y);
        Vector2 boxCastSize = new Vector2(feetCollider.bounds.size.x * movementStats.headWidth, movementStats.headCheckDistance);

        headHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.up, movementStats.headCheckDistance, movementStats.groundLayer);

        if (headHit.collider != null) hitHead = true;
        else hitHead = false;
        // Debug to draw box
    }

    private void IsTouchingWall()
    {
        float originEndPoint = 0f;
        if (isFacingRight) originEndPoint = bodyCollider.bounds.max.x;
        else originEndPoint = bodyCollider.bounds.min.x;

        float adjustedHeight = bodyCollider.bounds.size.y * movementStats.wallCheckDistanceHeightMultiplier;

        Vector2 boxCastOrigin = new Vector2(originEndPoint, bodyCollider.bounds.center.y);
        Vector2 boxCastSize = new Vector2(movementStats.wallCheckDistance, adjustedHeight);

        wallHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, transform.right, movementStats.wallCheckDistance, movementStats.groundLayer);
        if(wallHit.collider != null)
        {
            lastWallHit = wallHit;
            isTouchingWall = true;
        }
        else isTouchingWall = false;
    }

    private void CollisionChecks()
    {
        IsGrounded();
        IsHittingHead();
        IsTouchingWall();
    }
    #endregion

    #region Timers

    private void CountTimers()
    {
        jumpBufferTimer -= Time.deltaTime;

        if (isGrounded) coyoteTimer = movementStats.coyoteTime;
        else coyoteTimer -= Time.deltaTime;

        if (!ShouldApplyPostWallJumpBuffer()) wallJumpPostBufferTimer -= Time.deltaTime;

        if (isGrounded) dashOnGroundTimer -= Time.deltaTime;
    }

    #endregion
}
