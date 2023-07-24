using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovementGrappling : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed;
    private float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public float swingSpeed;
    public float wallrunSpeed;
    public float slideSpeed;
    public float climbSpeed;


    public float speedIncreaseMultiplier;
    public float slopeIncreaseMultiplier;


    public float groundDrag;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;
    private float crouchNoSpeed = .1f;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public bool grounded;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    [Header("Camera Effects")]
    public PlayerCam cam;
    public PlayerCam2 cam2;



    public float grappleFov = 75;

    public float maxHalth = 100;
    public float currentHealth;

    public HealthBar healthBar;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    private PlayerMovementGrappling PlayerHp;
    private Sliding sl;

    [SerializeField] float fallThersholdVelocity = 10f;

    Vector3 moveDirection;

    Rigidbody rb;



    public MovementState state;
    public enum MovementState
    {
        restricted,
        climbing,
        sliding,
        swinging,
        walking,
        wallrunning,
        sprinting,
        crouching,
        crouchingNo,
        air
    }

    public bool sliding;
    public bool crouching;
    public bool crouchingNo;
    public bool swinging;
    public bool wallrunning;
    public bool climbing;
    public bool restricted;
    public bool walking;

    float currentHeight;

    bool isKey = false;

    private void Start()
    {
        PlayerHp = GetComponent<PlayerMovementGrappling>();
        sl = GetComponent<Sliding>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;

        currentHealth = maxHalth;
        healthBar.SetMaxHealth(maxHalth);

        startYScale = transform.localScale.y;

        Time.timeScale = 1f;
    }

    private void Update()
    {
        // ground check
        bool previousGrounded = grounded;

        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        MyInput();
        SpeedControl();
        StateHandler();
        TextStuff();
        if (!previousGrounded && grounded)
        {
            Debug.Log("do damage" + (rb.velocity.y < -fallThersholdVelocity));

            if ((rb.velocity.y < -fallThersholdVelocity))
            {
                float damage = Mathf.Abs(rb.velocity.y + fallThersholdVelocity);
                Debug.Log("do damage" + damage);
                currentHealth -= damage;
                healthBar.SetHealth((int)currentHealth);

            }
        }
        // handle drag
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }

    private void FixedUpdate()
    {
        if (state != MovementState.restricted)
            MovePlayer();  
    }
    bool isFalling { get { return (grounded && rb.velocity.y < 0); } }



    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // when to jump
        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }


        // start crouch
        if (Input.GetKey(crouchKey))
        {
            if (sliding) return;
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            crouching = true;
            isKey = true;
        }
        else if (isKey)
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
            crouching = false;
            isKey = false;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Death")
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 2f, ForceMode.Impulse);
            crouchingNo = true;
        }
        if (other.tag == "crouch")
        {
            if (sliding) return;
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            crouching = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "crouch")
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
            crouching = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (enableMovementOnNextTouch)
        {
            enableMovementOnNextTouch = false;
            ResetRestrictions();
        }
    }

    private void StateHandler()
    {
        if (restricted)
        {
            state = MovementState.restricted;
        }
        if (climbing)
        {
            state = MovementState.climbing;
            desiredMoveSpeed = climbSpeed;
        }

        else if (wallrunning)
        {
            state = MovementState.wallrunning;
            desiredMoveSpeed = wallrunSpeed;

        }

        else if (sliding)
        {
            state = MovementState.sliding;

            // increase speed by one every second
            if (OnSlope() && rb.velocity.y < 0.1f)
                desiredMoveSpeed = slideSpeed;

            else
                desiredMoveSpeed = sprintSpeed;
        }

        // Mode - Swinging
        else if (swinging)
        {
            state = MovementState.swinging;
            desiredMoveSpeed = swingSpeed;
        }

        // Mode - Crouching
        else if (crouching)
        {
            if (crouchingNo) return;

            state = MovementState.crouching;
            desiredMoveSpeed = crouchSpeed;
        }
        else if (crouchingNo)
        {

            state = MovementState.crouchingNo;
            desiredMoveSpeed = crouchNoSpeed;
        }


        // Mode - Sprinting
        else if (grounded && Input.GetKey(sprintKey))
        {
            state = MovementState.sprinting;
            desiredMoveSpeed = sprintSpeed;
        }

        // Mode - Walking
        else if (grounded)
        {
            state = MovementState.walking;
            desiredMoveSpeed = walkSpeed;
        }

        // Mode - Air
        else
        {
            state = MovementState.air;
        }

        if (Mathf.Abs(desiredMoveSpeed - lastDesiredMoveSpeed) > 4f && moveSpeed != 0)
        {
            StopAllCoroutines();
            StartCoroutine(SmoothlyLerpMoveSpeed());

            print("Lerp Started");
        }
        else
        {
            moveSpeed = desiredMoveSpeed;
        }

        lastDesiredMoveSpeed = desiredMoveSpeed;
    }
    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        float time = 0;
        float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
        float startValue = moveSpeed;

        while (time < difference)
        {
            moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);

            if (OnSlope())
            {

                float slopeAngle = Vector3.Angle(Vector3.up, slopeHit.normal);
                float slopeAngleIncrease = 1 + (slopeAngle / 90f);

                time += Time.deltaTime * speedIncreaseMultiplier * slopeIncreaseMultiplier * slopeAngleIncrease;
            }
            else
                time += Time.deltaTime * speedIncreaseMultiplier;

            yield return null;
        }

        moveSpeed = desiredMoveSpeed;
    }

    private void MovePlayer()
    {
        if (swinging) return;
 
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on slope
        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection(moveDirection) * moveSpeed * 20f, ForceMode.Force);

            if (rb.velocity.y > 0)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }

        // on ground
        else if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        // in air
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);

        // turn gravity off while on slope
        rb.useGravity = !OnSlope();
    }

    private void SpeedControl()
    {
        // limiting speed on slope
        if (OnSlope() && !exitingSlope)
        {
            if (rb.velocity.magnitude > moveSpeed)
                rb.velocity = rb.velocity.normalized * moveSpeed;
        }

        // limiting speed on ground or in air
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            // limit velocity if needed
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
        if (moveSpeed < 13)
        {
            cam.DoFov(75f);
            cam2.DoFov(75f);
        }
        else
        {
            cam.DoFov(80f);
            cam2.DoFov(80f);
        }
    }

    private void Jump()
    {
        exitingSlope = true;

        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        readyToJump = true;

        exitingSlope = false;
    }
    public bool OnSlope()
    {

        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private bool enableMovementOnNextTouch;
    public void JumpToPosition(Vector3 targetPosition, float trajectoryHeight)
    {

        velocityToSet = CalculateJumpVelocity(transform.position, targetPosition, trajectoryHeight);
        Invoke(nameof(SetVelocity), 0.1f);

        Invoke(nameof(ResetRestrictions), 3f);
    }

    private Vector3 velocityToSet;
    private void SetVelocity()
    {
        enableMovementOnNextTouch = true;
        rb.velocity = velocityToSet;

        cam.DoFov(grappleFov);
        cam2.DoFov(grappleFov);
    }

    public void ResetRestrictions()
    {

        cam.DoFov(85f);
        cam2.DoFov(85f);
    }

    public Vector3 GetSlopeMoveDirection(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }

    public Vector3 CalculateJumpVelocity(Vector3 startPoint, Vector3 endPoint, float trajectoryHeight)
    {
        float gravity = Physics.gravity.y;
        float displacementY = endPoint.y - startPoint.y;
        Vector3 displacementXZ = new Vector3(endPoint.x - startPoint.x, 0f, endPoint.z - startPoint.z);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * trajectoryHeight);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * trajectoryHeight / gravity)
            + Mathf.Sqrt(2 * (displacementY - trajectoryHeight) / gravity));

        return velocityXZ + velocityY;
    }


    #region Text & Debugging

    public TextMeshProUGUI text_speed;
    public TextMeshProUGUI text_mode;
    private void TextStuff()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (OnSlope())
            text_speed.SetText("Speed: " + Round(rb.velocity.magnitude, 1) + " / " + Round(moveSpeed, 1));

        else
            text_speed.SetText("Speed: " + Round(flatVel.magnitude, 1) + " / " + Round(moveSpeed, 1));

        text_mode.SetText(state.ToString());
    }

    public static float Round(float value, int digits)
    {
        float mult = Mathf.Pow(10.0f, (float)digits);
        return Mathf.Round(value * mult) / mult;
    }
}
    #endregion
