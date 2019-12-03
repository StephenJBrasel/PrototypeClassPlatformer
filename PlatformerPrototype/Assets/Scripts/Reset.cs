using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reset : MonoBehaviour
{

	private Vector3 startPosition;
    // Start is called before the first frame update
    void Awake()
    {
		startPosition = transform.position;
    }

	public void ResetEverything()
	{
		transform.position = startPosition;
		Rigidbody rb = GetComponent<Rigidbody>();
		if (rb)
		{
			rb.velocity = Vector3.zero;
		}
		CharacterController cc = GetComponent<CharacterController>();
		if (cc)
		{
			cc.center = startPosition;
			cc.SimpleMove(Vector3.zero);
		}
	}
}
