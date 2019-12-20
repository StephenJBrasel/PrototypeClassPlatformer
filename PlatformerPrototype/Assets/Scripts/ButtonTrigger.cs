using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTrigger : MonoBehaviour
{

	public GameObject movePlatform1;
	public GameObject movePlatform2;

	void OnTriggerStay(Collider other)
	{
		GameObject.Find(movePlatform1.name).GetComponent<MovePlatform>().enabled = true;
		GameObject.Find(movePlatform2.name).GetComponent<MovePlatform>().enabled = true;
	}

	private void OnTriggerExit(Collider other)
	{
		GameObject.Find(movePlatform1.name).GetComponent<MovePlatform>().enabled = false;
		GameObject.Find(movePlatform2.name).GetComponent<MovePlatform>().enabled = false;
	}

}
