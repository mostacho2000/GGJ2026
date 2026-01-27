using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[CreateAssetMenu(menuName = "ScriptableObjects/PlayerMovementStats")]
public class PlayerMovementStats : ScriptableObject
{
    [Header("Walk")]
    [Range(0f, 1f)] public float MoveThreshold = 0.25f;
    [Range(1f, 100f)] public float maxWalkSpeed = 12.5f;
    [Range(1f, 200f)] public float runSpeed = 40f;
    [Range(0.25f, 50f)] public float groundAcceleration = 5f;
    [Range(0.25f, 50f)] public float groundDeceleration = 20f;
    [Range(0.25f, 50f)] public float airAcceleration = 5f;
    [Range(0.25f, 50f)] public float airDeceleration = 5f;
    [Range(0.25f, 50f)] public float wallJumpMoveAcceleration = 5f;
    [Range(0.25f, 50f)] public float wallJumpMoveDeceleration = 5f;

    [Header("Dash")]
    [Range(0f, 1f)] public float dashDuration = 0.11f;
    [Range(1f, 200f)] public float DashSpeed = 40f;
    [Range(0f, 1f)] public float timeBetweenDashesOnGround = 0.225f;
    public bool resetDashOnWallSlide = true;
    [Range(0, 5)] public int numberOfDashes = 2;
    [Range(0f, 0.5f)] public float dashDiagonallyBias = 0.4f;

    [Header("DashCancelTime")]
    [Range(0.01f, 5f)] public float dashGravityOnReleaseMultiplier = 1f;
    [Range(0.02f, 0.3f)] public float dashTimeForUpwardsCancel = 0.027f;

    [Header("Grounded/Collision Checks")]
    public LayerMask groundLayer;
    public float groundCheckDistance = 0.02f;
    public float headCheckDistance = 0.02f;
    [Range(0f, 1f)] public float headWidth = 0.75f;
    public float wallCheckDistance = 0.125f;
    [Range(0.01f, 2f)] public float wallCheckDistanceHeightMultiplier = 0.9f;

    [Header("Jump")]
    public float jumpHeight = 6.5f;
    [Range(1f, 1.1f)] public float jumpHeightCompensation = 1.054f;
    public float timeToJumpApex = 0.35f;
    [Range(0.01f, 5f)] public float gravityOnReleaseMultiplier = 2f;
    public float maxFallSpeed = 26f;
    [Range(1, 5)] public int maxJumpNumber = 2;

    [Header("Reset Jump Options")]
    public bool resetJumpsOnWallSlide = true;

    [Header("Jump Cut")]
    [Range(0.02f, 0.3f)] public float timeForUpwardsCancel = 0.027f;

    [Header("Jump Apex")]
    [Range(0.5f, 1f)] public float apexThreshold = 0.97f;
    [Range(0.01f, 1f)] public float apexHangTime = 0.075f;

    [Header("Jump Buffer")]
    [Range(0f, 1f)] public float jumpBufferTime = 0.125f;

    [Header("Coyote Time")]
    [Range(0f, 1f)] public float coyoteTime = 0.1f;

    [Header("Wall Slide")]
    [Min(0.01f)] public float wallSlideSpeed = 5f;
    [Range(0.25f, 50f)] public float wallSlideDeceleration = 50f;

    [Header("Wall Jump")]
    public Vector2 wallJumpDirection = new Vector2(-20f, 6.5f);
    [Range(0f, 1f)] public float wallJumpPostBufferTime = 0.125f;
    [Range(0.01f, 5f)] public float wallJumpGravityOnReleaseMultiplier = 1f;

    [Header("Debug")]
    public bool debugShowIsGroundedRays;
    public bool debugShowHitHeadRays;
    public bool debugShowWallHitBox;

    [Header("JumpVisualization Tool")]
    public bool showWalkJumpArc = false;
    public bool showDashJumpArc = false;
    public bool stopOnCollision = true;
    public bool drawRight = true;
    [Range(5, 100)] public int pointsPerArc = 20;
    [Range(0, 500)] public int visualizationSteps = 90;

    public readonly Vector2[] dashDirections = new Vector2[]
    {
        new Vector2(0,0),
        new Vector2(1,0),
        new Vector2(1,1).normalized,
        new Vector2(0,1),
        new Vector2(-1,1).normalized,
        new Vector2(-1,0),
        new Vector2(-1,-1).normalized,
        new Vector2(0,-1),
        new Vector2(1,-1).normalized
    };

    public float gravity { get; private set; }
    public float initialJumpVelocity { get; private set; }
    public float adjustedJumpHeight { get; private set; }

    public float wallJumpGravity { get; private set; }
    public float initialWallJumpVelocity { get; private set; }
    public float adjustedWallJumpHeight { get; private set; }

    private void OnValidate()
    {
        CalculateValues();
    }

    private void OnEnable()
    {
        CalculateValues();
    }

    private void CalculateValues()
    {
        adjustedJumpHeight = jumpHeight * jumpHeightCompensation;
        gravity = -(adjustedJumpHeight * 2) / Mathf.Pow(timeToJumpApex, 2f);
        initialJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;

        adjustedWallJumpHeight = wallJumpDirection.y * jumpHeightCompensation;
        wallJumpGravity = -(adjustedWallJumpHeight * 2) / Mathf.Pow(timeToJumpApex, 2f);
        initialWallJumpVelocity = Mathf.Abs(wallJumpGravity) * timeToJumpApex;
    }
}
