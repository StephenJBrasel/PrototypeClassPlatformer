using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reset : MonoBehaviour
{
	private Vector3 startPosition;
	private Quaternion startRotation;
	private Vector3 startScale;
	private string levelName = "";
	// Start is called before the first frame update
	void Awake()
    {
		startPosition = transform.position;
		startRotation = transform.rotation;
		startScale = transform.localScale;
		levelName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
	}

	public void ResetEverything()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene(levelName);
		//transform.position = startPosition;
		//transform.rotation= startRotation;
		//transform.localScale = startScale;
		//CharacterController cc = GetComponent<CharacterController>();
		//if (cc)
		//{
		//	cc.velocity.Set(0, 0, 0);
		//	//cc.transform.position = startPosition;
		//	//cc.transform.rotation = startRotation;
		//	//cc.transform.localScale = startScale;
		//	return;
		//}
		//Rigidbody rb = GetComponent<Rigidbody>();
		//if (rb)
		//{
		//	rb.velocity = Vector3.zero;
		//	return;
		//}
	}
}
