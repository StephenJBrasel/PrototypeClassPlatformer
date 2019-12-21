using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTrigger : MonoBehaviour
{

	public GameObject[] objectsToTrigger;
	public MonoBehaviour[] componentsToTrigger; 

	void OnTriggerStay(Collider other)
	{
		foreach (GameObject obj in objectsToTrigger)
		{
			obj.SetActive(true);
		}
		foreach (MonoBehaviour component in componentsToTrigger)
		{
			component.enabled = true;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		foreach (GameObject obj in objectsToTrigger)
		{
			obj.SetActive(false);
		}
		foreach (MonoBehaviour component in componentsToTrigger)
		{
			component.enabled = false;
		}
	}

}