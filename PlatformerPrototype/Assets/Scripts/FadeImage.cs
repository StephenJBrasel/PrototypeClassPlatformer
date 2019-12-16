using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeImage : MonoBehaviour
{
	[Range(0, 1)] [SerializeField] private float startAlpha = 0.673f;
	[SerializeField] private Image image;
	[SerializeField] private float fadeSpeed = 0.9f;

	private bool FADE_IMAGE = true;

	private PlayerControllerCC controller = null;

	void Awake()
	{
		if (image == null)
		{
			image = GetComponent<Image>();
		}

		Color tempColor = image.color;
		tempColor.a = 0f;
		image.color = tempColor;

		GameObject player = GameObject.FindGameObjectWithTag("Player");
		if (player != null) { controller = player.GetComponent<PlayerControllerCC>(); }

	}

	// Update is called once per frame
	void Update()
	{
		Color tempColor = image.color;
		if (tempColor.a > 0f && FADE_IMAGE)
		{
			float tempA = tempColor.a - (fadeSpeed * Time.unscaledDeltaTime);
			tempColor.a = Mathf.Clamp(tempA, 0f, 1f);
			image.color = tempColor;
		}
	}

	/// <summary>
	/// Pauses the fading of the fadeImage for X specified seconds.
	/// </summary>
	/// <param name="seconds">Defaults to -1f in order to check how long the playercontrollerCC slowTime is.</param>
	public void Pause(float seconds = -1f)
	{
		if (seconds == -1f)
		{
			if (controller != null) { seconds = controller.getSlowTime; }
			else { seconds = 3.0f; }
		}
		StartCoroutine(DontFadeForSeconds(seconds));
	}

	private IEnumerator DontFadeForSeconds(float seconds)
	{
		FADE_IMAGE = false;
		yield return new WaitForSecondsRealtime(seconds);
		FADE_IMAGE = true;
	}

	public void ResetAlpha()
	{
		Color tempColor = image.color;
		tempColor.a = startAlpha;
		image.color = tempColor;
	}
}
