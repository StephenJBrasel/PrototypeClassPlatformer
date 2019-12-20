using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerControllerRB : MonoBehaviour
{
    public Animator animator;

    public float speed = 6.0f;
	public float jumpSpeed = 8.0f;
	public float gravity = 20.0f;

	public float fallMultiplier = 3.5f;
	public float lowJumpMultiplier = 2f;

	public float multiJumpDelay = 0.2f;
	public ushort maxJumps = 2;

    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;

	[SerializeField] private float groundCheckDistance = 0.01f;
    [SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
    [SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
    const float k_GroundedRadius = .2f;                                         // Radius of the overlap circle to determine if grounded
    private bool m_Grounded;                                                    // Whether or not the player is grounded.

	private Rigidbody rb;
    private ushort jumps = 0;
	private float jumpTime;
	private Vector3 moveDirection = Vector3.zero;
	private float distToGround;
    private Vector3 m_Velocity = Vector3.zero;

    private bool slowdown = false;
	[SerializeField] private float slowTime = 1.0f;
	[SerializeField] private float slowspeed = 0.2f;

    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    public BoolEvent OnCrouchEvent;

    public bool resetJumps()
	{
		jumps = 0;
		return true;
	}

    // Start is called before the first frame update
    void Start()
    {
		distToGround = GetComponent<Collider>().bounds.extents.y;
		rb = GetComponent<Rigidbody>();
		resetJumps();
	}

    private void FixedUpdate()
    {
        bool wasGrounded = m_Grounded;
        m_Grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider[] colliders = Physics.OverlapSphere(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_Grounded = true;
                if (!wasGrounded)
                    OnLandEvent.Invoke();
            }
        }
    }

    private void DrawGizmos()
    {
        Gizmos.DrawSphere(m_GroundCheck.position, k_GroundedRadius);
    }

    // Update is called once per frame
    void Update()
    {
        ManageTime();
    }

    private void ManageTime()
    {
        if (slowdown)
        {
            Time.timeScale = 1.0f;
        }

        MoveCharacter();

        if (Input.GetButton("Fire1") && !slowdown)
        {
            StartCoroutine(SlowTime(slowTime));
        }

        if (slowdown)
        {
            Time.timeScale = slowspeed;
        }
    }

    private void MoveCharacter()
    {

        moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, 0.0f);
        moveDirection *= speed;

        animator.SetFloat("Speed", Mathf.Abs(moveDirection.x));

        if (m_Grounded)
        {
            // We are grounded, so reset number of jumps and 
            // recalculate move direction directly from axes
            resetJumps();

            if (Input.GetButtonDown("Jump"))
            {
                Jump();
                animator.SetBool("IsJumping", true);
            }
        }
        else if (jumps < maxJumps && Time.time - jumpTime > multiJumpDelay)
        {
            if (Input.GetButtonDown("Jump"))
            {
                Jump();
            }
        }

        //if player is moving down, speed up the player so they reach the ground faster.
        if (moveDirection.y < 0f)
        {
            moveDirection += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.unscaledDeltaTime;
        }
        // if player is moving up but no longer holding down jump, slow the player.
        else if (moveDirection.y > 0f && !Input.GetButton("Jump"))
        {
            moveDirection += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.unscaledDeltaTime;
        }

        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        moveDirection.y -= gravity * Time.unscaledDeltaTime;

        // Move the controller
        rb.velocity = Vector3.SmoothDamp(rb.velocity, moveDirection, ref m_Velocity, m_MovementSmoothing);
        //characterController.Move(moveDirection * Time.unscaledDeltaTime);
    }

    public void OnLanding ()
    {
        animator.SetBool("IsJumping", false);
    }

	public IEnumerator SlowTime(float seconds)
    {
        slowdown = true;
        yield return new WaitForSecondsRealtime(seconds); 
		slowdown = false;
		Time.timeScale = 1.0f;
	}

	private void Jump()
	{
		moveDirection.y = jumpSpeed;
		jumps++;
		jumpTime = Time.time;
	}
}
