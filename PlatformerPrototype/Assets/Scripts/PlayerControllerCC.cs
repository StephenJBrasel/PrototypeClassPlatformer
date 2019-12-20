using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// A player controller that expands on the base CharacterController.
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class PlayerControllerCC : MonoBehaviour
{
    public float speed = 6.0f;
    public float jumpSpeed = 0.3f;
    public float gravity = 1.0f;

    public float fallMultiplier = 3.5f;
    public float lowJumpMultiplier = 2f;

    public float multiJumpDelay = 0.2f;
    public ushort maxJumps = 2;

    [SerializeField]
    private Animator animator = null;
    [SerializeField]
    private Transform itemLocation;
    [SerializeField]
    private LayerMask itemLayer;
    [SerializeField]
    private float slowTime = 3.0f;
    [SerializeField]
    private float timePowerResetTime = 3.0f;
    [SerializeField]
    private float slowspeed = 0.2f;

    [Header("Events")]
    public UnityEvent OnTimeSlowEvent;
    public UnityEvent OnTimeResumeEvent;

    public bool slowdown { get; private set; } = false; //STOP MAKING THINGS PUBLIC.
    public float getSlowTime { get { return slowTime; } private set { } }

    private CharacterController characterController;
    private ushort jumps = 0;
    private float jumpStartTime;
    private float itemInteractStartTime;
    private float itemInteractDelay = 0.2f;
    private Vector3 moveDirection = Vector3.zero;
    private bool timePowerIsReset = true;
    private bool wasGrounded = true;
    private bool itemHeld = false;
    private bool ItemKinematicState;


    private float fixedDeltaTime;
    private AudioManager audioManager;

    public bool resetJumps()
    {
        jumps = 0;
        return true;
    }

	private void Awake()
	{
        this.fixedDeltaTime = Time.fixedDeltaTime;
        audioManager = FindObjectOfType<AudioManager>();
        if(animator == null) animator = GetComponent<Animator>();
        animator.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

	// Start is called before the first frame update
	void Start()
    {
        characterController = GetComponent<CharacterController>();
        resetJumps();
    }

    // Update is called once per frame
    void Update()
    {
        ManageTime();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(itemLocation.position, 0.2f);
    }

    private void ManageTime()
    {
        if (Input.GetButtonDown("Fire1") && !slowdown && timePowerIsReset)
        {
            StartCoroutine(SlowTime(slowTime, timePowerResetTime));
            OnTimeSlowEvent.Invoke();
        }

        if (slowdown)
        {
            Time.timeScale = 1.0f;
			//Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;
		}

        MoveCharacter();
        ManipulateItems();

        if (slowdown)
        {
            Time.timeScale = slowspeed;
			//Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;
		}
    }

    private void MoveCharacter()
    {
		moveDirection.x = Input.GetAxisRaw("Horizontal") * speed * Time.unscaledDeltaTime;
		moveDirection.z = 0f;

		#region TURNING
		if (moveDirection.x > 0)
        {
            transform.eulerAngles = Vector3.up * 90f;
        }
        else if(moveDirection.x < 0)
        {
            transform.eulerAngles = Vector3.up * 270f;
        }
		#endregion TURNING

		animator.SetFloat("Speed", Mathf.Abs(moveDirection.x));

		#region JUMPING
		if (characterController.isGrounded)
        {
            // We are grounded, so reset number of jumps and 
            // recalculate move direction directly from axes
            if (!wasGrounded) { OnLanding(); }
            resetJumps();
            if (Input.GetButtonDown("Jump")) Jump();
        }
        else if (jumps < maxJumps && Time.unscaledTime - jumpStartTime > multiJumpDelay)
        {
            if (Input.GetButtonDown("Jump")) Jump();
        }

        //if player is moving down, speed up the player so they reach the ground faster.
        if (moveDirection.y < 0f || characterController.velocity.y < 0f)
        {
            moveDirection += (Vector3.up * -gravity * fallMultiplier * Time.unscaledDeltaTime);
        }
        // if player is moving up but no longer holding down jump, slow the player.
        else if (moveDirection.y > 0f && !Input.GetButton("Jump"))
        {
            moveDirection += (Vector3.up * -gravity * lowJumpMultiplier * Time.unscaledDeltaTime);
        }

		moveDirection.y -= (gravity * Time.unscaledDeltaTime);
		#endregion JUMPING

        wasGrounded = characterController.isGrounded;
        // Move the controller
        characterController.Move(moveDirection);
    }
    private void OnLanding()
    {
        animator.SetBool("IsJumping", false);
    }

    public IEnumerator SlowTime(float seconds, float waitTime)
    {
        slowdown = true;
        timePowerIsReset = false;
        audioManager.Play("TimeSlow");
        yield return new WaitForSecondsRealtime(seconds);
        slowdown = false;
        Time.timeScale = 1.0f;
        OnTimeResumeEvent.Invoke();
        audioManager.Play("TimeResume");
        yield return new WaitForSecondsRealtime(waitTime);
        timePowerIsReset = true;
    }

    private void Jump()
    {
        animator.SetBool("IsJumping", true);
        audioManager.Play("Jump");
        moveDirection.y = jumpSpeed;
        jumps++;
        jumpStartTime = Time.unscaledTime;
    }

    private void ManipulateItems()
    {
        if (Input.GetAxis("Fire2") > 0f && Time.unscaledTime - itemInteractStartTime > itemInteractDelay)
        {
            if (!itemHeld)
            {
                Collider[] items = Physics.OverlapSphere(itemLocation.position, 0.2f, itemLayer);
                if(items.Length > 0)
                {
                    Rigidbody rb = items[0].GetComponent<Rigidbody>();
                    if (rb)
                    {
                        ItemKinematicState = rb.isKinematic;
                        rb.isKinematic = true;
                    }
                    items[0].transform.parent = itemLocation;
                    items[0].transform.position = new Vector3 (
                        items[0].transform.position.x, 
                        itemLocation.position.y, 
                        items[0].transform.position.z);
                    itemHeld = true;
                }
            }
            else
            {
                int childCount = itemLocation.childCount;
                for (int i = 0; i < childCount; i++)
                {
                    Rigidbody rb = itemLocation.GetChild(i).GetComponent<Rigidbody>();
                    if (rb)
                    {
                        rb.isKinematic = ItemKinematicState;
                    }
                    itemLocation.GetChild(i).transform.parent = null;
                }
                itemHeld = false;
            }
            itemInteractStartTime = Time.unscaledTime;
        }
    }
}
