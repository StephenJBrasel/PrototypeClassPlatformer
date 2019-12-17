﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerControllerCC : MonoBehaviour
{
    CharacterController characterController;

    public float speed = 6.0f;
    public float jumpSpeed = 0.3f;
    public float gravity = 1.0f;

    public float fallMultiplier = 3.5f;
    public float lowJumpMultiplier = 2f;

    public float multiJumpDelay = 0.2f;
    public ushort maxJumps = 2;

    public UnityEvent OnTimeSlowEvent;
    public UnityEvent OnTimeResumeEvent;

    private ushort jumps = 0;
    private float jumpTime;
    private Vector3 moveDirection = Vector3.zero;
    public bool slowdown { get; private set; } = false; //STOP MAKING THINGS PUBLIC.
    private bool timePowerIsReset = true;
    [SerializeField]
    private float slowTime = 3.0f;
    public float getSlowTime { get { return slowTime; } private set { } }
    [SerializeField]
    private float timePowerResetTime = 3.0f;
    [SerializeField]
    private float slowspeed = 0.2f;

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

        if (characterController.isGrounded)
        {
            // We are grounded, so reset number of jumps and 
            // recalculate move direction directly from axes
            resetJumps();
            if (Input.GetButtonDown("Jump")) Jump();
        }
        else if (jumps < maxJumps && Time.unscaledTime - jumpTime > multiJumpDelay)
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

        // Move the controller
        characterController.Move(moveDirection);
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
        audioManager.Play("Jump");
        moveDirection.y = jumpSpeed;
        jumps++;
        jumpTime = Time.unscaledTime;
    }
}
