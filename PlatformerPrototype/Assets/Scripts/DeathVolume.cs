using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathVolume : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		other.GetComponent<Reset>().ResetEverything();
		Debug.Log($"{other.name} has died.");
	}
}
