using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
	GameObject hurtImage;

	private void Awake()
	{
		hurtImage = GameObject.Find("HurtImage");
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			Damage();
			other.GetComponent<Reset>().ResetEverything();
		}
	}

	private void Damage()
	{
		if (hurtImage == null) return;
		FadeImage fadeImage = hurtImage.GetComponent<FadeImage>();
		if (fadeImage == null) return;
		fadeImage.ResetAlpha();
	}
}
