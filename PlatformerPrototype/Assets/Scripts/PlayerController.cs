using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	CharacterController characterController;

	public float speed = 6.0f;
	public float jumpSpeed = 8.0f;
	public float gravity = 20.0f;

	public float fallMultiplier = 3.5f;
	public float lowJumpMultiplier = 2f;

	public float multiJumpDelay = 0.2f;
	public ushort maxJumps = 2;

	private ushort jumps = 0;
	private float jumpTime;
	private Vector3 moveDirection = Vector3.zero;

	public bool resetJumps()
	{
		jumps = 0;
		return true;
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
		if (characterController.isGrounded)
		{
			// We are grounded, so reset number of jumps and 
			// recalculate move direction directly from axes
			resetJumps();
			moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
			moveDirection *= speed;

			if (Input.GetButton("Jump"))
			{
				Jump();
			}
		}
		else if (jumps < maxJumps && Time.time - jumpTime > multiJumpDelay)
		{
			if (Input.GetButton("Jump"))
			{
				Jump();
			}
		}

		//if player is moving down, speed up the player so they reach the ground faster.
		if (moveDirection.y < 0f)
		{
			moveDirection += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
		}
		// if player is moving up but no longer holding down jump, slow the player.
		else if (moveDirection.y > 0f && !Input.GetButton("Jump"))
		{
			moveDirection += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
		}

		// Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
		// when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
		// as an acceleration (ms^-2)
		moveDirection.y -= gravity * Time.deltaTime;

		// Move the controller
		characterController.Move(moveDirection * Time.deltaTime);
	}

	private void Jump()
	{
		moveDirection.y = jumpSpeed;
		jumps++;
		jumpTime = Time.time;
	}
}
