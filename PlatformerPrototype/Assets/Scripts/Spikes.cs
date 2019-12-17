using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
	GameObject hurtImage;
	private AudioManager audioManager;

	private void Awake()
	{
		hurtImage = GameObject.Find("HurtImage");
		audioManager = FindObjectOfType<AudioManager>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player") && !other.GetComponent<PlayerControllerCC>().slowdown)
		{
			Damage();
			other.GetComponent<Reset>().ResetEverything();
		}
	}

	private void Damage()
	{
		audioManager.Play("Damage");
		if (hurtImage == null) return;
		FadeImage fadeImage = hurtImage.GetComponent<FadeImage>();
		if (fadeImage == null) return;
		fadeImage.ResetAlpha();
	}
}
